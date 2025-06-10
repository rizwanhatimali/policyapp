using Microsoft.EntityFrameworkCore;
using Service.Models;

namespace Service.PolicyDbContext;

public class PolicyDbContext : DbContext
{
    public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options) { }
    public DbSet<Policy> Policies => Set<Policy>();
}
