using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SyncSyntax.Data;
using SyncSyntax.Models;
using SyncSyntax.Models.ViewModels;

namespace SyncSyntax.Controllers;

public class PostController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png", ".gif" };

    public PostController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Index(int? categoryId)
    {
        // Retrieve posts with their associated categories
        var postsQuery = _context.Posts.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
        {
            postsQuery = postsQuery.Where(p => p.CategoryId == categoryId.Value);
        }
        var postsList = postsQuery.ToList();

        ViewBag.Categories = _context.Categories.ToList();
        return View(postsList);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest();
        }

        var post = await _context.Posts.Include(p => p.Category).Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var postViewModel = new PostViewModel();
        postViewModel.Categories = _context.Categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return View(postViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == null || id <= 0)
        {
            return NotFound();
        }
        var postFromdb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (postFromdb == null)
        {
            return NotFound();
        }

        EditPostViewModel editPostViewModel = new EditPostViewModel
        {
            Post = postFromdb,
            Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return View(editPostViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostViewModel postViewModel)
    {
        if (ModelState.IsValid)
        {
            var inputFileExtension = Path.GetExtension(postViewModel.FeatureImage.FileName).ToLower();
            bool isAllowed = _allowedExtension.Contains(inputFileExtension);

            if (!isAllowed)
            {
                ModelState.AddModelError("FeatureImage", "Invalid image format. Only .jpg, .jpeg, .png, .gif are allowed.");
                return View(postViewModel);
            }

            postViewModel.Post.FeatureImagePath = await UploadFileToFolder(postViewModel.FeatureImage);
            await _context.Posts.AddAsync(postViewModel.Post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        //To repopulate categories in case 
        postViewModel.Categories = _context.Categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return View(postViewModel);
    }

    [HttpPost]
    public JsonResult AddComment([FromBody] Comment comment)
    {
        comment.PostedDate = DateTime.Now;
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return Json(new
        {
            author = comment.Author,
            content = comment.Content,
            postedDate = comment.PostedDate.ToString("dd MMM yyyy HH:mm")
        });
    }

    public async Task<string> UploadFileToFolder(IFormFile file)
    {
        var inputFileExtension = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString() + inputFileExtension;
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var imagesFolderPath = Path.Combine(wwwRootPath, "images");

        if (!Directory.Exists(imagesFolderPath))
        {
            Directory.CreateDirectory(imagesFolderPath);
        }

        var filePath = Path.Combine(imagesFolderPath, fileName);
        try
        {
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error uploading file: " + ex.Message);
        }

        return "/images/" + fileName;
    }
}

