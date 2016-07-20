using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.Json;

namespace Mocoding.EasyDocDb.ConsoleCrudSample
{
    public class Program
    {        
        private static async Task SaveAction(IDocument<Human> doc)
        {
            Console.WriteLine("Type your name");
            doc.Data.Name = Console.ReadLine();

            Console.WriteLine("Type your last name");
            doc.Data.LastName = Console.ReadLine();

            Console.WriteLine("Do you like this? (yes/no)");
            var s1 = Console.ReadLine();
            if (s1 == "yes")
            {
                doc.Data.HasLikeIt = true;                
            }
            else
            {
                doc.Data.HasLikeIt = false;                
            }            

            await doc.Save();


            Console.WriteLine("\nSaved");
        }

        private static void LoadAction(IDocument<Human> doc)
        {
            Console.WriteLine("\nName: " + doc.Data.Name);
            Console.WriteLine("Last Name: " + doc.Data.LastName);
            if (doc.Data.HasLikeIt == true)
            {
                Console.WriteLine("Likes it?: Yes\n");
            }
            else
            {
                Console.WriteLine("Likes it?: No\n");
            }
        }

        private static async Task DeleteAction(IDocument<Human> doc)
        {
            await doc.Delete();
            Console.WriteLine("Deleted!\n");
        }

        public static void Main(string[] args)
        {            
            IRepository repository = new Repository(new JsonSerializer());
            var task = repository.Init<Human>("HumanData.Json");
            task.Wait();
            var doc = task.Result;
                       
            string s = "";

            SaveAction(doc).Wait();
            LoadAction(doc);

            while (true)
            {
                Console.WriteLine("Type 'edit' or 'delete'");
                s = Console.ReadLine();
                if (s == "edit")
                {
                    SaveAction(doc).Wait();
                    LoadAction(doc);
                }

                if (s == "delete")
                {
                    DeleteAction(doc).Wait();
                    SaveAction(doc).Wait();
                    LoadAction(doc);
                }
            }
            
            
        }
    }
}
