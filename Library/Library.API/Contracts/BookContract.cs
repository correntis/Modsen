namespace Library.API.Contracts
{
    public class BookContract
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime? TakenAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
