using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;

namespace CloudDatastore
{
    class Program
    {
        // Google Cloud Platform project
        static string projectId = "cloudstoragesandbox";
        static DatastoreDb dsdb = null;
        static string dsNamespace = "falafelSurvey";

        static void Main(string[] args)
        {
            dsdb = DatastoreDb.Create(projectId, dsNamespace);
            if (!HasEntities())
            {
                StoreEntities();
            }
            Console.WriteLine("Enter a category to query questions: product, purchase, quality or price");
            var category = Console.ReadLine();
            var resultCount = QueryEntitiesCategory(category);
            Console.WriteLine($"{resultCount} in category {category}");

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }

        /// <summary>
        /// Store some sample survey questions
        /// </summary>
        static void StoreEntities()
        {
            var kFactory = dsdb.CreateKeyFactory("SurveyQuestion");
            var entities = new List<Entity>()
            {
                new Entity()
                {
                    Key = kFactory.CreateIncompleteKey(),
                    ["questiontype"] = "yesno",
                    ["text"] = "Would you purchase this product again?",
                    ["categories"] = new ArrayValue() {Values = {"product", "purchase"}},
                },
                new Entity()
                {
                    Key = kFactory.CreateIncompleteKey(),
                    ["questiontype"] = "starrating",
                    ["text"] = "How would you rate the quality of our product?",
                    ["categories"] = new ArrayValue() {Values = {"product", "quality"}},
                },
                new Entity()
                {
                    Key = kFactory.CreateIncompleteKey(),
                    ["questiontype"] = "starrating",
                    ["text"] = "How would you rate the price of our product?",
                    ["categories"] = new ArrayValue() {Values = {"product", "price"}},
                }
            };

            dsdb.Upsert(entities);
        }

        static int QueryEntitiesCategory(string category)
        {
            Query query = new Query("SurveyQuestion")
            {
                Filter = Filter.Equal("categories", category)
            };

           var results = dsdb.RunQuery(query).Entities;
            return results.Count;
        }

        static bool HasEntities()
        {
            return dsdb.RunQuery(new Query("SurveyQuestion")).Entities.Count > 0;
        }
    }
}
