using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Extensions;
using Dapper;

namespace CostEffectiveCode.Tests
{
    public class OptimizedQuery
        : IAsyncQuery<UberProductSpec, IPagedEnumerable<ProductDto>>
        , IDisposable
    {
        private readonly SqlConnection _sqlConnection;


        public int Price { get; set; } = 5;

        public OptimizedQuery()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            _sqlConnection.Open();
        }

        public SqlConnection OpenConnection(int i)
        {
            var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public async Task<IPagedEnumerable<ProductDto>> Ask(UberProductSpec spec)
        {
            var sqlConnection = _sqlConnection;
                //_dictionary.GetOrAdd(Thread.CurrentThread.ManagedThreadId, OpenConnection);
            var sw = new Stopwatch();
            sw.Start();
            var res1 = await sqlConnection
                .QueryAsync<ProductDto>("SELECT p.Id as Id, c.Name as CategoryName, p.Price as Price " +
                                        "FROM Products p INNER JOIN Categories c ON p.Category_Id = c.Id " +
                                        "WHERE Price > @Price " +
                                        "ORDER BY Id OFFSET 0 ROWS FETCH NEXT 30 ROWS ONLY", new {Price});

            var res2 = await sqlConnection.QueryAsync<int>("SELECT COUNT(*) FROM Products " +
                                                            "WHERE Price > @Price", new {Price});

            return Paged.From(res1, res2.Single());
        } 

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}
