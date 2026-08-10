namespace RealEstatePlatform.Core.Entities
{
    public class Location : BaseEntity
    {
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

    
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!; 
    }
}