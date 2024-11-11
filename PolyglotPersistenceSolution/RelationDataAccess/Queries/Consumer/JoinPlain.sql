SELECT u.*, c.*, cc.*, cf.*, cfu.friendUserId, cfu.friendEmail, cfu.friendName, cfu.FriendLastName,cfu.FriendBirthDate,cfu.telephone
FROM Users u
INNER JOIN Consumers c ON u.id = c.id
INNER JOIN CreditCard cc ON cc.consumerId = c.id
LEFT JOIN ConsumerFriends cf ON cf.consumerId = c.id
OUTER APPLY
(
	SELECT u1.id AS friendUserId, u1.email AS friendEmail,  -- kolone iz Users sa aliasom
	       c1.id AS friendConsumerId, c1.firstName AS friendName,c1.lastName AS FriendLastName, c1.birthDate FriendBirthDate,c1.telephone -- kolone iz Consumers sa aliasom
	FROM Users u1
	INNER JOIN Consumers c1 ON u1.id = c1.id
	WHERE u1.id = cf.friendId
) cfu;
