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

        public static List<ProductModel> GetProductsWithCompany(this SqlDataReader reader)
        {
            List<ProductModel> products = new();

            while (reader.Read())
            {
                var productId = reader.GetInt64("ProductId");
                if (products.Any(p => p.Id == productId))
                {
                    var product = products.Find(p => p.Id == productId);

                    var distributeProduct = new DistributeProductModel()
                    {
                        DistributionPrice = reader.GetDecimal("DistributionPrice"),
                        Distributor = new SellerModel()
                        {
                            Id = reader.GetInt64("DistributeId"),
                            DunsNumber = reader.GetString("DistributeDunsNumber"),
                            Name = reader.GetString("DistributeName"),
                            Telephone = reader.GetString("DistributeTelephone"),
                            Country = reader.GetString("DistributeCountry"),
                            Address = reader.GetString("DistributeAddress"),
                            City = reader.GetString("DistributeCity"),
                            HasShop = reader.GetBoolean("DistributeShop")
                        }

                    };

                    product.Distribute.Add(distributeProduct);
                }
                else
                {
                    var productModel = new ProductModel()
                    {
                        Id = productId,
                        Name = reader.GetString("Name"),
                        Price = reader.GetDecimal("Price"),
                        Distribute = new List<DistributeProductModel>()
                    };
                    var producedId = reader.GetInt64("ProduceId");
                    var producedSeller = new SellerModel
                    {
                        Id = producedId,
                        DunsNumber = reader.GetString("ProduceDunsNumber"),
                        Name = reader.GetString("ProduceName"),
                        Telephone = reader.GetString("ProduceTelephone"),
                        Country = reader.GetString("ProduceCountry"),
                        Address = reader.GetString("ProduceAddress"),
                        City = reader.GetString("ProduceCity"),
                        HasShop = reader.GetBoolean("ProduceShop")
                    };
                    productModel.Produced = producedSeller;
                    if (!reader.IsDBNull(reader.GetOrdinal("StoreId")))
                    {
                        var storeSeller = new SellerModel
                        {
                            Id = reader.GetInt64("StoreId"),
                            DunsNumber = reader.GetString("StoreDunsNumber"),
                            Name = reader.GetString("StoreName"),
                            Telephone = reader.GetString("StoreTelephone"),
                            Country = reader.GetString("StoreCountry"),
                            Address = reader.GetString("StoreAddress"),
                            City = reader.GetString("StoreCity"),
                            HasShop = reader.GetBoolean("StoreShop")
                        };
                        productModel.Store = storeSeller;
                    }

                    var distributeProduct = new DistributeProductModel()
                    {
                        DistributionPrice = reader.GetDecimal("DistributionPrice"),
                        Distributor = new SellerModel()
                        {
                            Id = reader.GetInt64("DistributeId"),
                            DunsNumber = reader.GetString("DistributeDunsNumber"),
                            Name = reader.GetString("DistributeName"),
                            Telephone = reader.GetString("DistributeTelephone"),
                            Country = reader.GetString("DistributeCountry"),
                            Address = reader.GetString("DistributeAddress"),
                            City = reader.GetString("DistributeCity"),
                            HasShop = reader.GetBoolean("DistributeShop")
                        }

                    };
                    productModel.Distribute.Add(distributeProduct);
                    products.Add(productModel);
                }
            }
            return products;
        }

        public static List<ProductModel> GetProductsWithCompanyBadWay(this SqlDataReader reader)
        {
            List<ProductModel> products = new();

            while (reader.Read())
            {
                var productId = reader.GetInt64(0);
                if (products.Any(p => p.Id == productId))
                {
                    var product = products.Find(p => p.Id == productId);

                    var distributeProduct = new DistributeProductModel()
                    {
                        DistributionPrice = reader.GetDecimal(26),
                        Distributor = new SellerModel()
                        {
                            Id = reader.GetInt64(27),
                            DunsNumber = reader.GetString(28),
                            Name = reader.GetString(29),
                            Telephone = reader.GetString(30),
                            Country = reader.GetString(31),
                            Address = reader.GetString(33),
                            City = reader.GetString(34),
                            HasShop = reader.GetBoolean(35)
                        }

                    };

                    product.Distribute.Add(distributeProduct);
                }
                else
                {
                    var productModel = new ProductModel()
                    {
                        Id = productId,
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Distribute = new List<DistributeProductModel>()
                    };
                    var producedId = reader.GetInt64(4);
                    var producedSeller = new SellerModel
                    {
                        Id = producedId,
                        DunsNumber = reader.GetString(7),
                        Name = reader.GetString(8),
                        Telephone = reader.GetString(9),
                        Country = reader.GetString(10),
                        Address = reader.GetString(12),
                        City = reader.GetString(13),
                        HasShop = reader.GetBoolean(14)
                    };
                    productModel.Produced = producedSeller;
                    if (!reader.IsDBNull(reader.GetOrdinal("store")))
                    {
                        var storeSeller = new SellerModel
                        {
                            Id = reader.GetInt64(15),
                            DunsNumber = reader.GetString(16),
                            Name = reader.GetString(17),
                            Telephone = reader.GetString(18),
                            Country = reader.GetString(19),
                            Address = reader.GetString(21),
                            City = reader.GetString(22),
                            HasShop = reader.GetBoolean(23)
                        };
                        productModel.Store = storeSeller;
                    }

                    var distributeProduct = new DistributeProductModel()
                    {
                        DistributionPrice = reader.GetDecimal(26),
                        Distributor = new SellerModel()
                        {
                            Id = reader.GetInt64(27),
                            DunsNumber = reader.GetString(28),
                            Name = reader.GetString(29),
                            Telephone = reader.GetString(30),
                            Country = reader.GetString(31),
                            Address = reader.GetString(33),
                            City = reader.GetString(34),
                            HasShop = reader.GetBoolean(35)
                        }

                    };
                    productModel.Distribute.Add(distributeProduct);
                    products.Add(productModel);
                }
            }
            return products;
        }
        public static List<ProductModel> GetProductWithDetailsBadWay(this SqlDataReader reader)
        {
            List<ProductModel> products = new();

            while (reader.Read())
            {
                ProductModel product = new();
                var productId = reader.GetInt64(0);
                product.Id = productId;
                product.Name = reader.GetString(1);
                product.Price = reader.GetDecimal(2);

                var productDetailId = reader.GetInt64(6);
                var shortDescription = reader.GetString(8);
                var imageUrl = reader.GetString(9);

                if (!reader.IsDBNull(10))
                {
                    product.Details = new CarDetailsModel
                    {
                        Id = productDetailId,
                        ProductId = productId,
                        ShortDescription = shortDescription,
                        ImageUrl = imageUrl,
                        YearManufactured = reader.GetInt32(11),
                        Model = reader.GetString(12),
                        SerialNumber = reader.GetString(13),
                        EngineDisplacement = reader.GetString(14),
                        EnginePower = reader.GetString(15),
                        LongDescription = reader.GetString(16),
                    };
                }
                else if (!reader.IsDBNull(17))
                {
                    if (!reader.IsDBNull(22))
                    {
                        product.Details = new MobileDetailsModel
                        {
                            Id = productDetailId,
                            ProductId = productId,
                            ShortDescription = shortDescription,
                            ImageUrl = imageUrl,
                            YearManufactured = reader.GetInt32(18),
                            SerialNumber = reader.GetString(19),
                            Weight = reader.GetString(20),
                            Storage = reader.GetString(21),
                            ScreenDiagonal = reader.GetString(23),
                            OperatingSystem = reader.GetString(24),
                            Color = reader.GetString(25),
                        };
                    }
                    else if (!reader.IsDBNull(26))
                    {

                    }

                }
                else if (!reader.IsDBNull(30))
                {

                }
                products.Add(product);
            }
            return products;
        }

        public static List<ProductModel> GetProductsWithDetails(this SqlDataReader reader)
        {
            List<ProductModel> products = new();

            while (reader.Read())
            {
                ProductModel product = new();
                var productId = reader.GetInt64("ProductId");
                product.Id = productId;
                product.Name = reader.GetString("ProductName");
                product.Price = reader.GetDecimal("Price");

                var productDetailId = reader.GetInt64("ProductDetailId");
                var shortDescription = reader.GetString("ShortDescription");
                var imageUrl = reader.GetString("ImageUrl");

                if (!reader.IsDBNull("CarYearManufactured"))
                {
                    product.Details = new CarDetailsModel
                    {
                        Id = productDetailId,
                        ProductId = productId,
                        ShortDescription = shortDescription,
                        ImageUrl = imageUrl,
                        YearManufactured = reader.GetInt32("CarYearManufactured"),
                        Model = reader.GetString("CarModel"),
                        SerialNumber = reader.GetString("CarSerialNumber"),
                        EngineDisplacement = reader.GetString("CarEngineDisplacement"),
                        EnginePower = reader.GetString("CarEnginePower"),
                        LongDescription = reader.GetString("CarLongDescription"),
                    };
                }
                else if (!reader.IsDBNull("DeviceYearManufactured"))
                {
                    if (!reader.IsDBNull("MobileScreenDiagonal"))
                    {
                        product.Details = new MobileDetailsModel
                        {
                            Id = productDetailId,
                            ProductId = productId,
                            ShortDescription = shortDescription,
                            ImageUrl = imageUrl,
                            YearManufactured = reader.GetInt32("DeviceYearManufactured"),
                            SerialNumber = reader.GetString("DeviceSerialNumber"),
                            Weight = reader.GetString("DeviceWeight"),
                            Storage = reader.GetString("DeviceStorage"),
                            ScreenDiagonal = reader.GetString("MobileScreenDiagonal"),
                            OperatingSystem = reader.GetString("MobileOperatingSystem"),
                            Color = reader.GetString("MobileColor"),
                        };
                    }
                    else if (!reader.IsDBNull("LaptopProductDetailId"))
                    {

                    }

                }
                else if (!reader.IsDBNull("MovieYearRelease"))
                {

                }
                products.Add(product);
            }
            return products;
        }


        public static SqlCommand CreateProductDetailCommand(this SqlCommand command,ProductModel product)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ProductId",product.Id);
            
            if(product.Details is CarDetailsModel car)
            {
                command.Parameters.AddWithValue("@ProductDetailId",car.Id);
                command.Parameters.AddWithValue("@ProductType","Car");
                command.Parameters.AddWithValue("@ShortDescription",car.ShortDescription);
                command.Parameters.AddWithValue("@ImageUrl",car.ImageUrl);
                command.Parameters.AddWithValue("@YearManufactured",car.YearManufactured);
                command.Parameters.AddWithValue("@CarModel", car.Model);
                command.Parameters.AddWithValue("@SerialNumber", car.SerialNumber);
                command.Parameters.AddWithValue("@EngineDisplacement",car.EngineDisplacement);
                command.Parameters.AddWithValue("@EnginePower", car.EnginePower);
                command.Parameters.AddWithValue("@LongDescription",car.LongDescription);
            }
            else if (product.Details is MobileDetailsModel mobile)
            {
                command.Parameters.AddWithValue("@ProductDetailId", mobile.Id);
                command.Parameters.AddWithValue("@ProductType", "Mobile");
                command.Parameters.AddWithValue("@ShortDescription", mobile.ShortDescription);
                command.Parameters.AddWithValue("@ImageUrl", mobile.ImageUrl);
                command.Parameters.AddWithValue("@YearManufactured", mobile.YearManufactured);
                command.Parameters.AddWithValue("@SerialNumber", mobile.SerialNumber);
                command.Parameters.AddWithValue("@Weight", mobile.Weight);
                command.Parameters.AddWithValue("@Storage", mobile.Storage);
                command.Parameters.AddWithValue("@ScreenDiagonal", mobile.ScreenDiagonal);
                command.Parameters.AddWithValue("@OperatingSystem", mobile.OperatingSystem);
                command.Parameters.AddWithValue("@Color", mobile.Color);
            }
            return command;
        }
    }
}
