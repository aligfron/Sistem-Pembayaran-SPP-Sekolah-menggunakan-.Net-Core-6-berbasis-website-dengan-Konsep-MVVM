using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VMTbMUser
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

        public string? Fullname { get; set; }
        public string? MobilePhone { get; set; }
        public string? Images { get; set; }
        public string? RoleName { get; set; }
        public string? Nis { get; set; }
        public string? Nisn { get; set; }
        public int? KelasId { get; set; }
        public int? JurusanId { get; set; }
        public int? SiswaId { get; set; }
    }
}
