using System.Text.Json.Serialization;

namespace DevCon21.SwaggerDemo.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        [JsonIgnore]
        public ICollection<WorkItem> WorkItems { get; set; }
    }
}
