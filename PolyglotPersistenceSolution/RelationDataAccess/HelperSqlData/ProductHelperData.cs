using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RelationDataAccess.HelperSqlData
{
    public static class ProductHelperData
    {
        public static List<ProductModel> GetProductsWithSubBadWay(this SqlDataReader reader)
        {
            List<ProductModel> products = new List<ProductModel>();

            while (reader.Read())
            {
                ProductModel product = new ProductModel();
                product.Id = reader.GetInt64(0);
                product.Name = reader.GetString(1);
                product.Price = reader.GetDecimal(2);
                product.SubCategory = new SubCategoryModel()
                {
                    Id = reader.GetInt64(6),
                    Name = reader.GetString(7),
                    Category = new CategoryModel
                    {
                        Id = reader.GetInt64(9),
                        Name = reader.GetString(10),
                    }
                };
                products.Add(product);
            }
            return products;
        }

        public static List<ProductModel> GetProductsWithSub(this SqlDataReader reader)
        {
            List<ProductModel> products = new List<ProductModel>();

            while (reader.Read())
            {
                ProductModel product = new ProductModel();
                product.Id = reader.GetInt64("ProductId");
                product.Name = reader.GetString("Name");
                product.Price = reader.GetDecimal("Price");
                product.SubCategory = new SubCategoryModel()
                {
                    Id = reader.GetInt64("SubCategoryId"),
                    Name = reader.GetString("SubCategoryName"),
                    Category = new CategoryModel
                    {
                        Id = reader.GetInt64("CategoryId"),
                        Name = reader.GetString("CategoryName"),
                    }
                };
                products.Add(product);
            }
            return products;
        }

        public static SqlCommand CreateProductWithCompanyBulk(this SqlCommand command,List<ProductModel> products)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId",typeof(long));
            dt.Columns.Add("ProductName",typeof(string));
            dt.Columns.Add("ProductPrice",typeof(decimal));
            dt.Columns.Add("SubCategoryId",typeof(long));

            foreach (ProductModel product in products) 
            {
                dt.Rows.Add(product.Id,product.Name,product.Price,product.SubCategory.Id);
            }
            command.Parameters.AddWithValue("@ProductsWithSubcategory",dt);

            return command;
        }

        public static SqlCommand CreateProductWithCompany(this SqlCommand command,ProductModel product)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ProductId", product.Id);
            command.Parameters.AddWithValue("@ProduceId", product.Produced.Id);
            command.Parameters.AddWithValue("@StoreId", product.Store.Id);
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId",typeof(long));
            dt.Columns.Add("SellerId",typeof(long));
            dt.Columns.Add("Price", typeof(decimal));
            foreach(var dp in product.Distribute)
            {
                dt.Rows.Add(dp.Product.Id,dp.Distributor.Id,dp.DistributionPrice);
            }
            command.Parameters.AddWithValue("@DistributeProducts",dt);

            return command;
        }
    }
}
