DROP DATABASE IF EXISTS warehub;
CREATE DATABASE IF NOT EXISTS warehub;
USE warehub;

CREATE TABLE products (
    id CHAR(36) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    amount INT NOT NULL
);
