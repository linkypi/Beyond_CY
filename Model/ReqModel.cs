using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class ReqModel
    {
        public string seller { get; set; }
        public string service { set; get; }
        public int? timestamp { get; set; }
        public string user { set; get; }
        public string pass { get; set; }
        public string sign { set; get; }
        public string callback { set; get; }
        public int?    filter_type{ set; get; }
        public string keyword{get;set;}
        public int? page_no{ set; get; }
        public int? page_size{ set; get; }
        public int? type_id{ set; get; }
        public int? sort_type{ set; get; }
        public bool? in_stock { set; get; }
        public int? goods_id { set; get; }
        public List<int?> goods_ids { set; get; }
        public int? img_type { set; get; }
        public int? member_id { set; get; }
        public DateTime? stime { set; get; }
        public DateTime? etime { set; get; }
        public string order_note { get; set; }
        public string goods_data { set; get; }
        public int? order_id { set; get; }
    }
}