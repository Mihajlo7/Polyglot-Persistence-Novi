using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ExternalData;
using Core.Models;

namespace Services
{
    public static class OrderGeneratorService
    {
        public static List<OrderModel> GenerateOrdersList
            (List<ConsumerModel> consumers,IEnumerable<ChartModel> charts,List<OrderEx> ordersRaw)
        {
            List<OrderModel > orders = new List<OrderModel> ();
            for (int i=0;i<charts.Count();i++)
            {
                ChartModel chart = charts.ElementAt(i);
                OrderModel order = new OrderModel ();
                order.Chart = chart;
                order.Consumer = chart.Consumer;

                OrderEx orderEx = ordersRaw[i];

                order.Address=orderEx.Address;
                order.PostalCode = orderEx.PostalCode; 
                order.Country=orderEx.Country;
                order.City = orderEx.City;
                
                ConsumerModel consumerWithCreditCards=consumers.Find(c=>c.Id==chart.Consumer.Id);
                if (consumerWithCreditCards != null)
                {
                    order.CreditCard = consumerWithCreditCards.CreditCards[0];
                }

                orders.Add(order);
                
            }
            return orders;
        }
    }
}
