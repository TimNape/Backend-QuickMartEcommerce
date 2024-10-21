using PragmaOnce.Core.src.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.Interfaces.Shop;

namespace PragmaOnce.Controller.src.Controllers.Shop
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReadDto>> GetProductByIdAsync([FromRoute] Guid productId)
        {
            var product = await _productService.GetOneByIdAsync(productId);
            return Ok(product);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync([FromQuery] QueryOptions options)
        {
            var products = await _productService.GetAllAsync(options);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductReadDto>> CreateProductAsync([FromForm] ProductCreateDto createDto)
        {
            var product = await _productService.CreateOneAsync(createDto);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { productId = product.Id }, product);
        }

        [HttpPatch("{productId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReadDto>> UpdateProductAsync([FromRoute] Guid productId, [FromForm] ProductUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _productService.UpdateOneAsync(productId, updateDto);
            return Ok(product);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid productId)
        {
            var result = await _productService.DeleteOneAsync(productId);
            return result ? Ok(result) : NotFound();
        }

        [HttpGet("most-purchased")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetMostPurchasedProductsAsync([FromQuery] int topNumber)
        {
            var products = await _productService.GetMostPurchased(topNumber);
            return Ok(products);
        }
    }
}