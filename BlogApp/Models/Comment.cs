using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The author is required")]
        [MaxLength(100, ErrorMessage = "The author name cannot exceed 100 characters.")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "The content is required.")]
        [MaxLength(2000, ErrorMessage = "The content cannot exceed 2000 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        // Foreign Key
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

    }
}
