using DropZone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace DropZone.Controllers
{
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            var viewModel = new ImageUploadViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Upload(ImageUploadViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Image validation logic
                //if (!IsJpeg(viewModel.ImageFile) ||
                //    viewModel.ImageFile.Length > 5 * 1024 * 1024 ||
                //    !IsAspectRatioOneToOne(viewModel.ImageFile) ||
                //    !IsMinDpi(viewModel.ImageFile, 300) ||
                //    !IsMinResolution(viewModel.ImageFile, 1920, 1080))
                //{
                //    ModelState.AddModelError("ImageFile", "Invalid image properties.");
                //    return BadRequest(new { success = false, message = "Invalid image properties." });
                //}

                // Process and save the image (replace this with your logic)
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var imagePath = Path.Combine(path, viewModel.ImageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    viewModel.ImageFile.CopyTo(stream);
                }

                // Redirect or return a success message
                return Ok(new { success = true, message = "Image uploaded successfully." });
            }

            // If validation fails, return to the upload view with the model
            return BadRequest(new { success = false, message = "Invalid model state." });
        }

        public IActionResult UploadSuccess()
        {
            // Display a success message or redirect to another action
            return View();
        }

        // Image validation methods
        private bool IsJpeg(IFormFile file)
        {
            return file.ContentType == "image/jpeg" || Path.GetExtension(file.FileName).ToLower() == ".jpg";
        }

        private bool IsAspectRatioOneToOne(IFormFile file)
        {
            // Implement aspect ratio validation logic here
            // For simplicity, assuming all images are valid in this example
            //using (var image = Image.FromStream(file.OpenReadStream()))
            //{
            //    // Calculate aspect ratio (width / height)
            //    var aspectRatio = (double)image.Width / image.Height;
            //    return Math.Abs(aspectRatio - 1) < 0.001; // Allowing a small epsilon for precision issues
            //}
            return true;
        }

        private bool IsMinDpi(IFormFile file, int minDpi)
        {
            // Implement DPI validation logic here
            // For simplicity, assuming all images are valid in this example
            // Basic DPI validation (Note: DPI in browsers can be unreliable)
            //var dpi = 300; // Default DPI value in most browsers
            //using (var image = Image.FromStream(file.OpenReadStream()))
            //{
            //    dpi = (int)image.HorizontalResolution;
            //}

            //return dpi >= minDpi;
            return true;
        }

        private bool IsMinResolution(IFormFile file, int minWidth, int minHeight)
        {
            // Implement resolution validation logic here
            // For simplicity, assuming all images are valid in this example
            //using (var image = Image.FromStream(file.OpenReadStream()))
            //{
            //    return image.Width >= minWidth && image.Height >= minHeight;
            //}

            return true;
        }

        [HttpPost]
        public IActionResult RemoveFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine("wwwroot", "uploads", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(new { success = true, message = "File removed successfully." });
                }

                return NotFound(new { success = false, message = "File not found." });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { success = false, message = "Error removing the file." });
            }
        }

    }
}
