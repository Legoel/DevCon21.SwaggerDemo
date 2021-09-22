using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DevCon21.SwaggerDemo.Models
{
    /// <summary>
    /// A person that can have work items
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The person identifier
        /// </summary>
        /// <example>42</example>
        public long Id { get; set; }
        /// <summary>
        /// The person full name
        /// </summary>
        /// <example>Anna Conda</example>
        [Required]
        public string FullName { get; set; }
        /// <summary>
        /// The person's workitems
        /// </summary>
        [JsonIgnore]
        public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    }
}
