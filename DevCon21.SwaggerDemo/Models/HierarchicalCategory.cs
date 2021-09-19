using System.Collections.Generic;

namespace DevCon21.SwaggerDemo.Models
{
    public class HierarchicalCategory
    {
        public HierarchicalCategory()
        {
        }

        public HierarchicalCategory(Category item)
        {
            Item = item;
        }

        public Category Item { get; set; }
        public List<HierarchicalCategory> Children { get; set; } = new();
    }
}
