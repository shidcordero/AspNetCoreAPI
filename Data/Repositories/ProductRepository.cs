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
using Data.ViewModels.Product;

namespace Data.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Find Product using id
        /// </summary>
        /// <param name="id">Holds the product id</param>
        /// <returns>Product Entity Model</returns>
        public async Task<Product> FindById(int id)
        {
            return await GetDbSet<Product>().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        /// <returns>PaginatedList</returns>
        public async Task<Extensions.PaginatedList<Product>> FindProducts(ProductSearchViewModel searchViewModel)
        {
            // gets sort direction
            var sortDir = Constants.SortDirection.Ascending;
            if ((!string.IsNullOrEmpty(searchViewModel.SortOrder) && searchViewModel.SortOrder.Equals(Constants.SortDirection.Descending)))
                sortDir = Constants.SortDirection.Descending;

            // get list of product
            var products = GetDbSet<Product>()
                .Where(x => string.IsNullOrEmpty(searchViewModel.ProductName.Trim()) ||
                    EF.Functions.Like(x.ProductName, $"%{searchViewModel.ProductName.Trim()}%"))
                .OrderByPropertyName(searchViewModel.SortBy, sortDir);

            // generate a paginated list
            return await Extensions.PaginatedList<Product>.CreateAsync(products, searchViewModel.Page, searchViewModel.PageSize);
        }

        /// <summary>
        /// Creates Product data
        /// </summary>
        /// <param name="product">Product data</param>
        public async Task Create(Product product)
        {
            await GetDbSet<Product>().AddAsync(product);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Updates Product data
        /// </summary>
        /// <param name="product">Product data</param>
        /// <returns>UpdateProductViewModel</returns>
        public async Task<UpdateProductViewModel> Update(Product product)
        {
            // initialize view model and Region data to be updated
            var updateViewModel = new UpdateProductViewModel();
            var productUpdate = await GetDbSet<Product>().FirstOrDefaultAsync(x => x.Id == product.Id);

            try
            {
                if (productUpdate != null)
                {
                    // Update the RowVersion to the value when this entity was
                    // fetched. If the entity has been updated after it was
                    // fetched, RowVersion won't match the DB RowVersion and
                    // a DbUpdateConcurrencyException is thrown.
                    // A second postback will make them match, unless a new
                    // concurrency issue happens.
                    if (product.RowVersion != null && product.RowVersion.Length > 0)
                        Context.Entry(productUpdate).Property(Constants.Product.RowVersion).OriginalValue = product.RowVersion;

                    // update each property
                    productUpdate.ProductName = product.ProductName.Trim();
                    productUpdate.ProductDesc = product.ProductDesc.Trim();
                    productUpdate.ProductImage = product.ProductImage;
                    productUpdate.CategoryId = product.CategoryId;
                }

                // if RowVersion does not match from database, it will throw a DbUpdateConcurrencyException
                await UnitOfWork.SaveChangesAsync();
            }
            // Catch Concurrency exception. Any exception other than this will be catch in controller
            catch (DbUpdateConcurrencyException ex)
            {
                // Get current data
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Product) exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    // if database data is null, this means data is deleted by another user.
                    updateViewModel.ValidationResults.Add(
                        new ValidationResult("Unable to save changes. The product was deleted by another user."));
                }
                else
                {
                    // Get database data
                    var databaseValues = (Product) databaseEntry.ToObject();

                    //get all properties of the class(Product)
                    var properties = databaseValues.GetType().GetProperties();

                    //iterate class properties and add to validation result list
                    updateViewModel.ValidationResults.AddRange(from prop in properties
                        where prop.Name != Constants.Product.Id &&
                              prop.Name != Constants.Product.RowVersion //exclude Id and RowVersion from the iteration
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

                    updateViewModel.ValidationResults.Add(new ValidationResult(
                        @"The record you attempted to edit was modified by another user after you got the original value.
                            The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, 
                            click the Save button again."));

                    // Save the current RowVersion so next postback
                    // matches unless a new concurrency issue happens.
                    if (productUpdate != null)
                        productUpdate.RowVersion = updateViewModel.RowVersion = databaseValues.RowVersion;
                }
            }

            return updateViewModel;
        }

        /// <summary>
        /// Delete a Product data by Product Entity Model
        /// </summary>
        /// <param name="product">Holds the product datta</param>
        public async Task Delete(Product product)
        {
            GetDbSet<Product>().Remove(product);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a Product data by id
        /// </summary>
        /// <param name="id">Holds the product id</param>
        public async Task DeleteById(int id)
        {
            var productDelete = await GetDbSet<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (productDelete != null)
            {
                GetDbSet<Product>().Remove(productDelete);
            }
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if Product is still exists in the database
        /// </summary>
        /// <param name="name">Holds the product name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsProductExists(string name)
        {
            return await GetDbSet<Product>().AnyAsync(x =>
                x.ProductName.ToLower() == name.ToLower().Trim());
        }
    }
}