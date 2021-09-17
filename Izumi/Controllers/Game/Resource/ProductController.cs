using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izumi.Services.Game.Product.Commands;
using Izumi.Services.Game.Product.Models;
using Izumi.Services.Game.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Izumi.Controllers.Game.Resource
{
    [ApiController, Route("game/resource/product")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetProductQuery(id)));
        }

        [HttpGet, Route("list")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            return Ok(await _mediator.Send(new GetProductsQuery()));
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost, Route("update")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(UpdateProductCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
