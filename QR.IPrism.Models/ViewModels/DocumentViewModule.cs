﻿using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
    public class DocumentViewModel
    {
        public List<DocumentModel> Documents { get; set; }

        public List<DocumentModel> result { get; set; }
    }
}


