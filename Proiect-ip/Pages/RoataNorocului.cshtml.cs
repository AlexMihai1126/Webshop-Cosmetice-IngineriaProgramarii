using Microsoft.AspNetCore.Mvc.RazorPages;

public class RoataNoroculuiModel : PageModel
{
    public List<RoataItem> Items { get; set; }

    public void OnGet()
    {
        // Creezi lista de items pentru roată
        Items = new List<RoataItem>
        {
            new RoataItem { Label = "Dell LAPTOP", Question = "What CSS property is used for specifying the area between the content and its border?" },
            new RoataItem { Label = "IMAC PRO", Question = "What CSS property is used for changing the font?" },
            new RoataItem { Label = "SUZUKI", Question = "What CSS property is used for changing the color of text?" },
            new RoataItem { Label = "SUZUKI", Question = "What CSS property is used for changing the color of text?" },
            // Adaugă toate elementele pentru roată
        };
    }
}

public class RoataItem
{
    public string Label { get; set; }
    public string Question { get; set; }
}
