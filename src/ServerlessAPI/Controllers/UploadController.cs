using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace ServerlessAPI.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class UploadController(ILogger<UploadController> logger, IConfiguration configuration, IAmazonS3 s3Client) : ControllerBase
{
    private readonly ILogger<UploadController> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IAmazonS3 _s3Client = s3Client;

    [HttpPut]
    public ActionResult<string> GenerateUploadUrl([FromBody] UploadData uploadData)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(uploadData.FileName))
                return BadRequest();
            string bucketName = _configuration["AWS:BucketName"] ?? "";
            return GeneratePreSignedURL(bucketName, uploadData.FileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating presigned url");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private string GeneratePreSignedURL(string bucketName, string? objectKey, double duration = 1)
    {
        GetPreSignedUrlRequest r = new()
        {
            BucketName = bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.AddHours(duration),
            Verb = HttpVerb.PUT
        };
        return _s3Client.GetPreSignedURL(r);
    }

    public class UploadData
    {
        public string? FileName { get; set; }
    }

}
