﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace asp_mvc_2.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DEMODB1Entities : DbContext
    {
        public DEMODB1Entities()
            : base("name=DEMODB1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<LOOKUPRole> LOOKUPRole { get; set; }
        public virtual DbSet<SYSUser> SYSUser { get; set; }
        public virtual DbSet<SYSUserProfile> SYSUserProfile { get; set; }
        public virtual DbSet<SYSUserRole> SYSUserRole { get; set; }

        public System.Data.Entity.DbSet<Asp_mvc_2.Models.ViewModel.UserSignUpView> UserSignUpViews { get; set; }
    }
}
