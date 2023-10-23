using AutoMapper;
using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Products;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        private readonly IMapper _mapper;
         
        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetProducts();

            var result = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            
            var productOdl = await _repository.GetProductByNo(productDto.No);

            if (productOdl != null) 
                return BadRequest($"Product No {productOdl.No} not exist !");

            var product = _mapper.Map<CatalogProduct>(productDto);
            await _repository.CreateProduct(product);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        //[Authorize]

        public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
                return NotFound();

            var updateProduct = _mapper.Map(productDto, product);

            await _repository.UpdateProduct(updateProduct);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        //[HttpPut("{id:long}")]
        ////[Authorize]
        //public async Task<IActionResult> UpdateProduct([FromQuery]long id)
        //{
        //    var product = await _repository.GetProduct(id);

        //    if (product == null)
        //        return NotFound();

        //    await _repository.DeleteAsync(product);

        //    await _repository.SaveChangesAsync();

        //    var result = _mapper.Map<ProductDto>(product);
        //    return Ok(result);
        //}

        #endregion

        #region Additional Resources
        /// <summary>
        /// get-by-product-no
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        [HttpGet("get-by-product-no/{productNo}")]
        public async Task<IActionResult> UpdateProduct([Required] string productNo)
        {
            var product = await _repository.GetProductByNo(productNo);

            if (product == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);

            return Ok(result);
        }
        #endregion
    }
}