using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proiect_ip.Pages
{
    public class ConfirmPlataModel : PageModel

    {
        public string Status { get; set; } = "Fail"; // Default status

        public void OnGet(string status)
        {
            Status = status; 
        }
    }
}
