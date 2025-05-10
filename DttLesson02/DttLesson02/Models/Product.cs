namespace DttLesson02.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public required string ProductName { get; set; }
        public int YearRelease { get; set; }
        public double Price { get; set; }
    }
      
}
