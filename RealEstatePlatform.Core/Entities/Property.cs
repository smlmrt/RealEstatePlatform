using System.Collections.Generic;

namespace RealEstatePlatform.Core.Entities
{
    public class Property : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string RoomCount { get; set; } = string.Empty;
        public int SquareMeters { get; set; }
        public bool HasParking { get; set; }

        public Location Location { get; set; } = null!;
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    }
}