
CREATE OR ALTER PROCEDURE  CreateProduct
    @ProductId BIGINT,
    @ProductDetailId BIGINT,
    @ProductName NVARCHAR(100),
    @Price DECIMAL(10, 2),
    @SubCategoryName NVARCHAR(50),
    @ProducedBy BIGINT,
    @StoreId BIGINT = NULL,
    @ShortDescription NVARCHAR(255) = NULL,
    @ImageUrl NVARCHAR(255),
    @ProductType NVARCHAR(50), -- Tip proizvoda ('Car', 'Device', 'Movie', 'Mobile', 'Laptop')
    @DistributeProducts DistributeProductType READONLY, -- Lista companyId vrednosti za DistributeProducts
    -- Specifični parametri za Cars
    @YearManufactured INT = NULL,
    @CarModel NVARCHAR(100) = NULL,
    @SerialNumber NVARCHAR(500) = NULL,
    @EngineDisplacement NVARCHAR(20) = NULL,
    @EnginePower NVARCHAR(50) = NULL,
    @LongDescription NVARCHAR(2000) = NULL,
    -- Specifični parametri za Devices
    @Weight NVARCHAR(10) = NULL,
    @Storage NVARCHAR(10) = NULL,
    -- Specifični parametri za Movies
    @YearRelease INT = NULL,
    @Genre VARCHAR(50) = NULL,
    @Duration INT = NULL,
    @Subtitling NVARCHAR(50) = NULL,
    -- Specifični parametri za Mobiles
    @ScreenDiagonal VARCHAR(10) = NULL,
    @OperatingSystem VARCHAR(50) = NULL,
    @Color VARCHAR(50) = NULL,
    -- Specifični parametri za Laptops
    @Processor VARCHAR(10) = NULL,
    @RamMemory VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NewProductId BIGINT;
    DECLARE @NewProductDetailId BIGINT;
    DECLARE @SubCategoryId BIGINT;
    
    BEGIN TRY
        BEGIN TRANSACTION;

		
        -- Proveravamo da li podkategorija postoji prema imenu i tipu proizvoda
		SELECT @SubCategoryId = Id 
		FROM SubCategories 
		WHERE Name = @SubCategoryName 
              AND categoryId = (SELECT Id FROM Categories WHERE name = @ProductType);

		-- Ako podkategorija ne postoji, unosimo novu
		IF @SubCategoryId IS NULL
		BEGIN
			INSERT INTO SubCategories (name, categoryId) 
            VALUES (@SubCategoryName, (SELECT Id FROM Categories WHERE name = @ProductType));

			-- Ponovno dobijanje @SubCategoryId
			SELECT @SubCategoryId = Id 
			FROM SubCategories
			WHERE Name = @SubCategoryName 
                  AND categoryId = (SELECT Id FROM Categories WHERE name = @ProductType);
		END;

        -- Umetanje u ProductsHeader
        INSERT INTO ProductsHeader (ProductId, name, price, subCategoryId, produced, store)
        VALUES (@ProductId, @ProductName, @Price, @SubCategoryId, @ProducedBy, @StoreId);

        -- Umetanje u ProductDetails
        INSERT INTO ProductDetails (ProductDetailId, ProductId, shortDescription, imageUrl)
        VALUES (@ProductDetailId, @ProductId, @ShortDescription, @ImageUrl);

        -- Unos u specifične tabele na osnovu tipa proizvoda
        IF @ProductType = 'Car'
        BEGIN
            INSERT INTO Cars (ProductDetailId, yearManifactured, model, serialNumber, engineDisplacement, enginePower, longDescription)
            VALUES (@ProductDetailId, @YearManufactured, @CarModel, @SerialNumber, @EngineDisplacement, @EnginePower, @LongDescription);
        END
        ELSE IF @ProductType = 'Movie'
        BEGIN
            INSERT INTO Movies (ProductDetailId, yearRelease, genre, duration, subtitling)
            VALUES (@ProductDetailId, @YearRelease, @Genre, @Duration, @Subtitling);
        END
        ELSE IF @ProductType = 'Mobile'
        BEGIN
			INSERT INTO Devices (ProductDetailId, yearManifactured, serialNumber, weight, storage)
            VALUES (@ProductDetailId, @YearManufactured, @SerialNumber, @Weight, @Storage);

            INSERT INTO Mobiles (ProductDetailId, screenDiagonal, operatingSystem, color)
            VALUES (@ProductDetailId, @ScreenDiagonal, @OperatingSystem, @Color);
        END
        ELSE IF @ProductType = 'Laptop'
        BEGIN
			INSERT INTO Devices (ProductDetailId, yearManifactured, serialNumber, weight, storage)
            VALUES (@NewProductDetailId, @YearManufactured, @SerialNumber, @Weight, @Storage);

            INSERT INTO Laptops (ProductDetailId, processor, ramMemory, longDescription)
            VALUES (@ProductDetailId, @Processor, @RamMemory, @LongDescription);
        END

        -- Unos u DistributeProducts za svaki SellerId i Price iz DistributeProductType liste
        INSERT INTO DistributeProducts (productId, sellerId, distributionPrice)
        SELECT @ProductId, SellerId, Price 
        FROM @DistributeProducts;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROC CreateProductWithSubCategory
    @ProductId BIGINT,
    @ProductName NVARCHAR(100),
    @ProductPrice DECIMAL(10,2),
    @SubCategoryName NVARCHAR(50),
    @CategoryName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SubCategoryId BIGINT;
    SELECT @SubCategoryId = Id 
	FROM SubCategories 
	WHERE Name = @SubCategoryName AND categoryId = (SELECT id FROM Categories WHERE name = @CategoryName);

    IF @SubCategoryId IS NULL
		BEGIN
			INSERT INTO SubCategories (name, categoryId) 
            VALUES (@SubCategoryName, (SELECT Id FROM Categories WHERE name = @CategoryName));

			SELECT @SubCategoryId = Id 
			FROM SubCategories
			WHERE Name = @SubCategoryName AND categoryId = (SELECT Id FROM Categories WHERE name = @CategoryName);
		END;
    INSERT INTO ProductsHeader (id,name,price,subCategoryId) VALUES (@ProductId,@ProductName,@ProductPrice,@SubCategoryId)
