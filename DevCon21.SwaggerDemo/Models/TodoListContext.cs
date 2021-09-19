using Microsoft.EntityFrameworkCore;

namespace DevCon21.SwaggerDemo.Models
{
    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(entity =>
            {
                entity.Property(e => e.Tags)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries));

                entity
                .HasOne(e => e.Category)
                .WithMany(e => e.WorkItems)
                .IsRequired();

                entity
                .HasOne(e => e.Person)
                .WithMany(e => e.WorkItems);
            });

            base.OnModelCreating(modelBuilder);

            EnsureData(modelBuilder);
        }

        private void EnsureData(ModelBuilder modelBuilder)
        {
            var categories = new[]
            {
                new Category { Id = 1L, Name = "Root" },
                new Category { Id = 2L, Name = "Development", ParentId = 1 },
                new Category { Id = 3L, Name = "Testing", ParentId = 1 },
                new Category { Id = 4L, Name = "Deployment", ParentId = 1 },
                new Category { Id = 5L, Name = "Sql", ParentId = 2 },
                new Category { Id = 6L, Name = "BizTalk", ParentId = 2 },
                new Category { Id = 7L, Name = "SharePoint", ParentId = 2 },
                new Category { Id = 8L, Name = "Services", ParentId = 2 },
                new Category { Id = 9L, Name = "UI", ParentId = 2 },
                new Category { Id = 10L, Name = "Eventing", ParentId = 8 },
                new Category { Id = 11L, Name = "StaticSources", ParentId = 8 },
                new Category { Id = 12L, Name = "DynamicSources", ParentId = 8 },
                new Category { Id = 13L, Name = "Synchronization", ParentId = 8 },
                new Category { Id = 14L, Name = "ClientApp", ParentId = 9 }
            };
            modelBuilder.Entity<Category>()
                .HasData(categories);

            var people = new[]
            {
                new Person { Id = 1, FullName = "Olivier" },
                new Person { Id = 2, FullName = "David" },
                new Person { Id = 3, FullName = "Guillaume" },
                new Person { Id = 4, FullName = "Mélanie" },
                new Person { Id = 5, FullName = "Thomas" },
                new Person { Id = 6, FullName = "François" }
            };
            modelBuilder.Entity<Person>()
                .HasData(people);

            var workItems = new[]
            {
                new WorkItem { Id = 1L, IsComplete = false, Name = "Create deployment pipeline",  CategoryId = 4L, PersonId = 2L },
                new WorkItem { Id = 2L, IsComplete = false, Name = "Create Alerting web service", CategoryId = 8L, PersonId = 5L },
                new WorkItem { Id = 3L, IsComplete = false, Name = "Create Alerting Angular component", CategoryId = 14L, PersonId = 4L },
            };
            modelBuilder.Entity<WorkItem>()
                .HasData(workItems);

        }
    }
}
