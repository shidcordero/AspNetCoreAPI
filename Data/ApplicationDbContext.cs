using Data.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Function to modify database structure or seed data
        /// </summary>
        /// <param name="modelBuilder">Instance of modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}