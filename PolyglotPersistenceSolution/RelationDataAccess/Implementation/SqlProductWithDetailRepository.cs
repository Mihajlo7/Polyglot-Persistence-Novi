using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.Implementation
{
    public class SqlProductWithDetailRepository 
    {
        private readonly string _connectionString;
        private readonly string _database;

        public SqlProductWithDetailRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={database};Integrated Security=True;TrustServerCertificate=True;";
        }
    }
}
