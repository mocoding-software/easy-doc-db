using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.SimpleSample.Models
{
    public class Category
    {      
        public Category()
        {
            CategoryId = Guid.NewGuid();    
        }
           
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }             
    }     
}
                                                       