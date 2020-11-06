using EntityFramwork.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramwork.Data
{
    public class UserData : DbContext
    {
        public UserData(DbContextOptions options): base(options) { }
        public DbSet<Termin> Termine { get; set; }
        public DbSet<Teilnehmer> Teilnahmen { get; set; }
    }
}
