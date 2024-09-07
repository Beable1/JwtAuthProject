using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using JwtAuthProject.Core.Repositories;
using JwtAuthProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthProject.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> productService;

        public ProductController(IGenericService<Product, ProductDto> genericService)
        {
            this.productService = genericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {

            return ActionResultInstance(await productService.AddAsync(productDto));
        }

        [HttpPut]
        public  IActionResult UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance( productService.Update(productDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            
            return ActionResultInstance(await productService.Remove(id));
        }
    }
}
