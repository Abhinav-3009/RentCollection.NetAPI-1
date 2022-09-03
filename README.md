# RentCollection.NetAPI

# db-context
```
scaffold-dbcontext "Server=127.0.0.1,1433;Database=RentCollection;User Id=sa;Password=Baisla1999@" Microsoft.EntityframeworkCore.SqlServer -outputdir Models -f
```


# Database
```sql
DROP DATABASE RentCollection;

CREATE DATABASE RentCollection;

USE RentCollection;

CREATE TABLE Users (
    UserId INT NOT NULL IDENTITY(1, 1),
    FullName VARCHAR(50) NOT NULL,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Contact VARCHAR(15) NOT NULL UNIQUE,
    Role VARCHAR(50) NOT NULL,
    PRIMARY KEY(UserId)
);

CREATE TABLE Rentals (
    RentalId INT NOT NULL IDENTITY(1, 1),
    UserId INT NOT NULL,
    Title VARCHAR(50) NOT NULL,
    Description VARCHAR(200) NOT NULL,
    Amount FLOAT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    PRIMARY KEY(RentalId),
    FOREIGN KEY(UserId) REFERENCES Users(UserID) ON DELETE CASCADE,
    CONSTRAINT Unique_Rental UNIQUE(UserId, Title)
);

CREATE TABLE Tenants (
    TenantId INT NOT NULL IDENTITY(1, 1),
    UserId INT NOT NULL,
    FullName VARCHAR(50) NOT NULL,
    Contact VARCHAR(15) NOT NULL,
    Email VARCHAR(50),
    Password VARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    PRIMARY KEY(TenantId),
    FOREIGN KEY(UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);


CREATE TABLE DocumentType (
    DocumentTypeId INT NOT NULL IDENTITY(1, 1),
    UserId INT NOT NULL,
    Code VARCHAR(50) NOT NULL,
    PRIMARY KEY(DocumentTypeId),
    FOREIGN KEY(UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT Document_Type UNIQUE(UserId, Code)
);

CREATE TABLE Documents (
    DocumentId INT NOT NULL IDENTITY(1, 1),
    TenantId INT NOT NULL,
    DocumentTypeId INT NOT NULL,
    DocumentName VARCHAR(100) NOT NULL,
    PRIMARY KEY(DocumentId),
    FOREIGN KEY(TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE
);
-- FOREIGN KEY(DocumentTypeId) REFERENCES DocumentType(DocumentTypeId) ON DELETE CASCADE


CREATE TABLE Allocation (
    AllocationId INT NOT NULL IDENTITY(1, 1),
    RentalId INT NOT NULL,
    TenantId INT NOT NULL,
    AllocatedOn DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT(0),
    IsDeleted BIT NOT NULL DEFAULT(0),
    PRIMARY KEY (AllocationId),
    FOREIGN KEY(RentalId) REFERENCES Rentals(RentalId) ON DELETE CASCADE
);
-- FOREIGN KEY(TenantId) REFERENCES Tenants(TenantId) ON DELETE CASCADE

CREATE TABLE ElectricityMeterReading (
    MeterReadingId INT NOT NULL IDENTITY(1, 1),
    RentalId INT NOT NULL,
    Units INT NOT NULL,
    TakenOn DATE NOT NULL,
    PRIMARY KEY (MeterReadingId),
    FOREIGN KEY(RentalId) REFERENCES Rentals(RentalId) ON DELETE CASCADE,
);

CREATE TABLE Invoices (
    InvoiceId INT NOT NULL IDENTITY(1, 1),
    AllocationId INT NOT NULL,
    InvoiceDate DATE NOT NULL,
    PRIMARY KEY(InvoiceId),
    FOREIGN KEY(AllocationId) REFERENCES Allocation(AllocationId) ON DELETE CASCADE
);

CREATE TABLE InvoiceItemCategory (
    InvoiceItemCategoryId INT NOT NULL IDENTITY(1, 1),
    UserId INT NOT NULL,
    Code VARCHAR(50) NOT NULL,
    PRIMARY KEY(InvoiceItemCategoryId),
    FOREIGN KEY(UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT User_Invoice_Item_Category UNIQUE(UserId, Code)
);

CREATE TABLE InvoiceItem (
    InvoiceItemId INT NOT NULL IDENTITY(1, 1),
    InvoiceId INT NOT NULL,
    InvoiceItemCategoryId INT NOT NULL,
    Description VARCHAR(100) NOT NULL,
    Amount FLOAT NOT NULL,
    Date DATE NOT NULL,
    PRIMARY KEY(InvoiceItemId),
    FOREIGN KEY(InvoiceId) REFERENCES Invoices(InvoiceId) ON DELETE CASCADE,
);
-- FOREIGN KEY(InvoiceItemCategoryId) REFERENCES InvoiceItemCategory(InvoiceItemCategoryId) ON DELETE CASCADE

CREATE TABLE AutomatedRaisedPayments (
    AutomatedRaisedPaymentId INT NOT NULL IDENTITY(1, 1),
    AllocationId INT NOT NULL,
    InvoiceItemCategoryId INT NOT NULL,
    Description VARCHAR(100) NOT NULL,
    Amount FLOAT NOT NULL,
    PRIMARY KEY(AutomatedRaisedPaymentId),
    FOREIGN KEY(AllocationId) REFERENCES Allocation(AllocationId) ON DELETE CASCADE
);
-- FOREIGN KEY(InvoiceItemCategoryId) REFERENCES InvoiceItemCategory(InvoiceItemCategoryId) ON DELETE CASCADE


CREATE TABLE ModeOfPayment (
    ModeOfPaymentId INT NOT NULL IDENTITY(1, 1),
    Code VARCHAR(50),
    PRIMARY KEY(ModeOfPaymentId)
);

INSERT INTO ModeOfPayment VALUES 
('Cash'),
('Online');

CREATE TABLE Payments (
    PaymentId INT NOT NULL IDENTITY(1, 1),
    InvoiceId INT NOT NULL,
    ModeOfPaymentId INT NOT NULL,
    Description VARCHAR(100),
    Amount FLOAT NOT NULL,
    Date DATE NOT NULL,
    PRIMARY KEY(PaymentId),
    FOREIGN KEY(InvoiceId) REFERENCES Invoices(InvoiceId) ON DELETE CASCADE,
    FOREIGN KEY(ModeOfPaymentId) REFERENCES ModeOfPayment(ModeOfPaymentId) ON DELETE CASCADE
);


CREATE TABLE MonthYearList (
    SerialNo INT NOT NULL IDENTITY(1, 1),
    MonthYear VARCHAR(20),
    PRIMARY KEY(SerialNo)
);
```

