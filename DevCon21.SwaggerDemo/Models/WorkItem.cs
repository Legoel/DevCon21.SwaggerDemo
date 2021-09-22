using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;

namespace DevCon21.SwaggerDemo.Models
{
    /// <summary>
    /// A unit of work
    /// </summary>
    public class WorkItem
    {
        /// <summary>
        /// The work item identifier
        /// </summary>
        /// <example>12</example>
        public long Id { get; set; }
        /// <summary>
        /// The work item name
        /// </summary>
        /// <example>Do the homeworks</example>
        public string Name { get; set; }
        /// <summary>
        /// Indicates if the work item is completed
        /// </summary>
        /// <example>true</example>
        public bool IsComplete { get; set; }
        /// <summary>
        /// The identifier of the category to which the task belongs
        /// </summary>
        /// <example>26</example>
        public long CategoryId { get; set; }
        /// <summary>
        /// The category to which the task belongs
        /// </summary>
        [JsonIgnore]
        public Category Category { get; set; }
        /// <summary>
        /// The identifier of the person who owns the work item
        /// </summary>
        /// <example>42</example>
        public long PersonId { get; set; }
        /// <summary>
        /// The person who owns the work item
        /// </summary>
        [JsonIgnore]
        public Person Person { get; set; }
        /// <summary>
        /// A  list of tags associated to the work item
        /// </summary>
        /// <example>[ "LoadTesting", "Functional Testing", "Unit Testing"]</example>
        public ICollection<string> Tags { get; set; } = new List<string>();

        internal class WorkitemListExample : IExamplesProvider<IEnumerable<WorkItem>>
        {
            public IEnumerable<WorkItem> GetExamples()
            {
                return new List<WorkItem>
                {
                    new WorkItem {
                        Id = 33,
                        Name = "Develop the web service",
                        CategoryId = 3,
                        PersonId = 1,
                        Tags = new List<string>() { "Development", "BackOffice" }
                    },
                    new WorkItem {
                        Id = 34,
                        Name = "Unit test the web service",
                        CategoryId = 3,
                        PersonId = 2,
                        Tags = new List<string>() { "UnitTest", "BackOffice" }
                    }
                };
            }
        }
    }
}
