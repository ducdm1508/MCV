namespace bai4.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Director { get; set; }
        public string Trailer { get; set; }     
        public string WatchLink { get; set; }   
        public string Poster { get; set; }
    }
}
