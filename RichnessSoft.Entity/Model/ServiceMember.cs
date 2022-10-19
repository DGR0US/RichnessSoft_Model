﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Entity.Model
{
    public class ServiceMember : BaseModel
    {
        public int companyid { get; set; } = default;
        public int? customerid { get; set; } = default;
        public int? serviceid { get; set; } = default;
        public int? umid { get; set; } = default;
        public int? serviceumid { get; set; } = default;

        public decimal stdprice { get; set; }
        public decimal memberprice1 { get; set; }
        public decimal memberprice2 { get; set; }
        public decimal memberprice3 { get; set; }
        public decimal memberprice4 { get; set; }
        public decimal memberprice5 { get; set; }
        public decimal memberprice6 { get; set; }
        public decimal memberprice7 { get; set; }
        public decimal memberprice8 { get; set; }
        public string seq { get; set; }
        public string discount { get; set; }

        public Company Company { get; set; }
        public Customer Customer { get; set; }
        public Service Service { get; set; }


        //#region Expression Field
        //[NotMapped]
        //public string flag { get; set; }

        //[NotMapped]
        //public decimal ExpQty { get; set; } = 0M;

        //[NotMapped]
        //public string ExpStdUm { get; set; } = string.Empty;

        //[NotMapped]
        //public decimal ExpStdPrice { get; set; } = 0M;
        //#endregion


    }
}
