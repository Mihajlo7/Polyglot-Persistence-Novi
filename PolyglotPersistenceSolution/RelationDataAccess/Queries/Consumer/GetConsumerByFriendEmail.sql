SELECT 
    u.id AS Id,
    u.email AS Email,
    c.firstName AS FirstName,
    c.lastName AS LastName,
    c.birthDate AS BirthDate,
    c.telephone AS Telephone,
    cc.id AS CreditCardId,
    cc.number AS CreditCardNumber,
    cc.cardType AS CreditCardType,
    cf.friendId AS FriendId,
    cf.friendshipLevel AS FriendshipLevel,
    cf.establishedDate AS FriendEstablishedDate,
    cfu.friendUserId,
    cfu.friendEmail,
    cfu.friendName,
    cfu.FriendLastName,
    cfu.FriendBirthDate,
    cfu.telephone AS FriendTelephone
FROM Users u
INNER JOIN Consumers c ON u.id = c.id
INNER JOIN CreditCard cc ON cc.consumerId = c.id
LEFT JOIN ConsumerFriends cf ON cf.consumerId = c.id
OUTER APPLY
(
    SELECT 
        u1.id AS friendUserId, 
        u1.email AS friendEmail,
        c1.firstName AS friendName,
        c1.lastName AS FriendLastName,
        c1.birthDate AS FriendBirthDate,
        c1.telephone AS telephone
    FROM Users u1
    INNER JOIN Consumers c1 ON u1.id = c1.id
    WHERE u1.id = cf.friendId
) cfu
WHERE cf.friendId IN (SELECT cfu.id FROM Users cfu WHERE cfu.email= @Email);
