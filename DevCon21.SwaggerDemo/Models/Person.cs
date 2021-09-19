using System.Collections.Generic;

namespace DevCon21.SwaggerDemo.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public ICollection<WorkItem> WorkItems { get; set; }
        //public ICollection<WorkItem> ParticipatingWorkItems { get; set; }
    }
}
