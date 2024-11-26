using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VMTbTPembayaran
    {
        public int Id { get; set; }
        public int? SiswaId { get; set; }
        public string? Bulan { get; set; }
        public string? Tahun { get; set; }
        public int? Jumlah { get; set; }
        public int? KelasId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }



        public string? Email { get; set; }
        public string? MobilePhone { get; set; }
        public string? Images { get; set; }
        public int? BiodataId { get; set; }
        public string? Nisn { get; set; }
        public string? Nis { get; set; }
        public int? JurusanId { get; set; }
        public string? Fullname { get; set; }
        public string? NamaKelas { get; set; }
        public string? NamaJurusan { get; set; }
        public string? TahunMasuk { get; set; }
    }
}
