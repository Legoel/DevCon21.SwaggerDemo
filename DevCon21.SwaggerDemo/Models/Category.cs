using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;

namespace DevCon21.SwaggerDemo.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        [JsonIgnore]
        public Category Parent { get; set; }
        [JsonIgnore]
        public ICollection<WorkItem>? WorkItems { get; set; } = new List<WorkItem>();

        internal class CategoryExample : IExamplesProvider<Category>
        {
            public Category GetExamples()
            {
                return new Category
                {
                    Id = 12,
                    Name = "Daily chores",
                    ParentId = 6
                };
            }
        }

    }
}
