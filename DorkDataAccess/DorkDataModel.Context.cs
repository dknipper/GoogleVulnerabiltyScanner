﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DorkDataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DorkEntities : DbContext
    {
        public DorkEntities()
            : base("name=DorkEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<GoogleDork> GoogleDorks { get; set; }
        public DbSet<GoogleDorkParent> GoogleDorkParents { get; set; }
        public DbSet<VulnerableSite> VulnerableSites { get; set; }
        public DbSet<FullGoogleDork> FullGoogleDorks { get; set; }
    }
}
