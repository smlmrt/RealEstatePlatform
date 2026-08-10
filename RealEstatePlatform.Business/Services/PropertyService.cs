using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstatePlatform.Business.Interfaces;
using RealEstatePlatform.Core.Entities;
using RealEstatePlatform.Core.Interfaces;

namespace RealEstatePlatform.Business.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepository<Property> _repository;

        public PropertyService(IRepository<Property> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Property?> GetPropertyByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddPropertyAsync(Property property)
        {
            await _repository.AddAsync(property);
        }

        public void UpdateProperty(Property property)
        {
            _repository.Update(property);
        }

        public void DeleteProperty(Property property)
        {
            _repository.Delete(property);
        }

        public async Task<IEnumerable<Property>> SearchPropertiesAsync(string? city, decimal? minPrice, decimal? maxPrice, string? roomCount, bool hasParking)
        {
            var properties = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(city))
            {
                properties = properties.Where(p => p.Location != null && 
                                                 p.Location.City.Contains(city, System.StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(roomCount))
            {
                properties = properties.Where(p => p.RoomCount == roomCount);
            }

            if (hasParking)
            {
                properties = properties.Where(p => p.HasParking);
            }

            return properties;
        }
    }
}