﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.EntityObjects.Module
{
    public class CareerPathEO
    {
        public String FromGrade { get; set; }
        public String ToGrade { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? DOJ { get; set; }
    }
}