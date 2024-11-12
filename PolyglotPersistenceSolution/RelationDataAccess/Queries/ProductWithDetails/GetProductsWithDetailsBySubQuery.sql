	SELECT 
    ph.productId AS ProductId,
    ph.name AS ProductName,
    ph.price AS Price,
    ph.subCategoryId AS SubCategoryId,
    ph.produced AS Produced,
    ph.store AS Store,
    
    -- Detalji proizvoda
    (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId) AS ProductDetailId,
    (SELECT pd.shortDescription FROM ProductDetails pd WHERE ph.productId = pd.ProductId) AS ShortDescription,
    (SELECT pd.imageUrl FROM ProductDetails pd WHERE ph.productId = pd.ProductId) AS ImageUrl,

    -- Detalji automobila
    (SELECT c.yearManifactured FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarYearManufactured,
    (SELECT c.model FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarModel,
    (SELECT c.serialNumber FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarSerialNumber,
    (SELECT c.engineDisplacement FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarEngineDisplacement,
    (SELECT c.enginePower FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarEnginePower,
    (SELECT c.longDescription FROM Cars c WHERE c.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS CarLongDescription,

    -- Detalji uređaja
    (SELECT d.yearManifactured FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS DeviceYearManufactured,
    (SELECT d.serialNumber FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS DeviceSerialNumber,
    (SELECT d.weight FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS DeviceWeight,
    (SELECT d.storage FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS DeviceStorage,

    -- Detalji mobilnog telefona
    (SELECT m.screenDiagonal FROM Mobiles m WHERE m.ProductDetailId = (SELECT d.ProductDetailId FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId))) AS MobileScreenDiagonal,
    (SELECT m.operatingSystem FROM Mobiles m WHERE m.ProductDetailId = (SELECT d.ProductDetailId FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId))) AS MobileOperatingSystem,
    (SELECT m.color FROM Mobiles m WHERE m.ProductDetailId = (SELECT d.ProductDetailId FROM Devices d WHERE d.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId))) AS MobileColor,

    -- Detalji laptopa
    (SELECT l.ProductDetailId FROM Laptops l WHERE l.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS LaptopProductDetailId,

    -- Detalji filma
    (SELECT mo.yearRelease FROM Movies mo WHERE mo.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS MovieYearRelease,
    (SELECT mo.genre FROM Movies mo WHERE mo.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS MovieGenre,
    (SELECT mo.duration FROM Movies mo WHERE mo.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS MovieDuration,
    (SELECT mo.subtitling FROM Movies mo WHERE mo.ProductDetailId = (SELECT pd.ProductDetailId FROM ProductDetails pd WHERE ph.productId = pd.ProductId)) AS MovieSubtitling

FROM 
    ProductsHeader ph;
