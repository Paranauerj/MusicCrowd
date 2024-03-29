﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetoCrowdsourcing
{
    public class BaseContext : DbContext
    { 
        public DbSet<Models.HIT> HITs { get; set; }
        public DbSet<Models.Assignment> Assignments { get; set; }
        public DbSet<Models.ValidationHIT> ValidationHITs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=MusicCrowd;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
    }
}
