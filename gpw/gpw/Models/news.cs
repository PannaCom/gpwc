//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace gpw.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class news
    {
        public long id { get; set; }
        public string new_title { get; set; }
        public string new_content { get; set; }
        public string new_flug { get; set; }
        public string new_img { get; set; }
        public Nullable<int> cat_id { get; set; }
        public string user_id { get; set; }
        public Nullable<int> quyen_hang { get; set; }
        public Nullable<int> trang_thai { get; set; }
        public Nullable<byte> isHot { get; set; }
        public Nullable<System.DateTime> ngay_tao { get; set; }
        public string new_des { get; set; }
    }
}
