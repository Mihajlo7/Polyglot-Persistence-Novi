using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace IDataAccess
{
    public interface IOrderRepository
    {
        public Task InsertChart(ChartModel chart);
        public Task UpdateChart(ChartModel chart);
        public Task AddProduct(ProductModel product,ChartModel chart);
        public Task<bool> DeleteProduct(ProductModel product, ChartModel chart);
        public Task InsertOneOrder(OrderModel order);
        public Task<int> InsertManyOrder(List<OrderModel> orders);
        public Task<int> InsertManyOrderBulk(List<OrderModel> orders);

    }
}
