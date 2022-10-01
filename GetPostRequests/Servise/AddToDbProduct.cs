using GetPostRequests.EntityFramework;
using GetPostRequests.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GetPostRequests.Servise
{
    public class AddToDbProduct
    {
        public void AddToDb (ApplicationDb db, List<Videocard> videocards)
        {
            foreach (var item in videocards)
            {
                Product product = new Product();
                product.IdRozetka = item.Id;
                product.Price = item.Price;
                product.ProductRozetkaId = "c80087";
                product.Characteristics = JsonSerializer.Serialize(item.CharacteristicsList);
                product.CountryMade = item.CharacteristicsList.Where(x => x.CharacteristicsName == "Країна-виробник").Select(x => x.CharacteristicsDescription).FirstOrDefault();
                product.Name = item.Name;
                product.ProductCategoryName = "Videocard";

                db.AddRange(product);
            }
            db.SaveChanges();
        }
    }
}