### Changes In Database
```sql
ALTER TABLE Rentals ADD CONSTRAINT Unique_Rental UNIQUE(UserId, Title);

ALTER TABLE Tenants ALTER COLUMN Email VARCHAR(50);

ALTER TABLE Tenants ADD CONSTRAINT Unique_Contact UNIQUE(UserId, Contact);

ALTER TABLE Allocation ALTER COLUMN AllocatedOn DATE;

ALTER TABLE Invoices ADD IsDeleted BIT NOT NULL DEFAULT(0);

ALTER TABLE ElectricityMeterReading ADD IsDeleted BIT NOT NULL DEFAULT(0);

ALTER TABLE ElectricityMeterReading ADD GenerateBill BIT NOT NULL DEFAULT(0);

ALTER TABLE ElectricityMeterReading ADD Charges INT NOT NULL;

ALTER TABLE InvoiceItem ALTER COLUMN Description VARCHAR(200) NOT NULL;

ALTER TABLE AutomatedRaisedPayments ADD CONSTRAINT Unique_Automated_Raised_Item UNIQUE(AllocationId, InvoiceItemCategoryId);


```

### Sql Procedures

```sql

CREATE PROCEDURE DeleteInvoiceItemAssociatedWithElectricityBill @MeterReadingId int
AS
DELETE FROM InvoiceItem WHERE Description LIKE CONCAT('%MeterReadingId: ', @MeterReadingId)
GO;


```

### Testing sql
```sql

SELECT * FROM Users;
SELECT * FROM Rentals;
SELECT * FROM Tenants;
SELECT * FROM Allocation;
SELECT * FROM Invoices;
SELECT * FROM InvoiceItem;
SELECT * FROM InvoiceItemCategory;
SELECT * FROM Documents;
SELECT * FROM DocumentType;
SELECT * FROM Payments;
SELECT * FROM ModeOfPayment;

```

