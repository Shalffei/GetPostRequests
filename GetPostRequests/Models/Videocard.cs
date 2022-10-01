using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPostRequests.Models
{
    public class Videocard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ReferenceToCharacteristics { get; set; }
        public List<Characteristics> CharacteristicsList { get; set; }
    } 
}
