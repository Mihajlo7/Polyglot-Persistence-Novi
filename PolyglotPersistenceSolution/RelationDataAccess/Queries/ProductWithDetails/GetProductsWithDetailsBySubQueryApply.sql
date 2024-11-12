SELECT 
    ph.productId AS ProductId,
    ph.name AS ProductName,
    ph.price AS Price,
    ph.subCategoryId AS SubCategoryId,
    ph.produced AS Produced,
    ph.store AS Store,

    pd.ProductDetailId AS ProductDetailId,
    pd.shortDescription AS ShortDescription,
    pd.imageUrl AS ImageUrl,


	c.ProductDetailId AS CarDetailsId,
    c.yearManifactured AS CarYearManufactured,
    c.model AS CarModel,
    c.serialNumber AS CarSerialNumber,
    c.engineDisplacement AS CarEngineDisplacement,
    c.enginePower AS CarEnginePower,
    c.longDescription AS CarLongDescription,


	d.ProductDetailId AS DeviceDetailsId,
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
    OUTER APPLY (
        SELECT pd.ProductDetailId, pd.shortDescription, pd.imageUrl
        FROM ProductDetails pd
        WHERE ph.productId = pd.ProductId
    ) pd
    OUTER APPLY (
        SELECT c.ProductDetailId, c.yearManifactured, c.model, c.serialNumber, c.engineDisplacement, c.enginePower, c.longDescription
        FROM Cars c
        WHERE c.ProductDetailId = pd.ProductDetailId
    ) c
    OUTER APPLY (
        SELECT d.ProductDetailId, d.yearManifactured, d.serialNumber, d.weight, d.storage
        FROM Devices d
        WHERE d.ProductDetailId = pd.ProductDetailId
    ) d
    OUTER APPLY (
        SELECT m.screenDiagonal, m.operatingSystem, m.color
        FROM Mobiles m
        WHERE m.ProductDetailId = d.ProductDetailId
    ) m
    OUTER APPLY (
        SELECT l.ProductDetailId
        FROM Laptops l
        WHERE l.ProductDetailId = pd.ProductDetailId
    ) l

    -- Pridružujemo Movies
    OUTER APPLY (
        SELECT mo.yearRelease, mo.genre, mo.duration, mo.subtitling
        FROM Movies mo
        WHERE mo.ProductDetailId = pd.ProductDetailId
    ) mo;
