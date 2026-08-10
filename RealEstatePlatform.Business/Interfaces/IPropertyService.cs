using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstatePlatform.Core.Entities;

namespace RealEstatePlatform.Business.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task<Property?> GetPropertyByIdAsync(int id);
        Task AddPropertyAsync(Property property);
        
        void UpdateProperty(Property property);
        void DeleteProperty(Property property);

        Task<IEnumerable<Property>> SearchPropertiesAsync(string? city, decimal? minPrice, decimal? maxPrice, string? roomCount, bool hasParking);
    }
}