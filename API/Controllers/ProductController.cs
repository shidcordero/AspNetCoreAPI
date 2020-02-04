using System;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Data.Models.Entities;
using Data.ViewModels.Category;
using Data.ViewModels.Common;
using Data.ViewModels.Product;
using Domain.Contracts;
using Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Helpers = API.Utilities.Helpers;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductHandler _productHandler;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IProductHandler productHandler, IMapper mapper)
        {
            _productService = productService;
            _productHandler = productHandler;
            _mapper = mapper;
        }

        /// <summary>
        /// Used to get list of product
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        [HttpGet]
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductSearchViewModel searchViewModel)
        {
            try
            {
                var result = await _productService.FindProducts(searchViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }


        /// <summary>
        /// Used to get product by id
        /// </summary>
        /// <param name="id">Holds the product id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _productService.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(new ValidationResult("Product not found."));
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }

        /// <summary>
        /// Used to create product data
        /// </summary>
        /// <param name="productViewModel">Holds the product data</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductViewModel productViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<Product>(productViewModel);

                    var validationResult = await _productHandler.CanAdd(product);
                    if (validationResult == null)
                    {
                        await _productService.Create(product);
                        return Ok("Product created successfully");
                    }

                    ModelState.AddModelError(validationResult);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }

        /// <summary>
        /// Used to update product data
        /// </summary>
        /// <param name="productViewModel">Holds the update product data</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ProductViewModel productViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<Product>(productViewModel);

                    var validationResult = await _productHandler.CanUpdate(id, product);
                    if (validationResult == null)
                    {
                        product.Id = id;
                        var result = await _productService.Update(product);
                        if (result.ValidationResults.Count == 0)
                        {
                            return Ok("Product modified successfully!");
                        }

                        ModelState.AddModelErrors(result.ValidationResults);
                    }

                    ModelState.AddModelError(validationResult);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }

        /// <summary>
        /// Used to delete product
        /// </summary>
        /// <param name="id">Holds the id of product to be deleted</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var validationResult = await _productHandler.CanDelete(id);
                if (validationResult == null)
                {
                    await _productService.DeleteById(id);

                    return Ok("Product deleted successfully!");
                }

                ModelState.AddModelError(validationResult);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }
    }
}
