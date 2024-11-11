using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccess.HelperSqlData
{
    internal static class ProductSqlDataHelper
    {
        public static List<ProductModel> CreateProductsWithCompany(SqlDataReader reader)
        {
            List<ProductModel> products=new();

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

        public static List<ProductModel> CreateProductsWithCompanyBadWay(SqlDataReader reader)
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

        public static List<ProductModel> CreateProductWithDetailsBadWay(SqlDataReader reader)
        {
            List<ProductModel> products = new();

            while (reader.Read()) 
            {
                ProductModel product = new();
                var productId=reader.GetInt64(0);
                product.Id = productId;
                product.Name = reader.GetString(1);
                product.Price = reader.GetDecimal(2);

                var productDetailId= reader.GetInt64(6);
                var shortDescription= reader.GetString(8);
                var imageUrl= reader.GetString(9);

                if (!reader.IsDBNull(10))
                {
                    product.Details = new CarDetailsModel
                    {
                        Id = productDetailId,
                        ProductId = productId,
                        ShortDescription = shortDescription,
                        ImageUrl = imageUrl,
                        YearManufactured = reader.GetInt32(11),
                        Model= reader.GetString(12),
                        SerialNumber= reader.GetString(13),
                        EngineDisplacement=reader.GetString(14),
                        EnginePower = reader.GetString(15),
                        LongDescription= reader.GetString(16),
                    };
                } else if (!reader.IsDBNull(17))
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

        public static List<ProductModel> CreateProductsWithDetails(SqlDataReader reader)
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
    }
}
