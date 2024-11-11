CREATE OR ALTER PROCEDURE CreateUser
    @Id BIGINT,
    @Email NVARCHAR(100),
    @Password NVARCHAR(255),
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @BirthDate DATE,
    @Telephone VARCHAR(20),
    @CreditCard1Id BIGINT,
    @CreditCardType1 NVARCHAR(20),
    @CreditCardNumber1 NVARCHAR(50),
    @CreditCard2Id BIGINT,
    @CreditCardType2 NVARCHAR(20),
    @CreditCardNumber2 NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (id, email, passwordHash, passwordSalt)
    VALUES (@Id, @Email, HASHBYTES('SHA2_256', @Password), CAST(@Password AS VARBINARY(255)));

    INSERT INTO Consumers (id, firstName, lastName, birthDate, telephone)
    VALUES (@Id, @FirstName, @LastName, @BirthDate, @Telephone);

    INSERT INTO CreditCard (id, consumerId, cardType, number) 
    VALUES (@CreditCard1Id, @Id, @CreditCardType1, @CreditCardNumber1);

    IF @CreditCardType2 IS NOT NULL AND @CreditCardNumber2 IS NOT NULL
        INSERT INTO CreditCard (id, consumerId, cardType, number) 
        VALUES (@CreditCard2Id, @Id, @CreditCardType2, @CreditCardNumber2);
END;
GO

-- Kreiranje korisničkog tipa pre upotrebe u bulk proceduri
DROP TYPE IF EXISTS UserConsumerType;
GO

CREATE TYPE UserConsumerType AS TABLE
(
    Id BIGINT,
    Email NVARCHAR(100),
    PasswordHash VARBINARY(255),
    PasswordSalt VARBINARY(255),
    FirstName NVARCHAR(255),
    LastName NVARCHAR(255),
    BirthDate DATE,
    Telephone VARCHAR(20),
    CreditCard1Id BIGINT,
    CreditCardType1 NVARCHAR(20),
    CreditCardNumber1 NVARCHAR(50),
    CreditCard2Id BIGINT,
    CreditCardType2 NVARCHAR(20),
    CreditCardNumber2 NVARCHAR(50)
);
GO

CREATE OR ALTER PROCEDURE CreateUsersBulk
    @UserData UserConsumerType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (id, email, passwordHash, passwordSalt)
    SELECT Id, Email, PasswordHash, PasswordSalt
    FROM @UserData;

    INSERT INTO Consumers (id, firstName, lastName, birthDate, telephone)
    SELECT Id, FirstName, LastName, BirthDate, Telephone
    FROM @UserData;

    INSERT INTO CreditCard (id, consumerId, cardType, number)
    SELECT CreditCard1Id, Id, CreditCardType1, CreditCardNumber1
    FROM @UserData;
END;
GO

CREATE OR ALTER PROCEDURE CreateConsumerFriends
    @ConsumerId BIGINT,
    @FriendId BIGINT,
    @FriendshipLevel INT,
    @EstablishedDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ConsumerFriends (consumerId, friendId, friendshipLevel, establishedDate)
    VALUES (@ConsumerId, @FriendId, @FriendshipLevel, @EstablishedDate);
END;
GO

-- Kreiranje korisničkog tipa za prijatelje
DROP TYPE IF EXISTS ConsumerFriendType;
GO

CREATE TYPE ConsumerFriendType AS TABLE
(
    consumerId BIGINT,
    friendId BIGINT,
    friendshipLevel INT CHECK (friendshipLevel BETWEEN 1 AND 10),
    establishedDate DATE
);
GO

CREATE OR ALTER PROCEDURE CreateConsumerFriendsBulk
    @FriendsData ConsumerFriendType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ConsumerFriends (consumerId, friendId, friendshipLevel, establishedDate)
    SELECT consumerId, friendId, friendshipLevel, establishedDate
    FROM @FriendsData;
END;
GO

-- Procedure za brisanje prijateljstva
CREATE OR ALTER PROCEDURE DeleteConsumerFriendById
    @ConsumerId BIGINT,
    @FriendId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ConsumerFriends 
    WHERE consumerId = @ConsumerId AND friendId = @FriendId;
END;
GO

CREATE OR ALTER PROCEDURE DeleteConsumerFriendByEstablishedDateAndLevel
    @Date DATE,
    @FriendshipLevel INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ConsumerFriends 
    WHERE establishedDate < @Date AND friendshipLevel >= @FriendshipLevel;
END;
GO

-- Procedure za ažuriranje nivoa prijateljstva
CREATE OR ALTER PROCEDURE UpdateConsumerFriendFriendshipLevel
    @ConsumerId BIGINT,
    @FriendId BIGINT,
    @NewFriendshipLevel INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ConsumerFriends
    SET friendshipLevel = @NewFriendshipLevel
    WHERE consumerId = @ConsumerId AND friendId = @FriendId;
END;
GO

CREATE OR ALTER PROCEDURE UpdateConsumerFriendByEmail
    @Email NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ConsumerFriends
    SET friendshipLevel = friendshipLevel - 2
    WHERE consumerId IN (
        SELECT id FROM Consumers c 
        WHERE c.id IN (SELECT id FROM Users u WHERE u.email = @Email)
    );
END;
GO
