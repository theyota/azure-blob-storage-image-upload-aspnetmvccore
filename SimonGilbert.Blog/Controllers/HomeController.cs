using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimonGilbert.Blog.Images;

namespace SimonGilbert.Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageUploadService _imageUploadService;
        private const string DummyAccountId = "Simon.Gilbert";

        public HomeController(IImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var images = await _imageUploadService.GetAll(DummyAccountId);

            return View(images);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFileCollection images)
        {
            if (images != null && images.Count > 0)
            {
                await _imageUploadService.Upload(images, DummyAccountId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }


    }
}
