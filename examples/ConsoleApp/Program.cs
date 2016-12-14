using System;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Json;

namespace Mocoding.EasyDocDb.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRepository repository = new Repository(new JsonSerializer());
            var task = repository.Init<User>("UserData.Json");
            task.Wait();
            var doc = task.Result;
            var s = string.Empty;

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

        private static async Task SaveAction(IDocument<User> doc)
        {
            Console.WriteLine("Type your first name");
            doc.Data.FirstName = Console.ReadLine();

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

        private static void LoadAction(IDocument<User> doc)
        {
            var docData = doc.Data;
            Console.WriteLine("\nFirstName: " + docData.FirstName);
            Console.WriteLine("Last Name: " + docData.LastName);
            if (docData.HasLikeIt == true)
            {
                Console.WriteLine("Likes it?: Yes\n");
            }
            else
            {
                Console.WriteLine("Likes it?: No\n");
            }
        }

        private static async Task DeleteAction(IDocument<User> doc)
        {
            await doc.Delete();
            Console.WriteLine("Deleted!\n");
        }
    }
}