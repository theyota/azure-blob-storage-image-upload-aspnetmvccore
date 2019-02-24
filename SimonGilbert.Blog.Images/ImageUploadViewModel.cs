using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SimonGilbert.Blog.Images
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Please select images")]
        public IFormFileCollection[] Images { get; set; }
    }
}
