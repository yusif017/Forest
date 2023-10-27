using ForesTitle.web.DTOs;
using ForesTitle.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForesTitle.web.Data
{
    public class AppDbContext:IdentityDbContext<User>

    {
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Article> Articles { get; set; }    
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleSabTag> ArticleSabTags { get; set; }
        public DbSet<ArticleSab> ArticleSabs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleComment> ArticelCommets { get; set; }
        public DbSet<Reklam> Reklams { get; set; }



        

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            builder.Entity<SearchDto>().ToView(null);

			builder.Entity<User>().ToTable("Users");
			builder.Entity<IdentityRole>().ToTable("Roles");
		}




	}
}
