using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public interface ICustomerBlogItemRepository
    {
        IList<BlogItem> FindByName(string namePattern);
        IList<BlogItem> FindPage(int page, int size);
        BlogItem FindById(int id);
    }

    class CustomerBlogItemRepository : ICustomerBlogItemRepository
    {
        private ApplicationDbContext context;
        public CustomerBlogItemRepository(ApplicationDbContext applicationDbContext)
        {
            context = applicationDbContext;
        }

        public IList<BlogItem> FindByName(string namePattern)
        {
            return (from p in context.BlogItems where p.Title.Contains(namePattern) select p).ToList();
        }

        public IList<BlogItem> FindPage(int page, int size)
        {
            return (from p in context.BlogItems select p).OrderBy(p => p.Title).Skip((page - 1) * size).Take(size).ToList();
        }
        public BlogItem FindById(int id)
        {
            return context.BlogItems.Find(id);
        }
    }
    public interface ICrudBlogItemRepository
    {
        BlogItem Find(int id);
        BlogItem Delete(int id);
        BlogItem Add(BlogItem blogItem);
        BlogItem Update(BlogItem blogItem);
        void Delete(object id);
        IList<BlogItem> FindAll();
        BlogItem Save(BlogItem item);
    }

    class CrudBlogItemRepository : ICrudBlogItemRepository
    {
        private ApplicationDbContext _context;
        public CrudBlogItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public BlogItem Find(int id)
        {
            return _context.BlogItems.Find(id);
        }

        public BlogItem Delete(int id)
        {
            var blogItem = _context.BlogItems.Remove(Find(id)).Entity;
            _context.SaveChanges();
            return blogItem;
        }

        public BlogItem Add(BlogItem blogItem)
        {
            var entity = _context.BlogItems.Add(blogItem).Entity;
            _context.SaveChanges();
            return entity;
        }

        public BlogItem Update(BlogItem blogItem)
        {
            var entity = _context.BlogItems.Update(blogItem).Entity;
            _context.SaveChanges();
            return entity;
        }

        public IList<BlogItem> FindAll()
        {
            return _context.BlogItems.ToList();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public BlogItem Save(BlogItem item)
        {
            throw new NotImplementedException();
        }
    }
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
