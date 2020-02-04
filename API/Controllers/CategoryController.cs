using System;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Data.Models.Entities;
using Data.ViewModels.Category;
using Data.ViewModels.Common;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryHandler _categoryHandler;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, ICategoryHandler categoryHandler, IMapper mapper)
        {
            _categoryService = categoryService;
            _categoryHandler = categoryHandler;
            _mapper = mapper;
        }

        /// <summary>
        /// Used to get list of category
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        [HttpGet]
        public async Task<IActionResult> GetAllCategory([FromQuery] CategorySearchViewModel searchViewModel)
        {
            try
            {
                var result = await _categoryService.FindCategories(searchViewModel);
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
        /// Used to get category by id
        /// </summary>
        /// <param name="id">Holds the category id</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _categoryService.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(new ValidationResult("Category not found."));
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }

        /// <summary>
        /// Used to create category data
        /// </summary>
        /// <param name="categoryViewModel">Holds the category data</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(categoryViewModel);

                    var validationResult = await _categoryHandler.CanAdd(category);
                    if (validationResult == null)
                    {
                        await _categoryService.Create(category);
                        return Ok("Category created successfully");
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
        /// Used to update category data
        /// </summary>
        /// <param name="categoryViewModel">Holds the update category data</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _mapper.Map<Category>(categoryViewModel);

                    var validationResult = await _categoryHandler.CanUpdate(id, category);
                    if (validationResult == null)
                    {
                        category.Id = id;
                        var result = await _categoryService.Update(category);
                        if (result.ValidationResults.Count == 0)
                        {
                            return Ok("Category modified successfully!");
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
        /// Used to delete category
        /// </summary>
        /// <param name="id">Holds the id of category to be deleted</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var validationResult = await _categoryHandler.CanDelete(id);
                if (validationResult == null)
                {
                    await _categoryService.DeleteById(id);

                    return Ok("Category deleted successfully!");
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
