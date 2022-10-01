using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPostRequests.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int? IdRozetka { get; set; } 
        public string? ProductRozetkaId { get; set; }
        public int? YearMade { get; set; }
        public string? CountryMade { get; set; }
        public string? ProductCategoryName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? BuyCount { get; set; }
        public string Characteristics { get; set; }
        public List<Order>? ProductOrders { get; set; }
    }
}
