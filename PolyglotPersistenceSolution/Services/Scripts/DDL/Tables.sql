

-- Create Sequences --

CREATE SEQUENCE users_seq AS BIGINT START WITH 1000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE credit_card_seq AS BIGINT START WITH 2000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE company_seq AS BIGINT START WITH 3000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE category_seq AS BIGINT START WITH 100 INCREMENT BY 5 NO CYCLE;
CREATE SEQUENCE sub_category_seq AS BIGINT START WITH 1 INCREMENT BY 5 NO CYCLE;
CREATE SEQUENCE product_seq AS BIGINT START WITH 4000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE product_detail_seq AS BIGINT START WITH 5000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE chart_seq AS BIGINT START WITH 6000000 INCREMENT BY 1 NO CYCLE;
CREATE SEQUENCE order_seq AS BIGINT START WITH 7000000 INCREMENT BY 1 NO CYCLE;

-- Create Tables --

CREATE TABLE Users (
	id BIGINT DEFAULT NEXT VALUE FOR users_seq CONSTRAINT user_pk PRIMARY KEY,
	email NVARCHAR(100) NOT NULL,
	passwordHash NVARCHAR(255) NOT NULL,
	passwordSalt VARBINARY(255) NOT NULL
);

CREATE TABLE Consumers (
	id BIGINT CONSTRAINT consumer_pk PRIMARY KEY,
	firstName NVARCHAR(255) NOT NULL,
	lastName NVARCHAR(255) NOT NULL,
	birthDate DATE NOT NULL,
	telephone VARCHAR(20) NOT NULL
	CONSTRAINT consumer_fk FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE
)

CREATE TABLE ConsumerFriends (
	consumerId BIGINT,
	friendId BIGINT,
	friendshipLevel INT CONSTRAINT friendship_level_ck CHECK (friendshipLevel BETWEEN 1 AND 10),
	establishedDate DATE DEFAULT GETDATE(),
	CONSTRAINT consumer_friend_pk PRIMARY KEY (consumerId,friendId),
	CONSTRAINT consumer_friend_fk FOREIGN KEY (consumerId) REFERENCES Consumers(id) ON DELETE NO ACTION,
	CONSTRAINT friend_consumer_fk FOREIGN KEY (friendId) REFERENCES Consumers(id) ON DELETE NO ACTION
);

CREATE TABLE Administrators (
	id BIGINT CONSTRAINT admin_pk PRIMARY KEY,
	joinedDate DATE NOT NULL,
	role VARCHAR(20) NOT NULL CONSTRAINT admin_role CHECK (role IN ('BASIC_ADMIN','SUPER_ADMIN')),
	CONSTRAINT admin_fk FOREIGN KEY (Id) REFERENCES Users(Id)
);

CREATE TABLE CreditCard (
	id BIGINT DEFAULT NEXT VALUE FOR credit_card_seq CONSTRAINT credit_card_pk PRIMARY KEY,
	consumerId BIGINT NOT NULL CONSTRAINT credit_card_fk FOREIGN KEY (consumerId) REFERENCES Consumers(Id) ON DELETE CASCADE,
	number NVARCHAR(50) NOT NULL,
	cardType NVARCHAR(20) NOT NULL
);

CREATE TABLE Companies(
	id BIGINT DEFAULT NEXT VALUE FOR company_seq CONSTRAINT company_pk PRIMARY KEY,
	dunsNumber NVARCHAR(100) NOT NULL,
	name NVARCHAR(100) NOT NULL,
	telephone NVARCHAR(100) NOT NULL,
	country NVARCHAR(50) NOT NULL
);

CREATE TABLE Sellers (
	id BIGINT CONSTRAINT seller_pk PRIMARY KEY,
	address NVARCHAR(255) NOT NULL,
	city NVARCHAR(100) NOT NULL,
	hasShop BIT NOT NULL
	CONSTRAINT seller_fk FOREIGN KEY (id) REFERENCES Companies(id) ON DELETE CASCADE
);

CREATE TABLE Couriers (
	id BIGINT CONSTRAINT courier_pk PRIMARY KEY,
	deliveryPrice DECIMAL(7,2) NOT NULL
	CONSTRAINT courier_fk FOREIGN KEY (id) REFERENCES Companies(id) ON DELETE CASCADE
);

