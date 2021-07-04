namespace SpecialDaysSendMailingSystem.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GeneralContext : DbContext
    {
        public GeneralContext()
            : base("name=GeneralContext")
        {
        }

        public virtual DbSet<tbl_users> tbl_users { get; set; }
        public virtual DbSet<tbl_users_detail> tbl_users_detail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
