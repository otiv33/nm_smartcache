using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using traditional.API.Models;


namespace traditional.API
{
    public partial class EmailStoreDbContext : DbContext
    {
        public virtual DbSet<Email> Emails { get; set; }

        public EmailStoreDbContext()
        {
        }

        public EmailStoreDbContext(DbContextOptions<EmailStoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}

