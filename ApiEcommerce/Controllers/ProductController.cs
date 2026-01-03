using ApiEcommerce.Mapping;
using ApiEcommerce.Models.Dtos.Product;
using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;    
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts().ToList();
            List<ProductDto> productsDto = new List<ProductDto>();
            foreach (var p in products)
            {
                var product = p.ToDto();
                productsDto.Add(product);
            }
            return Ok(productsDto);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductById(int productId)
        {
            var product = _productRepository.GetProductById(productId);
            if (product == null) return NotFound("Product not found");
            return Ok(product.ToDto());
        }

        [HttpGet("GetProductByCategory/{categoryId:int}", Name = "GetProductByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProductByCategoryId(int categoryId)
        {
            if(categoryId <= 0 ) return BadRequest("CategoryId cannot be negative");

            var products = _productRepository.GetProductsByCategory(categoryId).ToList();
            if(products.Count == 0) return NotFound("Products not found");
            var productsDto = new List<ProductDto>();
            foreach (var p in products)
            {
                var productDto = p.ToDto();
                productsDto.Add(productDto);
            }
            return Ok(productsDto);
        }

        
        [HttpGet("searchProductByTerm/{searchTerm}", Name = "SearchProductByDescription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SearchProductByDescription(string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm)) return BadRequest("searchTerm cannot be empty");
            var products = _productRepository.SearchProducts(searchTerm).ToList();
            if (products.Count == 0) return NotFound("Products not found");
            var productsDto = new List<ProductDto>();
            foreach (var p in products)
            {
                var productDto = p.ToDto();
                productsDto.Add(productDto);
            }
            return Ok(productsDto);
        }
        
        [HttpPost]
        public IActionResult CreateProduct(int ProductId, [FromBody] CreateProductDto productDto )
        {
            if(productDto == null)  return BadRequest(ModelState);
            if(_productRepository.ProductExistsByName(productDto.Name)) return BadRequest("Product name already exists");
            if (!_categoryRepository.CategoryExists(productDto.CategoryId)) return BadRequest("Category does not exist");
            var product = productDto.ToEntity();
            if (!_productRepository.CreateProduct(product))
            {
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetProduct", new { productId = product.ProductId }, product);
        }

        [HttpPatch("buyProduct/{name}/{quantity:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuyProduct(string name, int quantity)
        {
            if(quantity <= 0) return BadRequest("Quantity cannot be negative");
            if(string.IsNullOrEmpty(name)) return BadRequest("name cannot be empty");
            var foundProduct = _productRepository.ProductExistsByName(name);
            if(!foundProduct) return NotFound("Product not found");

            if (!_productRepository.BuyProduct(name, quantity))
            {
                return BadRequest("Not enough stock");
            }
            var units = quantity == 1 ? "unit" : "units";
            return Ok($"Successfully buy {quantity} {units} of {name}");
        }

        [HttpPut("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateProduct(int productId, [FromBody] UpdateProductDto productDto)
        {
            if(productId <= 0) return BadRequest("ProductId cannot be negative");
            var foundProduct = _productRepository.GetProductById(productId);
            if(foundProduct == null) return NotFound("Product not found");
            
            productDto.UpdateEntity(foundProduct);
            if(!_productRepository.UpdateProduct(foundProduct)) return NotFound("Product not found");
            
            return NoContent();
        }

        [HttpDelete("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int productId)
        {
            if(productId <= 0) return BadRequest("ProductId cannot be negative");
            var product = _productRepository.GetProductById(productId);
            if(product == null) return NotFound("Product not found");
            _productRepository.DeleteProduct(product);
            return NoContent();
        }
    }
}
