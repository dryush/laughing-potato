﻿using mail_bank.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace mail_bank.Repositories
{
    public class SimpleFileProductsRepository : IProductsRepository
    {
        public delegate string FilePathFactory();
        private readonly FilePathFactory _filePathFactory;

        public delegate Task<ICollection<ExistProduct>> FileReader(string path);  
        private FileReader _reader;

        public delegate Task FileWriter(string path, IEnumerable<ExistProduct> products);
        private FileWriter _writer;

        public SimpleFileProductsRepository(FilePathFactory filePathFactory, FileReader reader, FileWriter writer)
        {
            _filePathFactory = filePathFactory;
            _reader = reader;
            _writer = writer;
        }

        public async Task<ExistProduct> AddAsync(NewProduct product, CancellationToken cancellationToken) 
        {
            var uri = this._filePathFactory.Invoke();
            var db = await _reader.Invoke(uri) ?? new List<ExistProduct>();
            var nextId = db.Any() ? db.Max(v => v.Id) + 1 : 1;
            var pr = new ExistProduct(nextId, product.Name, product.Description);
            db.Add(pr);
            await _writer(uri, db);
            return pr;
        }
        public async Task<ExistProduct> GetProductAsync(long id, CancellationToken cancellationToken)
        {
            var uri = this._filePathFactory.Invoke();
            var db = await _reader.Invoke(uri) ?? new List<ExistProduct>();
            return db.FirstOrDefault(f => f.Id == id);
        }
    }

}
