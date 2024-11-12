using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.Data.SqlClient;

namespace HybridDataAccess.HelperSqlData
{
    public static class ProductHelperData
    {
        public static List<SubCategoryModel> GetSubCategoriesHBadWay(this SqlDataReader reader)
        {
            List<SubCategoryModel> subCategories = new List<SubCategoryModel>();

            while (reader.Read())
            {
                SubCategoryModel subCategory = new SubCategoryModel();
                subCategory.Id = reader.GetInt64(0);
                subCategory.Name = reader.GetString(1);
                subCategory.Products=JsonSerializer.Deserialize<List<ProductModel>>(reader.GetString(2));

                subCategories.Add(subCategory);
            }
            return subCategories;
        }

        public static List<SubCategoryModel> GetSubCategoriesH(this SqlDataReader reader)
        {
            List<SubCategoryModel> subCategories = new List<SubCategoryModel>();

            while (reader.Read())
            {
                SubCategoryModel subCategory = new SubCategoryModel();
                subCategory.Id = reader.GetInt64("Id");
                subCategory.Name = reader.GetString("Name");
                subCategory.Products = JsonSerializer.Deserialize<List<ProductModel>>(reader.GetString("Products"));

                subCategories.Add(subCategory);
            }
            return subCategories;
        }
    }
}
