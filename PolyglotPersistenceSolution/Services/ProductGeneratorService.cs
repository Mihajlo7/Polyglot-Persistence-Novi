using Core.Data.Mongo;
using Core.ExternalData;
using Core.Models;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Services
{
    public static class ProductGeneratorService
    {
        private const int PRODUCT_ID = 4_000_000;
        public static List<ProductModel> GenerateProductsList(List<SellerModel> sellers, IEnumerable<ProductAndDetailsEx> productsRaw)
        {
            List<ProductModel> products = new();
            Random rand = new Random();
            int x = 0;
            int productNum = 1;
            int length = sellers.Count;
            foreach (var productRaw in productsRaw)
            {
                ProductModel product = new ProductModel()
                {
                    Name = productRaw.Name,
                    Id = productNum,
                    Price = productRaw.Price,
                    Distribute = new List<DistributeProductModel>(),
                    SubCategory = new SubCategoryModel() { Name = productRaw.SubCategory }
                };

                // get random number
                int index = rand.Next(length);
                SellerModel seller = sellers[index];

                product.Produced = seller;
                if (seller.HasShop)
                {
                    product.Store = seller;
                }
                else
                {
                    x++;
                }

                if (x == 7)
                {
                    while (true)
                    {
                        index = rand.Next(length);
                        seller = sellers[index];
                        if (seller.Id != product.Produced.Id)
                            break;
                    }
                    product.Store = seller;
                    x = 0;
                }

                int numberOfDistributors = rand.Next(1, 5);
                long[] guids = new long[numberOfDistributors];
                for (int i = 0; i < numberOfDistributors; i++)
                {

                    while (true)
                    {
                        index = rand.Next(length);
                        seller = sellers[index];
                        if (seller.Id != product.Produced.Id && !guids.Contains(seller.Id))
                            break;
                    }
                    guids[i] = seller.Id;
                    DistributeProductModel distributeProduct = new DistributeProductModel();
                    distributeProduct.Distributor = seller;
                    distributeProduct.DistributionPrice = rand.Next(10, 70);
                    product.Distribute.Add(distributeProduct);

                }

                if (productRaw is CarEx carRaw)
                {
                    product.Details = new CarDetailsModel()
                    {
                        ShortDescription = carRaw.ShortDescription,
                        ImageUrl = carRaw.ImageUrl,
                        Model = carRaw.SubCategory,
                        SerialNumber = carRaw.SerialNumber,
                        EngineDisplacement = $"{carRaw.EngineDisplacement} cm3",
                        EnginePower = $"{carRaw.EnginePower} hp",
                        YearManufactured = carRaw.Year,
                        LongDescription = carRaw.LongDescription,
                    };
                    product.SubCategory.Category = new CategoryModel() { Name = "Car" };
                }
                else if (productRaw is MobileEx mobileEx)
                {
                    product.Details = new MobileDetailsModel()
                    {
                        ShortDescription = mobileEx.ShortDescription,
                        ImageUrl = mobileEx.ImageUrl,
                        SerialNumber = mobileEx.SerialNumber,
                        YearManufactured = mobileEx.YearManifactured,
                        Weight = $"{mobileEx.Weight} kg",
                        Storage = $"{mobileEx.Storage} gb",
                        ScreenDiagonal = $"{mobileEx.ScreenDiagonal} in",
                        OperatingSystem = mobileEx.OperatingSystem,
                        Color = mobileEx.Color,
                    };
                    product.SubCategory.Category = new CategoryModel() { Name = "Mobile" };
                }
                products.Add(product);
            }
            return products;
        }
        public static List<ProductModel> ToMobilesFromRaws(this List<MobileEx> mobileExList, int indexProd, int indexDetail, int length)
        {
            List<ProductModel> products = new List<ProductModel>();

            for (int i = 0; i < length; i++)
            {
                ProductModel product = new ProductModel();
                product.Id = indexProd++;
                product.Name = mobileExList[i].Name;
                product.Price = mobileExList[i].Price;
                product.Details = new MobileDetailsModel()
                {
                    Id = indexDetail++,
                    ProductId = product.Id,
                    ShortDescription = mobileExList[i].ShortDescription,
                    ImageUrl = mobileExList[i].ImageUrl,
                    YearManufactured = mobileExList[i].YearManifactured,
                    SerialNumber = mobileExList[i].SerialNumber,
                    Weight = $"{mobileExList[i].Weight} g",
                    Storage = $"{mobileExList[i].Storage} gb",
                    ScreenDiagonal = $"{mobileExList[i].ScreenDiagonal} in",
                    OperatingSystem = mobileExList[i].OperatingSystem,
                    Color = mobileExList[i].Color,
                };
                product.SubCategory = new SubCategoryModel()
                {
                    Name = mobileExList[i].SubCategory,
                    Category = new CategoryModel() { Name = "Mobile" }
                };
                products.Add(product);
            }
            return products;
        }
        public static List<ProductModel> ToCarsFromRaws(this List<CarEx> carExList, int indexProd, int indexDetail, int length)
        {
            List<ProductModel> products = new List<ProductModel>();
            for (int i = 0; i < length; i++)
            {
                ProductModel product = new ProductModel();
                product.Id = indexProd++;
                product.Name = carExList[i].Name;
                product.Price = carExList[i].Price;
                product.Details = new CarDetailsModel()
                {
                    Id = indexDetail++,
                    ProductId = product.Id,
                    ShortDescription = carExList[i].ShortDescription,
                    ImageUrl = carExList[i].ImageUrl,
                    YearManufactured = carExList[i].Year,
                    Model = carExList[i].SubCategory,
                    SerialNumber = carExList[i].SerialNumber,
                    EngineDisplacement = $"{carExList[i].EngineDisplacement} cm3",
                    EnginePower = $"{carExList[i].EnginePower} ph",
                    LongDescription = carExList[i].LongDescription,
                };
                product.SubCategory = new SubCategoryModel()
                {
                    Name = carExList[i].SubCategory,
                    Category = new CategoryModel() { Name = "Car" }
                };
                products.Add(product);
            }
            return products;
        }
    }
}
