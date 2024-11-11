CREATE OR ALTER PROC CreateSeller
    @CompanyId BIGINT,
    @DunsNumber NVARCHAR(100),
    @Name NVARCHAR(100), 
    @Telephone NVARCHAR(100),
    @Country NVARCHAR(50),
    @Address NVARCHAR(255),
    @City NVARCHAR(100),
    @HasShop BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Companies (id, dunsNumber, name, telephone, country) 
    VALUES (@CompanyId, @DunsNumber, @Name, @Telephone, @Country);

    INSERT INTO Sellers (id, address, city, hasShop) 
    VALUES (@CompanyId, @Address, @City, @HasShop);
END;
GO

CREATE OR ALTER PROC CreateCourier
    @CompanyId BIGINT,
    @DunsNumber NVARCHAR(100),
    @Name NVARCHAR(100),
    @Telephone NVARCHAR(100),
    @Country NVARCHAR(50),
    @DeliveryPrice DECIMAL(7,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Companies (id, dunsNumber, name, telephone, country) 
    VALUES (@CompanyId, @DunsNumber, @Name, @Telephone, @Country);

    INSERT INTO Couriers (id, deliveryPrice) 
    VALUES (@CompanyId, @DeliveryPrice);
END;
GO

CREATE OR ALTER PROC CreateCourierContract
    @CourierId BIGINT,
    @CompanyId BIGINT,
    @SerialNumContract NVARCHAR(255),
    @ContactInfo NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO CourierContracts (courierId, consumerId, serialNumContract, contactInfo) 
    VALUES (@CourierId, @CompanyId, @SerialNumContract, @ContactInfo);
END;
GO

-- Kreirajte tip pre nego što ga koristite u procedurama
DROP TYPE IF EXISTS CourierContractType;
GO

CREATE TYPE CourierContractType AS TABLE (
    CourierId BIGINT,
    CompanyId BIGINT,
    SerialNumContract NVARCHAR(255),
    ContactInfo NVARCHAR(500)
);
GO

CREATE OR ALTER PROC CreateCourierContractBulk
    @CourierContracts CourierContractType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO CourierContracts (courierId, consumerId, serialNumContract, contactInfo)
    SELECT CourierId, CompanyId, SerialNumContract, ContactInfo
    FROM @CourierContracts;
END;
GO

CREATE OR ALTER PROC DeleteCourierContactById
    @CourierId BIGINT,
    @CompanyId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM CourierContracts 
    WHERE companyId = @CompanyId AND courierId = @CourierId;
END;
GO

CREATE OR ALTER PROC DeleteCourierContactsByDeliveryPrice
    @DeliveryPrice DECIMAL(7,2)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM CourierContracts 
    WHERE courierId IN (SELECT id FROM Couriers WHERE deliveryPrice > @DeliveryPrice);
END;
GO

CREATE OR ALTER PROC UpdateCourierContactById
    @CourierId BIGINT,
    @CompanyId BIGINT,
    @SerialNumContract NVARCHAR(255),
    @ContactInfo NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CourierContracts
    SET serialNumContract = @SerialNumContract, contactInfo = @ContactInfo
    WHERE companyId = @CompanyId AND courierId = @CourierId;
END;
GO

CREATE OR ALTER PROC UpdateCourierContactByName
    @CourierName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CourierContracts
    SET serialNumContract = NEWID()
    WHERE courierId IN (SELECT id FROM Couriers WHERE name = @CourierName);
END;
GO
