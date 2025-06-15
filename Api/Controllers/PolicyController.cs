using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.PolicyDbContext;

namespace PolicyService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PolicyController : ControllerBase
{
    private readonly PolicyDbContext _context;

    private readonly IConfiguration _configuration;

    public PolicyController(PolicyDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            service = "PolicyService",
            region = _configuration["REGION"] ?? "Unknown",
            status = "Healthy"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var blobFileName = "PolicyTermsAndConditions.txt";
        var blobUrl = $"https://{_configuration["Blob:AccountName"]}.blob.core.windows.net/{_configuration["Blob:Container"]}/{blobFileName}";
        var blobClient = new BlobClient(new Uri(blobUrl), new DefaultAzureCredential());
        var textFromBlob = (await blobClient.DownloadContentAsync()).Value.Content.ToString();

        // Fetch location from ARM API
        var region = await GetStorageAccountRegionAsync();

        return Ok(new PolicyInfo
        {
            Policies = _context.Policies.ToList(),
            Terms = textFromBlob
        });
    }

    private async Task<string> GetStorageAccountRegionAsync()
    {
        string subscriptionId = _configuration["Azure:SubscriptionId"];
        string resourceGroupName = _configuration["Azure:ResourceGroup"];
        string storageAccountName = _configuration["Blob:AccountName"];

        var credential = new DefaultAzureCredential();
        var armClient = new ArmClient(credential);

        var storageAccountResourceId = StorageAccountResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, storageAccountName);
        var storageAccount = await armClient.GetStorageAccountResource(storageAccountResourceId).GetAsync();

        return storageAccount.Value.Data.Location;
    }

    [HttpPost]
    public IActionResult Create(Policy policy)
    {
        _context.Policies.Add(policy);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAllAsync), new { id = policy.Id }, policy);
    }
}
