DROP TYPE IF EXISTS ChartProductType;
GO

CREATE TYPE ChartProductType AS TABLE(
	ProductId BIGINT
);
GO

CREATE OR ALTER PROC CreateChart
	@Products ChartProductType READONLY,
	@ConsumerId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ChartId BIGINT= NEXT VALUE FOR cha
	INSERT INTO Charts (id,status,createdAt,updatedAt) VALUES (@ChartId,1,GETDATE(),GETDATE());

	INSERT INTO ChartItems(chartId,productId,createdAt)
	SELECT @ChartId,ProductId,GETDATE()
	FROM @Products;
END;
GO

CREATE OR ALTER PROC UpdateChart
	@ChartId BIGINT,
	@Status INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Charts
	SET status=@Status
	WHERE id=@ChartId
END;
GO

CREATE OR ALTER PROC AddProductToChart
	@ProductId BIGINT,
	@ChartId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ChartItems (chartId,productId,createdAt) VALUES (@ChartId,@ProductId,GETDATE());
END;
GO

CREATE OR ALTER PROC DeleteProductToChart
	@ProductId BIGINT,
	@ChartId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM ChartItems WHERE chartId=@ChartId AND productId=@ProductId;
END;
GO

CREATE OR ALTER PROC CreateOrder
	@ChartId BIGINT,
	@ConsumerId BIGINT,
	@CreditCardId BIGINT,
	@TypeOrder INT,
	@Address NVARCHAR(100),
	@PostalCode NVARCHAR(20),
	@City NVARCHAR(50),
	@Country NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Orders (orderStatus,createdAt,updatedAt,typeOrder,address,postalCode,city,country,creditCardId,chartId,consumerId)
	VALUES (1,GETDATE(),GETDATE(),@TypeOrder,@Address,@PostalCode,@City,@Country,@CreditCardId,@ChartId,@ConsumerId)
END;
GO

DROP TYPE IF EXISTS OrderType;
GO

CREATE TYPE OrderType AS TABLE(
	ChartId BIGINT,
	ConsumerId BIGINT,
	CreditCardId BIGINT,
	TypeOrder INT,
	Address NVARCHAR(100),
	PostalCode NVARCHAR(20),
	City NVARCHAR(50),
	Country NVARCHAR(20)
);
GO

CREATE OR ALTER PROC CreateOrderBulk
	@Orders OrderType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Orders (orderStatus,createdAt,updatedAt,typeOrder,address,postalCode,city,country,creditCardId,chartId,consumerId)
	SELECT 1,GETDATE(),GETDATE(),TypeOrder,Address,PostalCode,City,Country,CreditCardId,ChartId,ConsumerId
	FROM @Orders;
END;
GO

CREATE OR ALTER PROC UpdateOrderStatus
	@OrderId BIGINT,
	@OrderStatus INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Orders SET orderStatus=@OrderStatus, updatedAt = GETDATE() WHERE id=@OrderId
END;
GO

CREATE OR ALTER PROC UpdateOrder
	@OrderId BIGINT,
	@ChartId BIGINT,
	@ConsumerId BIGINT,
	@CreditCardId BIGINT,
	@TypeOrder INT,
	@Address NVARCHAR(100),
	@PostalCode NVARCHAR(20),
	@City NVARCHAR(50),
	@Country NVARCHAR(20)
AS
BEGIN
	UPDATE Orders
    SET 
       chartId = @ChartId,
       creditCardId = @CreditCardId,
	   typeOrder = @TypeOrder,
       address = @Address,
       postalCode = @PostalCode,
       city = @City,
       country = @Country,
       updatedAt = GETDATE()
     WHERE id = @OrderId;
END;
GO