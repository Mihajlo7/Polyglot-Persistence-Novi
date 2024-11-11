SELECT * FROM ProductsHeader ph
INNER JOIN Companies cp ON (cp.Id=ph.produced) INNER JOIN Sellers p ON (ph.produced=p.id)
LEFT JOIN Companies cs ON (cs.Id=ph.store) LEFT JOIN Sellers s ON(s.id=ph.store)
INNER JOIN DistributeProducts d ON (d.productId=ph.productId)
INNER JOIN Companies cd ON(cd.Id=d.sellerId) INNER JOIN Sellers sd ON(d.sellerId=sd.id);
