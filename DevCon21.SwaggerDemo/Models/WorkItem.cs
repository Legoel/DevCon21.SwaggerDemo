using System.Text.Json.Serialization;

namespace DevCon21.SwaggerDemo.Models
{
    public class WorkItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public long CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public long PersonId { get; set; }
        [JsonIgnore]
        public Person Person { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
    }
}
