﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Shared
{
    public class MenuModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public int Rank { get; set; }
        public string Sref { get; set; }
        public string Controller { get; set; }
        public string Template { get; set; }
        public List<MenuModel> SubMenus { get; set; }
        public string IconClass { get; set; }
        public bool IsMobileLink { get; set; }
    }
}