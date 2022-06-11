using BlyckBox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BlyckBox.Controllers
{
    [Route("blogadmin")]
    public class BlogAdminController : Controller
    {
        private readonly IConfiguration _config;
        private readonly DataContext context;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        public BlogAdminController(IConfiguration config, DataContext _context, IWebHostEnvironment _hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _config = config;
            context = _context;
            hostingEnvironment = _hostingEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("PostBlog")]
        [HttpPost]
        public async Task<IActionResult> PostBlog(BlogVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await _userManager.FindByEmailAsync(User.Identity!.Name);
                    var blogDS = new Blog
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Tags = model.Tags,
                        UserId = user.Id,
                        Description = model.Description
                    };
                    if (model.Image != null)
                    {
                        string uploadfolder = Path.Combine(hostingEnvironment.WebRootPath, "blogimages");
                        string[] files = Directory.GetFiles(uploadfolder);
                        string UniqueFileNameimage = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filepath = Path.Combine(uploadfolder, UniqueFileNameimage);
                        foreach (var file in files)
                        {
                            if (blogDS.Image != null)
                                if (file.Contains(blogDS.Image))
                                    System.IO.File.Delete(file);
                        }
                        using (var stream = System.IO.File.Create(filepath))
                        {
                            await model.Image.CopyToAsync(stream);
                        }
                        blogDS.Image = "blogimages/" + UniqueFileNameimage;
                    }

                    context.Blogs?.AddAsync(blogDS);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("upload_ckeditor")]
        public async Task<IActionResult> UploadCKEditorImage(IFormFile upload)
        {
            var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + upload.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), hostingEnvironment.WebRootPath, "uploads", filename);
            var stream = new FileStream(path, FileMode.Create);
            await upload.CopyToAsync(stream);
            return new JsonResult(new { path = "/uploads/" + filename });
        }
    }
}