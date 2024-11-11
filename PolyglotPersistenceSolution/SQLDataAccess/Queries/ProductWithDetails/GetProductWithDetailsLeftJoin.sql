SELECT * 
FROM ProductsHeader ph LEFT JOIN ProductDetails pd ON (ph.productId=pd.ProductId)
LEFT JOIN Cars c ON (c.ProductDetailId=pd.ProductDetailId)
LEFT JOIN Devices d ON (d.ProductDetailId=pd.ProductDetailId) 
LEFT JOIN Mobiles m ON (m.ProductDetailId=d.ProductDetailId)
LEFT JOIN Laptops l ON (pd.ProductDetailId=l.ProductDetailId)
LEFT JOIN Movies mo ON (mo.ProductDetailId=pd.ProductDetailId)
