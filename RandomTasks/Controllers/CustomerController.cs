using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomTasks.Controllers
{
    [Authorize(Roles = "2")]
    public class CustomerController : Controller
    {
        private readonly IAmazonS3 _s3Client;

        // 1. Inject the S3 Client
        public CustomerController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handle the upload
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file, string bucketName)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file first!";
                return View();
            }

            // 2. Upload the file to S3
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(putRequest);

            // 3. Generate a temporary "VIP Pass" (Pre-signed URL) to view the file
            // This is necessary because your bucket is private!
            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = file.FileName,
                Expires = DateTime.UtcNow.AddHours(1) // Link valid for 1 hour
            };

            string imageUrl = _s3Client.GetPreSignedURL(urlRequest);

            // 4. Pass the URL back to the view to display it
            ViewBag.SuccessMessage = $"File '{file.FileName}' uploaded successfully!";
            ViewBag.ImageUrl = imageUrl;
            ViewBag.BucketName = bucketName; // Keep the bucket name for the next upload

            return View();
        }
    }
}
