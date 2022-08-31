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
    FOREIGN KEY(InvoiceId) REFERENCES Invoices(InvoiceId) ON DELETE CASCADE,
    FOREIGN KEY(ModeOfPaymentId) REFERENCES ModeOfPayment(ModeOfPaymentId) ON DELETE CASCADE
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

```

### Sql Procedures

```

CREATE PROCEDURE DeleteInvoiceItemAssociatedWithElectricityBill @MeterReadingId int
AS
DELETE FROM InvoiceItem WHERE Description LIKE CONCAT('%MeterReadingId: ', @MeterReadingId)
GO;


```