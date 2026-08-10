namespace RealEstatePlatform.Core.Entities
{
    public class PropertyImage : BaseEntity
    {
        public string ImagePath { get; set; } = string.Empty;
        public bool IsMainImage { get; set; } // Vitrin fotoğrafı mı?

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}