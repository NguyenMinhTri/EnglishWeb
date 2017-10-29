using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Framework.Model  
{
    public interface IAuditable
    {
        DateTime? CreatedDate { set; get; }
        string CreatedBy { set; get; }
        DateTime? UpdatedDate { set; get; }
        string UpdatedBy { set; get; }
        bool Status { set; get; }
        bool Protected { get; set; }

    }
    /// <summary>
    /// Lớp Auditable là lớp chứa các thuộc tính như ngày tạo người tạo record này có bị xóa hay không
    /// </summary>
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }


        [DefaultValue(true)]
        public bool Status{ set;  get; }
        public bool Protected { get; set; }
    }
}
