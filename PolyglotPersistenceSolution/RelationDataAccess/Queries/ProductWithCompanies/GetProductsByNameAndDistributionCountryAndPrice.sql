SELECT ph.productId ProductId,ph.name Name, ph.price Price,
cp.Id ProduceId, cp.dunsNumber ProduceDunsNumber, cp.name ProduceName, cp.telephone ProduceTelephone,cp.country ProduceCountry , p.address ProduceAddress, p.city ProduceCity, p.hasShop ProduceShop,
cs.Id StoreId, cs.dunsNumber StoreDunsNumber, cs.name StoreName, cs.telephone StoreTelephone,cs.country StoreCountry , s.address StoreAddress, s.city StoreCity, s.hasShop StoreShop,d.distributionPrice DistributionPrice,
cd.Id DistributeId, cd.dunsNumber DistributeDunsNumber, cd.name DistributeName, cd.telephone DistributeTelephone,cd.country DistributeCountry , sd.address DistributeAddress, sd.city DistributeCity, sd.hasShop DistributeShop
FROM ProductsHeader ph
INNER JOIN Companies cp ON (cp.Id=ph.produced) INNER JOIN Sellers p ON (cp.Id=p.id)
LEFT JOIN Companies cs ON (cs.Id=ph.store) LEFT JOIN Sellers s ON(cs.id=s.id)
INNER JOIN DistributeProducts d ON (d.productId=ph.productId)
INNER JOIN Companies cd ON(cd.Id=d.sellerId) INNER JOIN Sellers sd ON(d.sellerId=sd.id)
WHERE ph.name LIKE @ProductName AND cd.country=@DistributeCountry AND d.distributionPrice<@DistributePrice;
