using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("api/items")] //czasownik w liczbie mnogiej
    public class APIBlogController : Controller
    {
        private ICrudBlogItemRepository items;
        public APIBlogController(ICrudBlogItemRepository items)
        {
            this.items = items;
        }

        [HttpGet]
        public List<BlogItem> GetAll()
        {
            return items.FindAll().ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public BlogItem GetOne(int id)
        {
            BlogItem blogItem = items.Find(id);
            if (blogItem != null)
            {
                return new OkObjectResult(blogItem);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult Add([FromBody] BlogItem item)
        {
            if (ModelState.IsValid)
            {
                BlogItem blogItem = items.Save(item);
                return new CreatedResult($"/api/items/{blogItem.Id}", blogItem);
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            if (items.Find(id) != null)
            {
                items.Delete(id);
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(int id, [FromBody] BlogItem item)
        {
            item.Id = id; // w przypadku gdy ktos przysle dane bez id
            BlogItem blogItem = items.Update(item);
            if (blogItem == null)
                return NotFound();
            else
            {
                return Ok();
            }
        }
    }
}
