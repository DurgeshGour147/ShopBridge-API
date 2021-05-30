using System;
using System.Collections.Generic;
using System.Text;

namespace Shop_Bridge.Common.DBConnection
{
    public class ShopBridgeConnection : LinqToDB.Data.DataConnection
    {
        public ShopBridgeConnection() : base("ShopBridge") { }
    }
}
