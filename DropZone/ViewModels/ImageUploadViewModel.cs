using System.ComponentModel.DataAnnotations;

namespace DropZone.ViewModels
{
    public class ImageUploadViewModel
    {
        [Required(ErrorMessage = "Please select an image.")]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
