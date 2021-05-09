using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Jewelry_Store.Common.DBConnection
{
    public class DBSettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";
        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "JewelryStore",
                        ProviderName = "SqlServer",
                        ConnectionString = ConfigManager.Instance.JewelryStoreConnectionString
                    };
            }
        }
    }
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
