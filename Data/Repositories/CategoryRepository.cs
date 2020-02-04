using Data.Base;
using Data.Contracts;
using Data.Models.Entities;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ValidationResult = Data.ViewModels.Common.ValidationResult;
using Data.ViewModels.Category;

namespace Data.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Find Category using id
        /// </summary>
        /// <param name="id">Holds the category id</param>
        /// <returns>Category Entity Model</returns>
        public async Task<Category> FindById(int id)
        {
            return await GetDbSet<Category>().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get Category List
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        /// <returns>PaginatedList</returns>
        public async Task<Extensions.PaginatedList<Category>> FindCategories(CategorySearchViewModel searchViewModel)
        {
            // gets sort direction
            var sortDir = Constants.SortDirection.Ascending;
            if ((!string.IsNullOrEmpty(searchViewModel.SortOrder) && searchViewModel.SortOrder.Equals(Constants.SortDirection.Descending)))
                sortDir = Constants.SortDirection.Descending;

            // get list of category
            var categories = GetDbSet<Category>()
                .Where(x => string.IsNullOrEmpty(searchViewModel.CategoryName.Trim()) ||
                    EF.Functions.Like(x.CategoryName, $"%{searchViewModel.CategoryName.Trim()}%"))
                .OrderByPropertyName(searchViewModel.SortBy, sortDir);

            // generate a paginated list
            return await Extensions.PaginatedList<Category>.CreateAsync(categories, searchViewModel.Page, searchViewModel.PageSize);
        }

        /// <summary>
        /// Creates Category data
        /// </summary>
        /// <param name="category">Category data</param>
        public async Task Create(Category category)
        {
            await GetDbSet<Category>().AddAsync(category);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Updates Category data
        /// </summary>
        /// <param name="category">Category data</param>
        /// <returns>UpdateCategoryViewModel</returns>
        public async Task<UpdateCategoryViewModel> Update(Category category)
        {
            // initialize view model and Category data to be updated
            var updateViewModel = new UpdateCategoryViewModel();
            var categoryUpdate = await GetDbSet<Category>().FirstOrDefaultAsync(x => x.Id == category.Id);

            try
            {
                if (categoryUpdate != null)
                {
                    // Update the RowVersion to the value when this entity was
                    // fetched. If the entity has been updated after it was
                    // fetched, RowVersion won't match the DB RowVersion and
                    // a DbUpdateConcurrencyException is thrown.
                    // A second postback will make them match, unless a new
                    // concurrency issue happens.
                    if (category.RowVersion != null && category.RowVersion.Length > 0)
                        Context.Entry(categoryUpdate).Property(Constants.Category.RowVersion).OriginalValue = category.RowVersion;

                    // update each property
                    categoryUpdate.CategoryName = category.CategoryName.Trim();
                }

                // if RowVersion does not match from database, it will throw a DbUpdateConcurrencyException
                await UnitOfWork.SaveChangesAsync();
            }
            // Catch Concurrency exception. Any exception other than this will be catch in controller
            catch (DbUpdateConcurrencyException ex)
            {
                // Get current data
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Category) exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    // if database data is null, this means data is deleted by another user.
                    updateViewModel.ValidationResults.Add(
                        new ValidationResult("Unable to save changes. The category was deleted by another user."));
                }
                else
                {
                    // Get database data
                    var databaseValues = (Category) databaseEntry.ToObject();

                    //get all properties of the class(Category)
                    var properties = databaseValues.GetType().GetProperties();

                    //iterate class properties and add to validation result list
                    updateViewModel.ValidationResults.AddRange(from prop in properties
                        where prop.Name != Constants.Category.Id &&
                              prop.Name != Constants.Category.RowVersion //exclude Id and RowVersion from the iteration
                        let dbValue = prop.GetValue(databaseValues) //get database value
                        let currentValue = prop.GetValue(clientValues) //get current value
                        where !dbValue.Equals(currentValue) //check if db and current value is not equal
                        let displayName = prop
                            .GetCustomAttributes(typeof(DisplayAttribute), false) //get display name of property
                            .Cast<DisplayAttribute>()
                            .Single()
                            .Name
                        select new ValidationResult(
                            $"{displayName} current value: {prop.GetValue(databaseValues)}")); //add error to validation result

                    updateViewModel.ValidationResults.Add(new ValidationResult("The record you attempted to edit " +
                        "was modified by another user after you got the original value. The edit operation was canceled " +
                        "and the current values in the database have been displayed. If you still want to edit this record," +
                        " click the Save button again."));

                    // Save the current RowVersion so next postback
                    // matches unless a new concurrency issue happens.
                    if (categoryUpdate != null)
                        categoryUpdate.RowVersion = updateViewModel.RowVersion = databaseValues.RowVersion;
                }
            }

            return updateViewModel;
        }

        /// <summary>
        /// Delete a Category data by Category Entity Model
        /// </summary>
        /// <param name="category">Holds the category datta</param>
        public async Task Delete(Category category)
        {
            GetDbSet<Category>().Remove(category);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a Category data by id
        /// </summary>
        /// <param name="id">Holds the category id</param>
        public async Task DeleteById(int id)
        {
            var categoryDelete = await GetDbSet<Category>().FirstOrDefaultAsync(x => x.Id == id);
            if (categoryDelete != null)
            {
                GetDbSet<Category>().Remove(categoryDelete);
            }
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if Category is still exists in the database
        /// </summary>
        /// <param name="name">Holds the category name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsCategoryExists(string name)
        {
            return await GetDbSet<Category>().AnyAsync(x =>
                x.CategoryName.ToLower() == name.ToLower().Trim()) ;
        }

        /// <summary>
        /// Checks if Category is still in used
        /// </summary>
        /// <param name="id">Holds the category id</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsCategoryInUsed(int id)
        {
            return await GetDbSet<Product>().AnyAsync(x => x.CategoryId == id);
        }
    }
}