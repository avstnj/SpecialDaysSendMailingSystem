namespace SpecialDaysSendMailingSystem.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_users
    {
        [Key]
        public int user_UID { get; set; }

        [StringLength(11)]
        public string user_TCNO { get; set; }

        [StringLength(50)]
        public string user_code { get; set; }

        [StringLength(255)]
        public string user_FName { get; set; }

        [StringLength(255)]
        public string user_SName { get; set; }

        [StringLength(255)]
        public string user_LName { get; set; }

        [StringLength(10)]
        public string user_status_code { get; set; }

        [StringLength(128)]
        public string user_passwd { get; set; }

        [Column(TypeName = "date")]
        public DateTime? passwd_expire_date { get; set; }

        public int? change_on_initial_login { get; set; }

        public int? wrong_attempt_count { get; set; }

        [StringLength(255)]
        public string user_email { get; set; }

        [StringLength(20)]
        public string user_mobile { get; set; }

        [StringLength(50)]
        public string user_alias { get; set; }

        [MaxLength(8000)]
        public byte[] user_photo { get; set; }

        public DateTime? create_date { get; set; }

        public int? create_user_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? update_date { get; set; }

        public int? update_user_ID { get; set; }

        [StringLength(14)]
        public string mifareID { get; set; }

        public bool? password_changeable { get; set; }

        public DateTime? last_login { get; set; }

        public long? login_count { get; set; }

        public bool? IsActive { get; set; }

        public bool isCitizen { get; set; }

        public bool? IsDelete { get; set; }

        public DateTime? EmailApprovedDate { get; set; }

        public Guid? RowGuid { get; set; }
    }
}
