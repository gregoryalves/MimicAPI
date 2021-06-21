using Microsoft.EntityFrameworkCore;
using MimicAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.DataBase
{
    public class MimicContext : DbContext
    {
        public MimicContext(DbContextOptions<MimicContext> options) : base(options)
        {
            
        }

        public DbSet<Palavra> Palavras { get; set; }
    }
}
