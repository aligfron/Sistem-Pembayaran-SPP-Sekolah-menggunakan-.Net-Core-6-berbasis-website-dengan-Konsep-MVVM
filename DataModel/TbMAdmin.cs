using System;
using System.Collections.Generic;

namespace DataModel
{
    public partial class TbMAdmin
    {
        public int Id { get; set; }
        public int? BiodataId { get; set; }
        public string? Code { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