CREATE TABLE CourierContacts (
	courierId BIGINT,
	companyId BIGINT,
	serialNumContact NVARCHAR(255) NOT NULL,
	contactInfo NVARCHAR(500)
	CONSTRAINT courier_contact_pk PRIMARY KEY (courierId,companyId),
	CONSTRAINT courier_c_fk FOREIGN KEY (courierId) REFERENCES Couriers(id) ,
	CONSTRAINT company_c_fk FOREIGN KEY (companyId) REFERENCES Companies(id)
);

CREATE TABLE Categories (
	id BIGINT DEFAULT NEXT VALUE FOR category_seq CONSTRAINT category_pk PRIMARY KEY,
	name NVARCHAR(50) NOT NULL
);

CREATE TABLE SubCategories (
	id BIGINT DEFAULT NEXT VALUE FOR sub_category_seq CONSTRAINT subCategory_pk PRIMARY KEY,
	name NVARCHAR(50) NOT NULL,
	categoryId BIGINT CONSTRAINT subCategory_fk FOREIGN KEY (categoryId) REFERENCES Categories(id) ON DELETE SET NULL ON UPDATE CASCADE
);

CREATE TABLE ProductsHeader (
	productId BIGINT DEFAULT NEXT VALUE FOR product_seq CONSTRAINT product_pk PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	price DECIMAL(10,2) NOT NULL,
	subCategoryId BIGINT CONSTRAINT product_fk FOREIGN KEY (subCategoryId) REFERENCES SubCategories(id) ON DELETE SET NULL ON UPDATE CASCADE,
	produced BIGINT NULL CONSTRAINT produced_seller_fk FOREIGN KEY (produced) REFERENCES Sellers(id),
	store BIGINT NULL CONSTRAINT store_seller_fk FOREIGN KEY (store) REFERENCES Sellers (id)
);

CREATE TABLE DistributeProducts (
	productId BIGINT,
	sellerId BIGINT,
	distributionPrice DECIMAL(7,2),
	CONSTRAINT dist_pk PRIMARY KEY (productId,sellerId),
	CONSTRAINT p_seller_fk FOREIGN KEY (sellerid) REFERENCES Sellers(id),
	CONSTRAINT product_header_fk FOREIGN KEY (productId) REFERENCES ProductsHeader(productId)
)