END;
GO

DROP TYPE IF EXISTS ProductWithSubCategoryType;
GO

CREATE TYPE ProductWithSubCategoryType AS TABLE(
    ProductId BIGINT,
    ProductName NVARCHAR(100),
    ProductPrice DECIMAL(10,2),
    SubCategoryName NVARCHAR(50)
);
GO

CREATE OR ALTER PROC CreateProductWithSubCategoryBulk
    @ProductsWithSubcategory ProductWithSubCategoryType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ProductsHeader (id,name,price,subCategoryId)
    SELECT ProductId,ProductName,ProductPrice, (SELECT id FROM SubCategories WHERE name=SubCategoryName)
    FROM @ProductsWithSubcategory;
END;
GO

CREATE OR ALTER PROC CreateProductWithDetail
    @ProductId BIGINT,
    @ProductDetailId BIGINT,
    @ProductType NVARCHAR(20),
    @ShortDescription NVARCHAR(255) = NULL,
    @ImageUrl NVARCHAR(255),
    @YearManufactured INT = NULL,
    @CarModel NVARCHAR(100) = NULL,
    @SerialNumber NVARCHAR(500) = NULL,
    @EngineDisplacement NVARCHAR(20) = NULL,
    @EnginePower NVARCHAR(50) = NULL,
    @LongDescription NVARCHAR(2000) = NULL,
    -- Specifični parametri za Devices
    @Weight NVARCHAR(10) = NULL,
    @Storage NVARCHAR(10) = NULL,
    -- Specifični parametri za Movies
    @YearRelease INT = NULL,
    @Genre VARCHAR(50) = NULL,
    @Duration INT = NULL,
    @Subtitling NVARCHAR(50) = NULL,
    -- Specifični parametri za Mobiles
    @ScreenDiagonal VARCHAR(10) = NULL,
    @OperatingSystem VARCHAR(50) = NULL,
    @Color VARCHAR(50) = NULL,
    -- Specifični parametri za Laptops
    @Processor VARCHAR(10) = NULL,
    @RamMemory VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ProductDetails (productDetailId,productId,shortDescription,imageUrl)
    VALUES (@ProductDetailId,@ProductId,@ShortDescription,@ImageUrl);

    IF @ProductType = 'Car'
        BEGIN
            INSERT INTO Cars (ProductDetailId, yearManifactured, model, serialNumber, engineDisplacement, enginePower, longDescription)
            VALUES (@ProductDetailId, @YearManufactured, @CarModel, @SerialNumber, @EngineDisplacement, @EnginePower, @LongDescription);
        END
        ELSE IF @ProductType = 'Movie'
        BEGIN
            INSERT INTO Movies (ProductDetailId, yearRelease, genre, duration, subtitling)
            VALUES (@ProductDetailId, @YearRelease, @Genre, @Duration, @Subtitling);
        END
        ELSE IF @ProductType = 'Mobile'
        BEGIN
			INSERT INTO Devices (ProductDetailId, yearManifactured, serialNumber, weight, storage)
            VALUES (@ProductDetailId, @YearManufactured, @SerialNumber, @Weight, @Storage);

            INSERT INTO Mobiles (ProductDetailId, screenDiagonal, operatingSystem, color)
            VALUES (@ProductDetailId, @ScreenDiagonal, @OperatingSystem, @Color);
        END
        ELSE IF @ProductType = 'Laptop'
        BEGIN
			INSERT INTO Devices (ProductDetailId, yearManifactured, serialNumber, weight, storage)
            VALUES (@ProductDetailId, @YearManufactured, @SerialNumber, @Weight, @Storage);

            INSERT INTO Laptops (ProductDetailId, processor, ramMemory, longDescription)
            VALUES (@ProductDetailId, @Processor, @RamMemory, @LongDescription);
        END