### Query Data
```sql

-- Breif Invoice Info
SELECT 
    i.InvoiceId AS InvoiceId, 
    SUM(Amount) as InvoiceTotal,
    i.InvoiceDate AS InvoiceDate,
    (
        SELECT
            p.Amount AS PaymentAmount
        FROM Payments p
        WHERE p.InvoiceId = i.InvoiceId
    ) AS PaymentAmount
FROM InvoiceItem it INNER JOIN
Invoices i ON it.InvoiceId = i.InvoiceId
WHERE i.InvoiceId IN (
    SELECT
        InvoiceId
    FROM Invoices
    WHERE AllocationId = 23
)
GROUP BY i.InvoiceId, i.InvoiceDate;

-- ARP By Allocation
SELECT 
    AutomatedRaisedPaymentId, 
    arp.AllocationId AS AllocationId, 
    arp.Amount AS Amount, 
    Code AS Category, 
    arp.Description AS [Description]
FROM AutomatedRaisedPayments arp 
INNER JOIN 
InvoiceItemCategory itc ON arp.InvoiceItemCategoryId = itc.InvoiceItemCategoryId
INNER JOIN Allocation a
ON arp.AllocationId = a.AllocationId
INNER JOIN 
Rentals r ON r.RentalId = a.RentalId
WHERE a.IsActive = 1 AND r.UserId = 2008 AND arp.AllocationId = 23;

-- ARP By All Allocation
SELECT 
    arp.AllocationId AS AllocationId, 
    SUM(arp.Amount) AS Amount,
    r.Title As Title
FROM AutomatedRaisedPayments arp 
INNER JOIN 
InvoiceItemCategory itc ON arp.InvoiceItemCategoryId = itc.InvoiceItemCategoryId
INNER JOIN Allocation a
ON arp.AllocationId = a.AllocationId
INNER JOIN 
Rentals r ON r.RentalId = a.RentalId
WHERE a.IsActive = 1 AND r.UserId = 2008
GROUP BY arp.AllocationId, r.Title;

-- ARP Total Collection
SELECT 
    SUM(arp.Amount) AS TotalArpCollection
FROM AutomatedRaisedPayments arp
WHERE AllocationId IN (
    SELECT 
        AllocationId 
    FROM Allocation a INNER JOIN 
    Rentals r ON a.RentalId = r.RentalId 
    WHERE a.IsActive = 1 AND r.UserId = 2008
);

-- Current Rent Collection
SELECT 
    SUM(Amount) AS CurrentRentCollection 
FROM Rentals 
WHERE UserId = 2008 AND IsDeleted = 0;

-- ARP By Category
SELECT
    itc.Code AS Category,
    SUM(arp.Amount) AS Amount
FROM AutomatedRaisedPayments arp INNER JOIN
InvoiceItemCategory itc ON arp.InvoiceItemCategoryId = itc.InvoiceItemCategoryId
WHERE AllocationId IN (
    SELECT 
        AllocationId
    FROM Allocation a INNER JOIN 
    Rentals r On a.RentalId = r.RentalId
    WHERE a.IsActive = 1 AND r.UserId = 2008
)
GROUP BY itc.Code;

-- Monthly collection by category
SELECT
    itc_main.Code AS Category,
    'May-2022' AS MonthYear,
    (
        CASE
            WHEN CollectionByMonth.Amount IS NULL THEN 0
            ELSE CollectionByMonth.Amount
        END
    )
    AS Amount
FROM InvoiceItemCategory itc_main
LEFT JOIN
(
SELECT
    itc.Code AS Category,
    SUM(itm.Amount) AS Amount,
    CONCAT(DATENAME(M, itm.Date), '-' ,DATEPART(YEAR, itm.[Date])) AS MonthYear
FROM InvoiceItem itm INNER JOIN 
InvoiceItemCategory itc ON itm.InvoiceItemCategoryId = itc.InvoiceItemCategoryId 
WHERE InvoiceId IN (
    SELECT 
        InvoiceId 
    FROM Invoices i 
    WHERE AllocationId IN (
        SELECT 
            AllocationId
        FROM Allocation a INNER JOIN
        Rentals r ON r.RentalId = a.RentalId
        WHERE a.IsActive = 1 AND r.UserId = 2008
    )
)
AND itc.Code IN (
    SELECT
        Code
    FROM InvoiceItemCategory
    WHERE UserId = 2008
)
AND CONCAT(DATENAME(M, itm.Date), '-' ,DATEPART(YEAR, itm.[Date])) = 'May-2022'
GROUP BY itc.Code, CONCAT(DATENAME(M, itm.Date), '-' ,DATEPART(YEAR, itm.[Date])), CONCAT(DATEPART(MONTH, itm.Date), '-', DATEPART(YEAR, itm.Date))
-- ORDER BY CONCAT(DATEPART(MONTH, itm.Date), '-', DATEPART(YEAR, itm.Date))
) AS CollectionByMonth ON CollectionByMonth.Category = itc_main.Code;

-- Collection by payment mode
SELECT
    mof_main.Code,
    (
        CASE
            WHEN PaymentByMode.Amount IS NULL THEN 0
            ELSE PaymentByMode.Amount
        END
    ) AS PaymentMode
FROM ModeOfPayment mof_main LEFT JOIN
(
SELECT
    mof.Code AS PaymentMode,
    -SUM(p.Amount) AS Amount
FROM Payments p INNER JOIN
ModeOfPayment mof ON p.ModeOfPaymentId = mof.ModeOfPaymentId
WHERE p.InvoiceId IN (
    SELECT 
        InvoiceId 
    FROM Invoices i 
    WHERE AllocationId IN (
        SELECT 
            AllocationId
        FROM Allocation a INNER JOIN
        Rentals r ON r.RentalId = a.RentalId
        WHERE a.IsActive = 1 AND r.UserId = 2008
    )
)
GROUP BY mof.Code
) AS PaymentByMode ON PaymentByMode.PaymentMode = mof_main.Code;


-- Collection by payment mode
SELECT
    mof_main.Code,
    (
        CASE
            WHEN PaymentByMode.Amount IS NULL THEN 0
            ELSE PaymentByMode.Amount
        END
    ) AS PaymentMode,
    'June-2022' AS MonthYear
FROM ModeOfPayment mof_main LEFT JOIN
(
SELECT
    mof.Code AS PaymentMode,
    -SUM(p.Amount) AS Amount,
    CONCAT(DATENAME(M, p.Date), '-' ,DATEPART(YEAR, p.Date)) AS MonthYear
FROM Payments p INNER JOIN
ModeOfPayment mof ON p.ModeOfPaymentId = mof.ModeOfPaymentId
WHERE p.InvoiceId IN (
    SELECT 
        InvoiceId 
    FROM Invoices i 
    WHERE AllocationId IN (
        SELECT 
            AllocationId
        FROM Allocation a INNER JOIN
        Rentals r ON r.RentalId = a.RentalId
        WHERE a.IsActive = 1 AND r.UserId = 2008
    )
)
AND CONCAT(DATENAME(M, p.Date), '-' ,DATEPART(YEAR, p.Date)) = 'June-2022'
GROUP BY mof.Code, CONCAT(DATENAME(M, p.Date), '-' ,DATEPART(YEAR, p.Date))
) AS PaymentByMode ON PaymentByMode.PaymentMode = mof_main.Code;

```

