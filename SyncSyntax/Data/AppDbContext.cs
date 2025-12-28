using Microsoft.EntityFrameworkCore;
using SyncSyntax.Models;

namespace SyncSyntax.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology", Description = "Posts related to technology." },
                new Category { Id = 2, Name = "Health", Description = "Posts related to health and wellness." },
                new Category { Id = 3, Name = "Travel", Description = "Posts about travel experiences and tips." }
            );

            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Title = "The Rise of AI", Content = "Content about AI...", Author = "John Doe", PublishedDate = new DateTime(2019, 5, 23), CategoryId = 1, FeatureImagePath = "rise_of_ai_image.jpg" },
                new Post { Id = 2, Title = "Healthy Living Tips", Content = "Content about health...", Author = "Jane Smith", PublishedDate = new DateTime(2020, 1, 3), CategoryId = 2, FeatureImagePath = "Healthy_image_jpg" },
                new Post { Id = 3, Title = "Top 10 Travel Destinations", Content = "Content about travel...", Author = "Emily Johnson", PublishedDate = new DateTime(2025, 9, 16), CategoryId = 3, FeatureImagePath = "travel_Destinations_image.jpg" }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Author = "Alice Brown",
                    Content = "Very interesting article about AI. Looking forward to more!",
                    PostedDate = new DateTime(2019, 5, 24),
                    PostId = 1
                },
                new Comment
                {
                    Id = 2,
                    Author = "Michael Green",
                    Content = "AI is definitely shaping our future. Great insights.",
                    PostedDate = new DateTime(2019, 5, 25),
                    PostId = 1
                },
                new Comment
                {
                    Id = 3,
                    Author = "Laura White",
                    Content = "These healthy living tips are really useful. Thanks!",
                    PostedDate = new DateTime(2020, 1, 5),
                    PostId = 2
                },
                new Comment
                {
                    Id = 4,
                    Author = "Chris Black",
                    Content = "I have already tried some of these tips and they work!",
                    PostedDate = new DateTime(2020, 1, 6),
                    PostId = 2
                },
                new Comment
                {
                    Id = 5,
                    Author = "Sophia Martinez",
                    Content = "Amazing travel destinations. Adding these to my bucket list!",
                    PostedDate = new DateTime(2025, 9, 18),
                    PostId = 3
                }
            );

        }

    }
}