END;
GO

DROP TYPE IF EXISTS DistributeProductType;
GO

CREATE TYPE DistributeProductType AS TABLE(
    ProductId BIGINT,
	SellerId BIGINT,
	Price DECIMAL(7,2)
);
GO

CREATE OR ALTER PROC CreateProductWithCompanies
    @ProductId BIGINT,
    @ProduceId BIGINT,
    @StoreId BIGINT,
    @DistributeProducts DistributeProductType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ProductsHeader
    SET produced=@ProduceId, store=@StoreId
    WHERE productId=@ProductId;

    INSERT INTO DistributeProducts (productId,sellerId,distributionPrice)
    SELECT @ProductId,SellerId,Price
    FROM @DistributeProducts;
END;
GO

DROP TYPE IF EXISTS ProductWithCompaniesType;
GO

CREATE TYPE ProductWithCompaniesType AS TABLE(
    ProductId BIGINT,
    ProduceId BIGINT,
    StoreId BIGINT 
);
GO

CREATE OR ALTER PROC CreateProductWithCompaniesBulk
    @ProductsWithCompanies ProductWithCompaniesType READONLY,
    @DistributeProducts DistributeProductType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ProductsHeader
    SET produced=pc.ProduceId, store=pc.StoreId
    FROM @ProductsWithCompanies pc
    WHERE ProductsHeader.productId=pc.ProductId;

    INSERT INTO DistributeProducts (productId,sellerId,distributionPrice)
    SELECT ProductId,SellerId,Price
    FROM @DistributeProducts;
END;
GO


