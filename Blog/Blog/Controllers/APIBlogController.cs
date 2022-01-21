using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/items")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ApiBlogController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public ActionResult<BlogItem> Get(int id)
        {
        }

        public class MyExceptionAttribute : ExceptionFilterAttribute
        {
            public override void OnException(ExceptionContext context)
            {
                if (context.Exception is MyException)
                {
                    var body = new Dictionary<string, Object>();
                    body["error"] = context.Exception.Message;
                    context.Result = new BadRequestObjectResult(body);
                }
            }
        }

        public class MyException : Exception
        {
            public MyException(string? message) : base(message)
            {
            }
        }

        [HttpGet()]
        [Route("{id}")]
        [MyException]

        public class BasicAuthorizationFilter : IAuthorizationFilter
        {
            private const string USERNAME = "admin";
            private const string PASSWORD = "1234";
            private const string Realm = "App Realm";
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.Request.Headers.Keys.Contains(HeaderNames.Authorization))
                {
                    context.HttpContext.Response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
                    context.Result = new UnauthorizedResult();
                    return;
                }
                [HttpGet()]
                [Route("{id}")]
                [MyException]
                public IActionResult Get(int? id)
                {
                    if (id == null)
                    {
                        throw new MyException("Brak identyfikatora zasobu!");
                    }
                 }
                string authenticationToken =
               context.HttpContext.Request.Headers[HeaderNames.Authorization];
                authenticationToken = authenticationToken.Split(" ")[1].Trim();
                string decodedAuthenticationToken =
               Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];
                if (Validate(username, password))
                {
                    var identity = new GenericIdentity(username);
                    IPrincipal principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            public static bool Validate(string username, string password)
            {
                return USERNAME.Equals(username) && PASSWORD.Equals(password);
            }
        }


        private ICrudBlogItemRepository items;

        public ApiBlogController(ICrudBlogItemRepository items)
        {
            this.items = items;
        }

        [HttpGet]
        public IList<BlogItem> GetAll()
        {
            return items.FindAll();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOne(int id)
        {
            BlogItem blogItem = items.Find(id);
            if (blogItem != null)
            {
                return new OkObjectResult(blogItem);
            }
            else
            {
                return NotFound();
            }
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
            {
                return BadRequest();
            }
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
            {
                return NotFound();
            }

        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(int id, [FromBody] BlogItem item)
        {
            item.Id = id;
            BlogItem blogItem = items.Update(item);
            if (blogItem == null)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

    }
}
