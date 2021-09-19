using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevCon21.SwaggerDemo.Models;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CategoryTreeController : ControllerBase
    {
        private readonly TodoListContext _context;

        public CategoryTreeController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/CategoriesTree
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<HierarchicalCategory>> GetCategoriesTree()
        {
            var allCategories = await _context.Categories.ToListAsync();

            Dictionary<long, HierarchicalCategory> tree = allCategories.ToDictionary(c => c.Id, c => new HierarchicalCategory(c));

            foreach (var node in tree.Values)
            {
                if (node.Item.Parent?.Id is not null)
                {
                    tree[node.Item.Parent.Id].Children.Add(node);
                }
            }

            return tree.Values.SingleOrDefault(node => node.Item.Parent is null);
        }
    }
}
