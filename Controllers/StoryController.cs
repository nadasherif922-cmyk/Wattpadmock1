using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wattpadmock1.Models;

namespace Wattpadmock1.Controllers
{
    public class StoryController : Controller
    {
        private readonly AppDbContext _context;

        public StoryController(AppDbContext context)
        {
            _context = context;
        }

        // Show all stories
        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories
                .Include(s => s.Author)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(stories);
        }

        // Show create story page
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        // Handle create story form
        [HttpPost]
        public async Task<IActionResult> Create(string title, string description, string content, string genre)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var story = new Story
            {
                Title = title,
                Description = description,
                Content = content,
                Genre = genre,
                UserId = HttpContext.Session.GetInt32("UserId").Value
            };

            _context.Stories.Add(story);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Read a story
        public async Task<IActionResult> Read(int id)
        {
            var story = await _context.Stories
                .Include(s => s.Author)
                .Include(s => s.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (story == null)
                return NotFound();

            return View(story);
        }

        // Add a comment
        [HttpPost]
        public async Task<IActionResult> AddComment(int storyId, string content)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var comment = new Comment
            {
                Content = content,
                StoryId = storyId,
                UserId = HttpContext.Session.GetInt32("UserId").Value
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Read", new { id = storyId });
        }
    }
}