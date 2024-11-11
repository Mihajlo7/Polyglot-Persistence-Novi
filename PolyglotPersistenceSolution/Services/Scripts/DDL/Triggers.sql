CREATE TRIGGER trgUpdateTotalOnInsertOrDelete
ON ChartItems
AFTER INSERT, DELETE
AS
BEGIN
	DECLARE @total DECIMAL(10,2)

	SELECT @total= COALESCE(SUM(p.price),0)
	FROM ChartItems ci INNER JOIN ProductsHeader p ON ci.productId=p.ProductId;

	UPDATE c
	SET c.total=@total
	FROM Charts c
	WHERE c.id IN (
		SELECT chartId FROM inserted
		UNION
		SELECT chartid FROM deleted
	);
END;
