using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class EFBlogItemRepository : IBlogItemRepository
    {
        private ApplicationDbContext _applicationDbContext;
        
        public EFBlogItemRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IQueryable<BlogItem> BlogItems => _applicationDbContext.BlogItems;
    }
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options){ }
        public DbSet<BlogItem> BlogItems { get; set; }
    }
    public interface IBlogItemRepository
    {
        public IQueryable<BlogItem> BlogItems { get; }
    }
    public class BlogItem
    {

        // id powinno byc niezmienne, nadaje pole id rekordom w bazie
        // id powinno być ukryte HiddenInput ukrywa dane przed uzytkownikiem
        [HiddenInput]
        public int Id { get; set; } 
        
        [Required(ErrorMessage = "Musisz podać tytuł")] 
        public string Title { get; set; }

        // adnotacje walidujące
        [Required(ErrorMessage ="Musisz wpisać treść")]
        [MinLength(5,ErrorMessage = "Treść powinna mieć conajmniej 5 znaków")]
        public string Content { get; set; }
        
        public DateTime CreationTimestamp { get; set; }
    }
}
