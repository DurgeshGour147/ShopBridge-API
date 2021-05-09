using System;
using System.Collections.Generic;
using System.Text;

namespace Jewelry_Store.Common
{
    public class ResolutionElement
    {
        public ResolutionElement(string _contract, string _implementation)
        {
            Contract = _contract;
            Implementation = _implementation;
        }
        public string Contract { get; set; }
        public string Implementation { get; set; }
    }
}