CREATE TABLE ProductDetails (
	productDetailId BIGINT DEFAULT NEXT VALUE FOR product_detail_seq CONSTRAINT product_detail_pk PRIMARY KEY,
	productId BIGINT CONSTRAINT product_unique UNIQUE,
	shortDescription NVARCHAR(255) NULL,
	imageUrl NVARCHAR(255) NOT NULL
	CONSTRAINT product_detail_fk FOREIGN KEY (productId) REFERENCES ProductsHeader(productId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Cars (
	productDetailId BIGINT CONSTRAINT cars_pk PRIMARY KEY,
	yearManifactured INT NOT NULL,
	model NVARCHAR(100) NOT NULL,
	serialNumber NVARCHAR(500) NOT NULL,
	engineDisplacement NVARCHAR(20) NOT NULL,
	enginePower NVARCHAR(50) NOT NULL,
	longDescription NVARCHAR(2000),
	CONSTRAINT cars_fk FOREIGN KEY (ProductDetailId) REFERENCES ProductDetails (productDetailId)
);

CREATE TABLE Devices (
	productDetailId BIGINT CONSTRAINT device_pk PRIMARY KEY,
	yearManifactured INT NOT NULL,
	serialNumber NVARCHAR(500) NOT NULL,
	weight NVARCHAR(10) NULL,
	storage NVARCHAR(10) NOT NULL
	CONSTRAINT device_fk FOREIGN KEY (ProductDetailId) REFERENCES ProductDetails (ProductDetailId)
);

CREATE TABLE Movies (
	productDetailId BIGINT CONSTRAINT movie_pk PRIMARY KEY,
	yearRelease INT NOT NULL,
	genre VARCHAR(50) NOT NULL,
	duration INT NOT NULL,
	subtitling NVARCHAR(50) NOT NULL,
	CONSTRAINT movie_fk FOREIGN KEY (ProductDetailId) REFERENCES ProductDetails (ProductDetailId)
);

CREATE TABLE Mobiles (
	productDetailId BIGINT CONSTRAINT mobile_pk PRIMARY KEY,
	screenDiagonal VARCHAR(10) NOT NULL,
	operatingSystem VARCHAR(50) NOT NULL,
	color VARCHAR(50) NOT NULL,
	CONSTRAINT mobile_fk FOREIGN KEY (ProductDetailId) REFERENCES Devices (ProductDetailId)
);

CREATE TABLE Laptops (
	productDetailId BIGINT CONSTRAINT laptop_pk PRIMARY KEY,
	processor VARCHAR(10) NOT NULL,
	ramMemory VARCHAR(50) NOT NULL,
	longDescription NVARCHAR(2000),
	CONSTRAINT laptop_fk FOREIGN KEY (ProductDetailId) REFERENCES Devices (ProductDetailId)
);

CREATE TABLE Charts (
	id BIGINT DEFAULT NEXT VALUE FOR chart_seq CONSTRAINT chart_pk PRIMARY KEY,
	total DECIMAL(10,2) NOT NULL DEFAULT 0.0,
	status INT NOT NULL CONSTRAINT chart_status CHECK (status IN (0,1,2,3)),
	createdAt DATETIME NOT NULL,
	updatedAt DATETIME NOT NULL,
	consumer_id BIGINT NOT NULL CONSTRAINT chart_fk FOREIGN KEY (consumer_id) REFERENCES Consumers(id)
);

CREATE TABLE ChartItems (
	chartId BIGINT,
	productId BIGINT,
	createdAt DATETIME NOT NULL,
	CONSTRAINT chart_item_pk PRIMARY KEY(chartId,productId),
	CONSTRAINT chart_chart_item_fk FOREIGN KEY (chartId) REFERENCES Charts(id),
	CONSTRAINT product_chart_item_fk FOREIGN KEY (productId) REFERENCES ProductsHeader(productId)
);

CREATE TABLE Orders  (
	id BIGINT DEFAULT NEXT VALUE FOR order_seq CONSTRAINT order_pk PRIMARY KEY,
	orderStatus INT DEFAULT 0  CONSTRAINT order_value CHECK(orderStatus IN (0,1,2,3)),
	createdAt DATETIME NOT NULL,
	updatedAt DATETIME NOT NULL,
	typeOrder INT DEFAULT 0 CONSTRAINT type_order_ck CHECK(typeOrder IN (0,1,2)),
	address NVARCHAR(100),
	postalCode NVARCHAR(20),
	city NVARCHAR(50),
	country NVARCHAR(20),
	creditCardId BIGINT NOT NULL CONSTRAINT credit_card_order_fk FOREIGN KEY (creditCardId) REFERENCES CreditCard(id),
	chartId BIGINT CONSTRAINT chart_id FOREIGN KEY (chartId) REFERENCES Charts(id),
	consumerId BIGINT CONSTRAINT consumer_order_fk FOREIGN KEY (consumerId) REFERENCES Consumers(id)
	);


-- Create trigger for chart total --

-- Insert some data --
INSERT INTO Categories(name) VALUES ('Car');
INSERT INTO Categories(name) VALUES ('Mobile');
INSERT INTO Categories(name) VALUES ('Laptop');
INSERT INTO Categories(name) VALUES ('Movie');

INSERT INTO Users (email, passwordHash, passwordSalt)
VALUES 
    ('admin@ecommerce.com', HASHBYTES('SHA2_256', 'admin'), CAST('admin' AS VARBINARY(255))),
    ('mihajlo@ecommerce.com', HASHBYTES('SHA2_256', 'mihajlo'), CAST('mihajlo' AS VARBINARY(255)));

INSERT INTO Administrators (Id,joinedDate,role) SELECT id,'2024-10-26','SUPER_ADMIN' FROM Users WHERE email='admin@ecommerce.com';
INSERT INTO Administrators (Id,joinedDate,role) SELECT id,'2024-10-26','SUPER_ADMIN' FROM Users WHERE email='mihajlo@ecommerce.com';

