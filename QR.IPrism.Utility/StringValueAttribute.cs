using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QR.IPrism.Utility
{
    public class StringValueAttribute : Attribute
    {
        private string _strvalue;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringValueAttribute(string value)
        {
            _strvalue = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value></value>
        public string Value
        {
            get { return _strvalue; }
        }
    }
}
