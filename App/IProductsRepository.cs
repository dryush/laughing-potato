using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mail_bank.App
{
    public interface IProductsRepository
    {
        public Task<ExistProduct> AddAsync(NewProduct product, CancellationToken cancellationToken);
        public Task<ExistProduct> GetProductAsync(long id, CancellationToken cancellationToken);
    }
}
