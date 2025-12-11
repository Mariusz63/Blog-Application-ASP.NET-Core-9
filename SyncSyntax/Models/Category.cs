using System.ComponentModel.DataAnnotations;

namespace SyncSyntax.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The category name is required")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The description is required")]
        [MaxLength(200, ErrorMessage = "The description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        // Foreign Key Relationship
        public ICollection<Post>? Posts { get; set; }
    }
}
