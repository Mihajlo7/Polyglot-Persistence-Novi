
-- Get all products --
SELECT product.id Id,product.name Name,product.price Price, s.id SubCategoryId, s.name SubCategoryName 
FROM SubCategories s
CROSS APPLY OPENJSON(products)
WITH (
	id BIGINT '$.id',
	name NVARCHAR(30) '$.name',
	price DECIMAL(5,2) '$.price'
	)AS product

-- Get product by Id --
SELECT product.id Id,product.name Name,product.price Price, s.id SubCategoryId, s.name SubCategoryName 
FROM SubCategories s
CROSS APPLY OPENJSON(products)
WITH (
	id BIGINT '$.id',
	name NVARCHAR(30) '$.name',
	price DECIMAL(5,2) '$.price'
	)AS product
WHERE product.id=@ProductId;

-- Get products By SubCategoryId --
SELECT products
FROM SubCategories
WHERE id=@SubCategoryId;

-- Get products By SubCategoryName
SELECT products
FROM SubCategories
WHERE name=@SubCategoryName;