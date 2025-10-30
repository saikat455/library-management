# SQL Challenge Answers

## 1. Database Indexing Strategy

### When to Use Indexes:
- **Primary Keys**: Automatically indexed
- **Foreign Keys**: Index on `ProductId` in Sales table for JOIN performance
- **Frequently Queried Columns**: Columns used in WHERE, ORDER BY, GROUP BY clauses
- **Large Tables**: Essential for tables with 10,000+ rows

### Index Examples:
```sql
-- Index on foreign key
CREATE INDEX IX_Sales_ProductId ON Sales(ProductId);

-- Composite index for date range queries
CREATE INDEX IX_Sales_Date_Product ON Sales(SaleDate, ProductId);

-- Index on frequently searched columns
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Name ON Users(Name);
```

### When NOT to Index:
- Small tables (< 1000 rows)
- Columns with low cardinality (few distinct values)
- Frequently updated columns (indexes slow down INSERT/UPDATE)

---

## 2. Find Top Performers (Students Query)
```sql
-- Using ROW_NUMBER() - SQL Server/PostgreSQL
SELECT StudentID, Name, Subject, Score
FROM (
    SELECT 
        StudentID, 
        Name, 
        Subject, 
        Score,
        ROW_NUMBER() OVER (PARTITION BY Subject ORDER BY Score DESC) as rn
    FROM Students
) ranked
WHERE rn = 1;

-- Alternative using GROUP BY and JOIN
SELECT s1.StudentID, s1.Name, s1.Subject, s1.Score
FROM Students s1
INNER JOIN (
    SELECT Subject, MAX(Score) as MaxScore
    FROM Students
    GROUP BY Subject
) s2 ON s1.Subject = s2.Subject AND s1.Score = s2.MaxScore;
```

---

## 3. Sales and Products Queries

### Query 1: Total Quantity Sold
```sql
SELECT SUM(quantity_sold) AS TotalQuantitySold 
FROM Sales;
```
**Result**: 20

### Query 2: Product with Highest Unit Price
```sql
SELECT product_name, unit_price 
FROM Products 
ORDER BY unit_price DESC 
LIMIT 1;
```
**Result**: Laptop, 500.00

### Query 3: Products Not Sold
```sql
SELECT p.product_id, p.product_name, p.category, p.unit_price
FROM Products p
LEFT JOIN Sales s ON p.product_id = s.product_id
WHERE s.sale_id IS NULL;
```
**Result**: None (all products have sales in the sample data)