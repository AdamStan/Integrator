CREATE TABLE [dbo].[Customers] (
    [Id]        INT          NOT NULL,
    [cust_name] VARCHAR (32) NULL,
    [surname]   VARCHAR (64) NULL,
    [city]      VARCHAR (32) NULL,
    [street]    VARCHAR (32) NULL,
    [number]    CHAR (5)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Departments] (
    [dept_Id]   INT          NOT NULL,
    [dept_name] VARCHAR (32) NOT NULL,
    [city]      VARCHAR (32) NULL,
    PRIMARY KEY CLUSTERED ([dept_Id] ASC)
);
CREATE TABLE [dbo].[Employees] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [emp_name]      VARCHAR (32) NOT NULL,
    [emp_surname]   VARCHAR (64) NOT NULL,
    [emp_birthdate] DATE         NULL,
    [salary]        FLOAT (53)   NOT NULL,
    [bonus]         FLOAT (53)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Orders] (
    [Id]          INT        NOT NULL,
    [product_id]  INT        NULL,
    [customer_id] INT        NULL,
    [date_order]  DATE       NOT NULL,
    [amount]      FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Products] (
    [Id]           INT          NOT NULL,
    [product_name] VARCHAR (32) NOT NULL,
    [price]        FLOAT (53)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);