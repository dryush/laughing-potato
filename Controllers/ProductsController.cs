using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mail_bank.App;
using mail_bank.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mail_bank.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _repository;

        public ProductsController(IProductsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ExistProduct>> Get(long id, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductAsync(id, cancellationToken);
            if (product == null)
                return NotFound();
            return product;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add(NewProduct product, CancellationToken cancellationToken)
        {
            await _repository.Add(product, cancellationToken);
            return Ok();
        }

    }
}