using Microsoft.AspNetCore.Mvc;


namespace BlyckBox.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IConfiguration _config;
        private readonly DataContext context;
        private readonly IWebHostEnvironment hostingEnvironment;
        public BlogController(IConfiguration config, DataContext _context, IWebHostEnvironment _hostingEnvironment)
        {
            _config = config;
            context = _context;
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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