SELECT 
    ph.productId AS ProductId,
    ph.name AS Name,
    ph.price AS Price,
    produceData.ProduceId,
    produceData.ProduceDunsNumber,
    produceData.ProduceName,
    produceData.ProduceTelephone,
    produceData.ProduceCountry,
    produceData.ProduceAddress,
    produceData.ProduceCity,
    produceData.ProduceShop,
    storeData.StoreId,
    storeData.StoreDunsNumber,
    storeData.StoreName,
    storeData.StoreTelephone,
    storeData.StoreCountry,
    storeData.StoreAddress,
    storeData.StoreCity,
    storeData.StoreShop,
    distributionData.DistributionPrice,
    distributionData.DistributeId,
    distributionData.DistributeDunsNumber,
    distributionData.DistributeName,
    distributionData.DistributeTelephone,
    distributionData.DistributeCountry,
    distributionData.DistributeAddress,
    distributionData.DistributeCity,
    distributionData.DistributeShop
FROM 
    ProductsHeader ph
    CROSS APPLY (
        SELECT 
            cp.Id AS ProduceId,
            cp.dunsNumber AS ProduceDunsNumber,
            cp.name AS ProduceName,
            cp.telephone AS ProduceTelephone,
            cp.country AS ProduceCountry,
            p.address AS ProduceAddress,
            p.city AS ProduceCity,
            p.hasShop AS ProduceShop
        FROM Companies cp
        INNER JOIN Sellers p ON cp.Id = p.Id
        WHERE cp.Id = ph.produced
    ) AS produceData
    OUTER APPLY (
        SELECT 
            cs.Id AS StoreId,
            cs.dunsNumber AS StoreDunsNumber,
            cs.name AS StoreName,
            cs.telephone AS StoreTelephone,
            cs.country AS StoreCountry,
            s.address AS StoreAddress,
            s.city AS StoreCity,
            s.hasShop AS StoreShop
        FROM Companies cs
        LEFT JOIN Sellers s ON cs.Id = s.Id
        WHERE cs.Id = ph.store
    ) AS storeData
    CROSS APPLY (
        SELECT 
            d.distributionPrice AS DistributionPrice,
            cd.Id AS DistributeId,
            cd.dunsNumber AS DistributeDunsNumber,
            cd.name AS DistributeName,
            cd.telephone AS DistributeTelephone,
            cd.country AS DistributeCountry,
            sd.address AS DistributeAddress,
            sd.city AS DistributeCity,
            sd.hasShop AS DistributeShop
        FROM DistributeProducts d
        INNER JOIN Companies cd ON cd.Id = d.sellerId
        INNER JOIN Sellers sd ON sd.Id = d.sellerId
        WHERE d.productId = ph.productId
    ) AS distributionData;
