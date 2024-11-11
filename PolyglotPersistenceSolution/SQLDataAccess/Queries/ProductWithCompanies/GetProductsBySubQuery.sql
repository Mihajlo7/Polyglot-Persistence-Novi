SELECT 
    ph.productId AS ProductId,
    ph.name AS Name,
    ph.price AS Price,
    -- Podupit za proizvođača, sa ograničenjem na jedan red
    (SELECT TOP 1 cp.Id FROM Companies cp WHERE cp.Id = ph.produced) AS ProduceId,
    (SELECT TOP 1 cp.dunsNumber FROM Companies cp WHERE cp.Id = ph.produced) AS ProduceDunsNumber,
    (SELECT TOP 1 cp.name FROM Companies cp WHERE cp.Id = ph.produced) AS ProduceName,
    (SELECT TOP 1 cp.telephone FROM Companies cp WHERE cp.Id = ph.produced) AS ProduceTelephone,
    (SELECT TOP 1 cp.country FROM Companies cp WHERE cp.Id = ph.produced) AS ProduceCountry,
    (SELECT TOP 1 p.address FROM Sellers p WHERE p.Id = ph.produced) AS ProduceAddress,
    (SELECT TOP 1 p.city FROM Sellers p WHERE p.Id = ph.produced) AS ProduceCity,
    (SELECT TOP 1 p.hasShop FROM Sellers p WHERE p.Id = ph.produced) AS ProduceShop,
    -- Podupit za prodavnicu, sa ograničenjem na jedan red
    (SELECT TOP 1 cs.Id FROM Companies cs WHERE cs.Id = ph.store) AS StoreId,
    (SELECT TOP 1 cs.dunsNumber FROM Companies cs WHERE cs.Id = ph.store) AS StoreDunsNumber,
    (SELECT TOP 1 cs.name FROM Companies cs WHERE cs.Id = ph.store) AS StoreName,
    (SELECT TOP 1 cs.telephone FROM Companies cs WHERE cs.Id = ph.store) AS StoreTelephone,
    (SELECT TOP 1 cs.country FROM Companies cs WHERE cs.Id = ph.store) AS StoreCountry,
    (SELECT TOP 1 s.address FROM Sellers s WHERE s.Id = ph.store) AS StoreAddress,
    (SELECT TOP 1 s.city FROM Sellers s WHERE s.Id = ph.store) AS StoreCity,
    (SELECT TOP 1 s.hasShop FROM Sellers s WHERE s.Id = ph.store) AS StoreShop,
    -- Podupit za distributera, sa ograničenjem na jedan red
    (SELECT TOP 1 d.distributionPrice FROM DistributeProducts d WHERE d.productId = ph.productId) AS DistributionPrice,
    (SELECT TOP 1 cd.Id FROM Companies cd WHERE cd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeId,
    (SELECT TOP 1 cd.dunsNumber FROM Companies cd WHERE cd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeDunsNumber,
    (SELECT TOP 1 cd.name FROM Companies cd WHERE cd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeName,
    (SELECT TOP 1 cd.telephone FROM Companies cd WHERE cd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeTelephone,
    (SELECT TOP 1 cd.country FROM Companies cd WHERE cd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeCountry,
    (SELECT TOP 1 sd.address FROM Sellers sd WHERE sd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeAddress,
    (SELECT TOP 1 sd.city FROM Sellers sd WHERE sd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeCity,
    (SELECT TOP 1 sd.hasShop FROM Sellers sd WHERE sd.Id = (SELECT TOP 1 d.sellerId FROM DistributeProducts d WHERE d.productId = ph.productId)) AS DistributeShop
FROM 
    ProductsHeader ph;
