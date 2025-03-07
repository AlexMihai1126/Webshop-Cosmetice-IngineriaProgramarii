﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Proiect_ip.Models;

namespace Proiect_ip.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Proiect_ipUser class
public class Proiect_ipUser : IdentityUser
{
    public ICollection<Comanda> Comenzi { get; set; }
    public ICollection<IstoricPuncte> IstoricPuncte { get;set; }

}

