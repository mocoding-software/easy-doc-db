using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.SimpleSample.Models
{
    public class Product
    {
        public Product()
        {
            ProductId = Guid.NewGuid();
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }              
    }
}
