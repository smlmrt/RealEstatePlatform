using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstatePlatform.Business.Interfaces;
using RealEstatePlatform.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstatePlatform.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public async Task<IActionResult> Index(string? city, decimal? minPrice, decimal? maxPrice, string? roomCount, bool hasParking = false)
        {
            bool isSearch = !string.IsNullOrEmpty(city) || minPrice.HasValue || maxPrice.HasValue || !string.IsNullOrEmpty(roomCount) || hasParking;

            IEnumerable<Property> properties;

            if (isSearch)
            {
                properties = await _propertyService.SearchPropertiesAsync(city, minPrice, maxPrice, roomCount, hasParking);
            }
            else
            {
                properties = await _propertyService.GetAllPropertiesAsync();
            }

            return View(properties);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Property property, List<IFormFile> imageFiles, [FromServices] Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            // Model validation pürüzlerini tamamen temizliyoruz
            ModelState.Clear();

            var city = Request.Form["Location.City"].ToString();
            var district = Request.Form["Location.District"].ToString();
            var latStr = Request.Form["Location.Latitude"].ToString();
            var lngStr = Request.Form["Location.Longitude"].ToString();

            if (property.Location == null)
            {
                property.Location = new Location();
            }

            property.Location.City = string.IsNullOrWhiteSpace(city) ? "Belirtilmedi" : city;
            property.Location.District = string.IsNullOrWhiteSpace(district) ? "Belirtilmedi" : district;

            if (double.TryParse(latStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat))
            {
                property.Location.Latitude = lat;
            }
            if (double.TryParse(lngStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lng))
            {
                property.Location.Longitude = lng;
            }

            // Fotoğraf Sıkıştırma ve Watermark İşlemi
            if (imageFiles != null && imageFiles.Count > 0)
            {
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        var imagePath = await RealEstatePlatform.Web.Helpers.ImageHelper.CompressAndAddWatermarkAsync(file, env.WebRootPath);

                        property.Images.Add(new PropertyImage
                        {
                            ImagePath = imagePath,
                            IsMainImage = property.Images.Count == 0
                        });
                    }
                }
            }

            await _propertyService.AddPropertyAsync(property);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }
    }
}