namespace TransportApp.Core.Entities
{
    public class Location : BaseEntity
    {
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Adress { get; set; }
    }
}
