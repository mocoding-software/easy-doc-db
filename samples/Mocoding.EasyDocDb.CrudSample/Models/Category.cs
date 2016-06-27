using System;

namespace Mocoding.EasyDocDb.CrudSample.Models
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
                                                       