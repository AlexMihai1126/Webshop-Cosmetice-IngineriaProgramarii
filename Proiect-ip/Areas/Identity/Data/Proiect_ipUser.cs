using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Proiect_ip.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Proiect_ipUser class
public class Proiect_ipUser : IdentityUser
{
    public string Nume { get; set; }
    public string Prenume { get; set; }
    public string? Adresa { get; set; }
    public int Puncte { get; set; }
    public int NrComenzi { get; set; }
}

