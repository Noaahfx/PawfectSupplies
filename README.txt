-----------------------------------------------------------------------------------------------------------------------------
Admin Account:
Username: admin
Password: admin
-----------------------------------------------------------------------------------------------------------------------------
Database Script:
-- Create Database
CREATE DATABASE IF NOT EXISTS PawfectSuppliesDB;
USE PawfectSuppliesDB;

-- Create Users Table
CREATE TABLE Users (
    UserID            INT AUTO_INCREMENT PRIMARY KEY,
    Username          VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash      VARCHAR(255) NOT NULL,
    Role              VARCHAR(20) NOT NULL,
    Email             VARCHAR(255) NOT NULL,
    PhoneNumber       VARCHAR(15),
    FirstName         VARCHAR(50),
    LastName          VARCHAR(50),
    MobileCountryCode VARCHAR(10),
    IsVerified        TINYINT(1) DEFAULT 0,
    VerificationToken VARCHAR(100),
    Email2FA_Code     VARCHAR(10),
    Email2FA_Expiry   DATETIME,
    MFA_Type          VARCHAR(20),
    Is2FAEnabled      TINYINT(1) DEFAULT 0,
    CreatedAt         DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Insert an admin user into the Users table
INSERT INTO Users (Username, PasswordHash, Role, Email, IsVerified, CreatedAt)
VALUES ('admin', '$2a$11$.R0OA/YxWyvJdZ2vJ9hc8.4Iomg2ktNAuMyhYRaT.XYBsZ5x5G3QS', 
        'admin', 'admin@pawfectsupplies.com', 1, '2025-01-01 00:00:00');

-- Create Categories Table
CREATE TABLE Categories (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    Name       VARCHAR(100) NOT NULL
);

-- Create Products Table
CREATE TABLE Products (
    ProductID   INT AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(100) NOT NULL,
    Price       DECIMAL(10,2) NOT NULL,
    Image       VARCHAR(255),
    Category    VARCHAR(50),
    Rating      FLOAT DEFAULT 0,
    Stock       INT DEFAULT 0,
    Description VARCHAR(500),
    CategoryID  INT,
    IsFeatured  TINYINT(1) DEFAULT 0,
    ProductName VARCHAR(100),
    ImageUrl    VARCHAR(255),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE
);

-- Create HeroImages Table
CREATE TABLE HeroImages (
    HeroID   INT AUTO_INCREMENT PRIMARY KEY,
    ImageUrl VARCHAR(255) NOT NULL,
    Title    VARCHAR(100) NOT NULL,
    Subtitle VARCHAR(255),
    Link     VARCHAR(255),
    Priority INT DEFAULT 0
);

-- Create Orders Table
CREATE TABLE Orders (
    OrderID     INT AUTO_INCREMENT PRIMARY KEY,
    UserID      INT,
    TotalPrice  DECIMAL(10,2),
    OrderDate   DATETIME DEFAULT CURRENT_TIMESTAMP,
    OrderStatus VARCHAR(50) DEFAULT 'Pending',
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- Create OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID INT AUTO_INCREMENT PRIMARY KEY,
    OrderID       INT NOT NULL,
    ProductID     INT NOT NULL,
    Quantity      INT NOT NULL,
    Price         DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Create Cart Table
CREATE TABLE Cart (
    CartID    INT AUTO_INCREMENT PRIMARY KEY,
    UserID    INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity  INT NOT NULL DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Create Reviews Table
CREATE TABLE Reviews (
    ReviewID   INT AUTO_INCREMENT PRIMARY KEY,
    ProductID  INT NOT NULL,
    UserID     INT NOT NULL,
    Rating     INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment    VARCHAR(500),
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- 🔹 Stored Procedure: Get Cart Quantity
DELIMITER $$
CREATE PROCEDURE sp_GetCartQuantity(IN p_UserID INT, IN p_ProductID INT)
BEGIN
    SELECT Quantity FROM Cart WHERE UserID = p_UserID AND ProductID = p_ProductID;
END$$
DELIMITER ;

-- 🔹 Stored Procedure: Modify Cart Item (Add/Update Quantity)
DELIMITER $$
CREATE PROCEDURE sp_ModifyCartItem(IN p_UserID INT, IN p_ProductID INT, IN p_Change INT)
BEGIN
    DECLARE cart_exists INT;
    
    SELECT COUNT(*) INTO cart_exists FROM Cart WHERE UserID = p_UserID AND ProductID = p_ProductID;

    IF cart_exists > 0 THEN
        UPDATE Cart 
        SET Quantity = Quantity + p_Change
        WHERE UserID = p_UserID AND ProductID = p_ProductID;
    ELSE
        INSERT INTO Cart (UserID, ProductID, Quantity)
        VALUES (p_UserID, p_ProductID, 1);
    END IF;
END$$
DELIMITER ;

-- 🔹 Stored Procedure: Remove Cart Item
DELIMITER $$
CREATE PROCEDURE sp_RemoveCartItem(IN p_UserID INT, IN p_ProductID INT)
BEGIN
    DELETE FROM Cart WHERE UserID = p_UserID AND ProductID = p_ProductID;
END$$
DELIMITER ;

