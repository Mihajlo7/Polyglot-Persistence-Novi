SELECT 
    ph.productId AS ProductId,
    ph.name AS ProductName,
    ph.price AS Price,
    ph.subCategoryId AS SubCategoryId,
    ph.produced AS Produced,
    ph.store AS Store,
    pd.ProductDetailId AS ProductDetailId,
    --pd.ProductId AS DetailProductId,
    pd.shortDescription AS ShortDescription,
    pd.imageUrl AS ImageUrl,
    c.yearManifactured AS CarYearManufactured,
    c.model AS CarModel,
    c.serialNumber AS CarSerialNumber,
    c.engineDisplacement AS CarEngineDisplacement,
    c.enginePower AS CarEnginePower,
    c.longDescription AS CarLongDescription,
    d.yearManifactured AS DeviceYearManufactured,
    d.serialNumber AS DeviceSerialNumber,
    d.weight AS DeviceWeight,
    d.storage AS DeviceStorage,
    m.screenDiagonal AS MobileScreenDiagonal,
    m.operatingSystem AS MobileOperatingSystem,
    m.color AS MobileColor,
    l.ProductDetailId AS LaptopProductDetailId,
    mo.yearRelease AS MovieYearRelease,
    mo.genre AS MovieGenre,
    mo.duration AS MovieDuration,
    mo.subtitling AS MovieSubtitling

FROM 
    ProductsHeader ph
    INNER JOIN ProductDetails pd ON ph.productId = pd.ProductId
    LEFT JOIN Cars c ON c.ProductDetailId = pd.ProductDetailId
    LEFT JOIN Devices d ON d.ProductDetailId = pd.ProductDetailId
    LEFT JOIN Mobiles m ON m.ProductDetailId = d.ProductDetailId
    LEFT JOIN Laptops l ON pd.ProductDetailId = l.ProductDetailId
    LEFT JOIN Movies mo ON mo.ProductDetailId = pd.ProductDetailId
WHERE ph.productId=@ProductId;
