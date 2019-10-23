using System;
namespace ASPInClass.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int OnSale { get; set; }
        public string StockLevel { get; set; }
        public int CategoryID { get; set; }
    }
}
