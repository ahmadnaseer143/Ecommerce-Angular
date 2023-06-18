-- Populating Users table
INSERT INTO Users (FirstName, LastName, Email, Address, Mobile, Password, CreatedAt, ModifiedAt)
VALUES ('John', 'Doe', 'johndoe@example.com', '123 Main St', '1234567890', 'password123', '2023-06-16', '2023-06-16'),
       ('Jane', 'Smith', 'janesmith@example.com', '456 Elm St', '9876543210', 'password456', '2023-06-16', '2023-06-16');

-- Populating PaymentMethods table
INSERT INTO PaymentMethods (Type, Provider, Available, Reason)
VALUES ('Credit Card', 'Visa', 'Yes', NULL),
       ('PayPal', 'PayPal', 'Yes', NULL),
       ('Debit Card', 'Mastercard', 'Yes', NULL);

-- Populating Offers table
INSERT INTO Offers (Title, Discount)
VALUES ('Summer Sale', 20),
       ('Clearance', 30),
       ('Holiday Special', 15);

-- Populating ProductCategories table
INSERT INTO ProductCategories (Category, SubCategory)
VALUES ('Electronics', 'Smartphones'),
       ('Electronics', 'Laptops'),
       ('Clothing', 'Men\'s Apparel'),
       ('Clothing', 'Women\'s Apparel');

-- Populating Products table
INSERT INTO Products (Title, Description, CategoryId, OfferId, Price, Quantity, ImageName)
VALUES ('iPhone 12', 'Latest smartphone from Apple', 1, 1, 999.99, 10, 'iphone12.jpg'),
       ('Samsung Galaxy S21', 'Flagship Android smartphone', 1, 1, 899.99, 15, 'galaxys21.jpg'),
       ('Dell XPS 13', 'Powerful and portable laptop', 2, 2, 1299.99, 5, 'xps13.jpg'),
       ('HP Pavilion', 'Affordable laptop for everyday use', 2, 2, 699.99, 8, 'pavilion.jpg'),
       ('Men\'s T-Shirt', 'Comfortable and stylish', 3, 3, 19.99, 20, 'mens_tshirt.jpg'),
       ('Women\'s Dress', 'Elegant and fashionable', 4, 3, 49.99, 12, 'womens_dress.jpg');

-- Populating Carts table
INSERT INTO Carts (UserId, Ordered, OrderedOn)
VALUES (1, 'true', '2023-06-16'),
       (2, 'false', '2023-06-16');

-- Populating CartItems table
INSERT INTO CartItems (CartId, ProductId)
VALUES (1, 1),
       (1, 3),
       (2, 2),
       (2, 4);

-- Populating Reviews table
INSERT INTO Reviews (UserId, ProductId, Review, CreatedAt)
VALUES (1, 1, 'Great phone!', '2023-06-16'),
       (2, 2, 'Excellent laptop!', '2023-06-16'),
       (1, 5, 'Nice t-shirt!', '2023-06-16');

-- Populating Payments table
INSERT INTO Payments (UserId, PaymentMethodId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt)
VALUES (1, 1, 1999.99, 10, 400, 1609.99, '2023-06-16'),
       (2, 2, 1299.99, 15, 0, 1314.99, '2023-06-16');

-- Populating Orders table
INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt)
VALUES (1, 1, 1, '2023-06-16'),
       (2, 2, 2, '2023-06-16');
