DROP DATABASE IF EXISTS warehub;
CREATE DATABASE IF NOT EXISTS warehub;
USE warehub;

CREATE TABLE products (
    id CHAR(36) UNIQUE NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

CREATE INDEX `id` ON `products` (`id`);

INSERT INTO products VALUES('d3f3f9c4-34c3-4f3b-bc99-8129e54f26d1','Red T-shirt', 20.00);
INSERT INTO products VALUES('a12b9c3f-15e7-4a49-b8fc-6d97e37481ab','BLue T-shirt', 20.00);