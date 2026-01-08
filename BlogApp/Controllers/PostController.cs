using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers;

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
    [Authorize(Roles = "Admin")]
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


    [HttpGet]
    [Authorize(Roles = "Admin")]
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
    public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
    {
        if (ModelState.IsValid)
        {
            return View(editPostViewModel);
        }

        // Fetch the existing post from the database, as no-tracking to avoid EF Core tracking issues
        var postFromDb = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editPostViewModel.Post.Id);

        if (postFromDb == null)
        {
            return NotFound();
        }

        if (editPostViewModel.FeatureImage != null)
        {
            var inputFileExtension = Path.GetExtension(editPostViewModel.FeatureImage.FileName).ToLower();
            bool isAllowed = _allowedExtension.Contains(inputFileExtension);

            if (!isAllowed)
            {
                ModelState.AddModelError("FeatureImage", "Invalid image format. Only .jpg, .jpeg, .png, .gif are allowed.");
                return View(editPostViewModel);
            }

            var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(postFromDb.FeatureImagePath));
            if (System.IO.File.Exists(existingImagePath))
            {
                System.IO.File.Delete(existingImagePath);
            }

            editPostViewModel.Post.FeatureImagePath = await UploadFileToFolder(editPostViewModel.FeatureImage);
        }
        else
        {
            editPostViewModel.Post.FeatureImagePath = postFromDb.FeatureImagePath;
        }

        _context.Posts.Update(editPostViewModel.Post);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var postFromDb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (postFromDb == null)
        {
            return NotFound();
        }
        return View(postFromDb);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var postFromDb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (postFromDb == null)
        {
            return NotFound();
        }
        if (string.IsNullOrEmpty(postFromDb.FeatureImagePath))
        {
            var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(postFromDb.FeatureImagePath));
            if (System.IO.File.Exists(existingImagePath))
            {
                System.IO.File.Delete(existingImagePath);
            }
        }
        _context.Posts.Remove(postFromDb);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Authorize]
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

