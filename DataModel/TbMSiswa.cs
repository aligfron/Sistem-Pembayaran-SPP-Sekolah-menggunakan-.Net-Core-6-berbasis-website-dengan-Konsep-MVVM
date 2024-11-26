using System;
using System.Collections.Generic;

namespace DataModel
{
    public partial class TbMSiswa
    {
        public int Id { get; set; }
        public int? BiodataId { get; set; }
        public string? Nisn { get; set; }
        public string? Nis { get; set; }
        public int? KelasId { get; set; }
        public int? JurusanId { get; set; }
        public string? TahunMasuk { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