### Generate Month Year List Procedure
```sql
-- Generate Month Year List from UserDefined date to current date
-- Date format: yyyy-mm-01

CREATE PROCEDURE GenerateMonthYearList @date DATE
AS
BEGIN

    TRUNCATE TABLE MonthYearList;

    DECLARE @i INT = 0;
    DECLARE @tempDate DATE = @date;

    DECLARE @months INT = DATEDIFF(MONTH, @tempDate, GETDATE());
    Print @months;
    WHILE @i <= @months
        BEGIN
            INSERT INTO MonthYearList VALUES (CONCAT(DATENAME(M, @tempDate), '-', DATEPART(YEAR, @tempDate)));
            SET @tempDate = DATEADD(MONTH, 1, @tempDate);
            SET @i = @i + 1;
        END;

END;

-- EXEC GenerateMonthYearList '2022-04-01';
```

### Raised Payment By Month
```sql
SELECT
    (
        CASE 
            WHEN RaisedPaymentByMonth.Amount IS NULL THEN 0
            ELSE RaisedPaymentByMonth.Amount
        END
    ) AS Amount,
    myl.MonthYear AS MonthYear
FROM MonthYearList myl LEFT JOIN
(
    SELECT
        SUM(it.Amount) AS Amount,
        CONCAT(DATENAME(M, it.Date), '-' ,DATEPART(YEAR, it.Date)) AS Date
    FROM InvoiceItem it
    WHERE it.InvoiceId IN
    (
        SELECT 
            InvoiceId
        FROM Invoices i
        WHERE i.AllocationId IN
        (
            SELECT
                AllocationId
            FROM Allocation a INNER JOIN
            Rentals r ON a.RentalId = r.RentalId
            WHERE r.UserId = @UserId
        )
        AND i.IsDeleted = 0
    )
    GROUP BY CONCAT(DATENAME(M, it.Date), '-' ,DATEPART(YEAR, it.Date))
) AS RaisedPaymentByMonth ON myl.MonthYear = RaisedPaymentByMonth.Date
ORDER BY myl.SerialNo ASC;
```

