using System.ComponentModel.DataAnnotations;

namespace Wattpadmock1.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? Author { get; set; }

        public int StoryId { get; set; }
        public Story? Story { get; set; }
    }
}