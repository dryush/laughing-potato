using mail_bank.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace mail_bank.Repositories
{
    public class JsonFileProductRepository : SimpleFileProductsRepository
    {
        public JsonFileProductRepository(FilePathFactory filePathFactory) 
            : base(filePathFactory, ReadAsync, WriteAsync)
        {
        }

        private static async Task<ICollection<ExistProduct>> ReadAsync(string path)
        {
            using (var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                //string content = await file.ReadToEndAsync();
                var db = file.Length > 0 ? await JsonSerializer.DeserializeAsync<ExistProduct[]>(file) : null;
                return db?.ToList() ?? new List<ExistProduct>();
            }
        }


        private static async Task WriteAsync(string path, IEnumerable<ExistProduct> db)
        {
            using (var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await JsonSerializer.SerializeAsync<IEnumerable<ExistProduct>>(file, db);
                await file.FlushAsync();
                file.Close();
            }
        }
    }
}
