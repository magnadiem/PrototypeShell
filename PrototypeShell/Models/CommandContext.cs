using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrototypeShell.Models
{
    public class CommandContext : DbContext
    {
        public DbSet<Command> Commands { get; set; }
        public CommandContext(DbContextOptions<CommandContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
