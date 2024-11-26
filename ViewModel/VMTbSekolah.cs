using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VMTbSekolah
    {
        public int Id { get; set; }
        public string? NamaSekolah { get; set; }
        public string? Alamat { get; set; }
        public string? NoTlp { get; set; }
        public string? BiayaSpp { get; set; }
        public string? Email { get; set; }
        public string? Npsn { get; set; }
        public string? StatusSekolah { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDelete { get; set; }
    }
}
