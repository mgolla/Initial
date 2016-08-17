﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class EVRRequestFilterModel
    {
        public string StaffId { get; set; }
        public string evrId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime EvrSearchFromDate { get; set; }
        public DateTime EvrSearchToDate { get; set; }
        public string SectorFrom { get; set; }
        public string SectorTo { get; set; }
        public string Status { get; set; }
    }
}