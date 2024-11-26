using System;
using System.Collections.Generic;

namespace DataModel
{
    public partial class TbMUser
    {
        public int Id { get; set; }
        public int? BiodataId { get; set; }
        public int? RoleId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TahunMasuk { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int? LoginAttempt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
