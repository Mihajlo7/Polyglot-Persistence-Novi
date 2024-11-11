using Core.ExternalData;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class CompanyGeneratorService
    {
        private const int COMPANY_ID = 3_000_000;
        public static List<ContractCourierModel> GenerateContractsForCourier(List<CourierModel> couriers, List<CompanyModel> companies)
        {
            List<ContractCourierModel> contracts = new List<ContractCourierModel>();
            Random rand = new Random();
            foreach (var courier in couriers)
            {
                int numOfContracts=rand.Next(4,7);
                HashSet<long> companyIdsUsed = new HashSet<long>();
                for (int i = 0; i < numOfContracts; i++)
                {
                    int randomDays=rand.Next(30,700);
                    ContractCourierModel contract = new ContractCourierModel();
                    contract.CourierId = courier.Id;
                    contract.SerialNumContract = Guid.NewGuid();
                    contract.EndOfContract=DateTime.Now.AddDays(randomDays);

                    while (true)
                    {
                        int index = rand.Next(companies.Count);
                        long companyId = companies[index].Id;
                        if (companyId != courier.Id && companyIdsUsed.Add(companyId))
                        {
                            contract.Company= companies[index];
                            
                            break;
                        }
                    }

                    contracts.Add(contract);
                }

            }
            return contracts;
        }

        public static List<SellerModel> ToSellers(this List<SellerEx> sellerRaws)
        {
            List<SellerModel> sellers= new List<SellerModel>();
            int index = COMPANY_ID;

            foreach(var sellerRaw in sellerRaws)
            {
                SellerModel seller = new();
                seller.Id = index++;
                seller.Name = sellerRaw.Name;
                seller.DunsNumber = sellerRaw.DunsNumber;
                seller.Telephone = sellerRaw.Telephone;
                seller.Country= sellerRaw.Country;
                seller.Address = sellerRaw.Address;
                seller.City = sellerRaw.City;
                seller.HasShop = sellerRaw.HasStore;

                sellers.Add(seller);
            }

            return sellers;
        }
        public static List<CourierModel> ToCouriers(List<CourierEx> courierExes)
        {
            List<CourierModel> couriers = new();
            int index = COMPANY_ID+100_000;

            foreach(var courierEx in courierExes) 
            {
                CourierModel courier = new();
                courier.Id = index++;
                courier.Name = courierEx.Name;
                courier.DunsNumber= courierEx.DunsNumber;
                courier.Telephone = courierEx.Telephone;
                courier.Country = courierEx.Country;
                courier.DeliveryPrice = courierEx.DeliveryPrice;

                couriers.Add(courier);
            }

            return couriers;
        }
    }
}
