using Service.Models;

namespace PolicyService.Controllers
{
    public class PolicyInfo
    {
        public List<Policy> Policies { get; set; }

        public string Terms { get; set; }
    }
}