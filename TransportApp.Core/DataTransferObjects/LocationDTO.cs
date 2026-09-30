namespace TransportApp.Core.DataTransferObjects
{
    public class LocationDTO
    {
        public Guid Id { get; set; }
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string Adress { get; set; } = default!;
    }
}
