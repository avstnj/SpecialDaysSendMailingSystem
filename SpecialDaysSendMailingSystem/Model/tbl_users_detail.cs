namespace SpecialDaysSendMailingSystem.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_users_detail
    {
        public int Id { get; set; }

        public DateTime? startDateofWork { get; set; }

        public DateTime? dateofBirth { get; set; }

        public DateTime? workTerminatedDate { get; set; }

        public int? userID { get; set; }

        [StringLength(150)]
        public string userEmail { get; set; }

        public bool isActive { get; set; }
    }
}
