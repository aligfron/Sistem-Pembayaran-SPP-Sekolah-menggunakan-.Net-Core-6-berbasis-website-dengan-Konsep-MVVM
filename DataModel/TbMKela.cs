using System;
using System.Collections.Generic;

namespace DataModel
{
    public partial class TbMKela
    {
        public int Id { get; set; }
        public string? NamaKelas { get; set; }
        public int? JurusanId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
