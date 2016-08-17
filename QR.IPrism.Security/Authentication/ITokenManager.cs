﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Security.Authentication
{
    public interface ITokenManager
    {
        string GetAntiForgeryToken();
        bool ValidateAntiForgeryToken(string token);
    }
}