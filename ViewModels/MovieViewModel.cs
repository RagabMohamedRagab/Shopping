namespace Bookstore.ViewModels {
    public class MovieViewModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public byte[] Img { get; set; }
    }
}
