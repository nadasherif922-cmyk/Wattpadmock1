using System.ComponentModel.DataAnnotations;

namespace Wattpadmock1.Models
{
    public class Story
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? Genre { get; set; }

        public string? CoverImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? Author { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
