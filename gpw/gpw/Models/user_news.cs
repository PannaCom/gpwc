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
    
    public partial class user_news
    {
        public long id { get; set; }
        public string title { get; set; }
        public string des { get; set; }
        public string full_content { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public Nullable<long> user_id { get; set; }
        public string user_name { get; set; }
        public string tags { get; set; }
    }
}
