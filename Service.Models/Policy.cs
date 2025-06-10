namespace Service.Models;
public class Policy
{
    public int Id { get; set; }
    public required string PolicyNumber { get; set; }
    public required string PolicyHolderName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PremiumAmount { get; set; }
    public required string CoverageDetails { get; set; }
    public required string Status { get; set; } // e.g., Active, Inactive, Cancelled
}
