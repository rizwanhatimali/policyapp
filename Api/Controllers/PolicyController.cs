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

    public PolicyController(PolicyDbContext context)
    {
        _context = context;
    }

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            service = "PolicyService",
            region = Environment.GetEnvironmentVariable("REGION") ?? "Unknown",
            status = "Healthy"
        });
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Policies.ToList());

    [HttpPost]
    public IActionResult Create(Policy policy)
    {
        _context.Policies.Add(policy);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = policy.Id }, policy);
    }
}
