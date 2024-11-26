using System;
using System.Collections.Generic;

namespace DataModel
{
    public partial class TbTPembayaran
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
    }
}
