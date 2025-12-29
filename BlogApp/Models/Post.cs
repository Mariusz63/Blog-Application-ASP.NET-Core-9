using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is require")]
        [MaxLength(200, ErrorMessage = "The title cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [Required]
        [MaxLength(10000, ErrorMessage = "The content cannot exceed 10000 characters.")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "The Author is required")]
        [MaxLength(100, ErrorMessage = "The author name cannot exceed 100 characters.")]
        public string? Author { get; set; }

        [ValidateNever]
        public string? FeatureImagePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        // Foreign Key
        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
