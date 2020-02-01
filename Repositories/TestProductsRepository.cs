using MailBank.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailBank.Repositories
{
    public class TestProductsRepository : IProductsRepository
    {
        private Dictionary<long, ExistProduct> _db = new Dictionary<long, ExistProduct>();

        private long _GetFreeId()
        {
            return _db.LongCount() + 1;
        }
        public Task<ExistProduct> AddAsync(NewProduct product, CancellationToken cancellationToken = default) {
            var existProduct = new ExistProduct(
                _GetFreeId(),
                product.Name,
                product.Description
            );
            _db.Add(existProduct.Id ,existProduct);
            return Task.FromResult(existProduct);
        }

        public Task<ExistProduct> GetProductAsync(long id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _db.GetValueOrDefault(id, null)
            );
        }
    }
}
