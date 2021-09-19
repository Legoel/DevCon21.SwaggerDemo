using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevCon21.SwaggerDemo.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        [JsonIgnore]
        public Category Parent { get; set; }
        public ICollection<WorkItem> WorkItems { get; set; }
    }
}
