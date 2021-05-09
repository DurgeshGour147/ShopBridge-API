using System;
using System.Collections.Generic;
using System.Text;

namespace Jewelry_Store.Common.DBConnection
{
    public class JewelryStoreConnection : LinqToDB.Data.DataConnection
    {
        public JewelryStoreConnection() : base("JewelryStore") { }
    }
}
