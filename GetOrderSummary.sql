USE [Northwind]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pr_GetOrderSummary](
														@EMPLOYEEID int = NULL,
														@CUSTOMERID varchar(60) = NULL,
														@STARTDATE DATE,
														@ENDDATE DATE
													)
AS
BEGIN
	BEGIN TRY
		select  [TitleOfCourtesy] + ' ' + [FirstName] + ' ' + [LastName] as EmployeeFullName,
			   shp.CompanyName ShipperCompanyName,
			   cus.CompanyName CustomerCompanyName,
			   count(odd.OrderID) number_of_orders,
			   ord.OrderDate Date,
			   ord.Freight * count(odd.productid) Total_Freight,
			   count(odd.productid) NumberOfDifferentProducts,
			   sum(UnitPrice) * sum(Quantity) Total_Order_Value	   
		from Orders ord
		left join Employees emp on ord.EmployeeID = emp.EmployeeID
		left join Customers cus on ord.CustomerID = cus.CustomerID
		left join Shippers shp on shp.ShipperID = ord.ShipVia
		left join [order details] odd on odd.OrderId = ord.OrderId
		where OrderDate between  @STARTDATE and @ENDDATE
		AND ord.EmployeeID = COALESCE(NULLIF(@EMPLOYEEID, ''), ord.EmployeeID)
		AND ord.CustomerID = COALESCE(NULLIF(@CUSTOMERID, ''), ord.CustomerID)
		group by TitleOfCourtesy,FirstName,LastName, Freight,shp.CompanyName,
			   cus.CompanyName,
			   ord.CustomerID,
			   ord.OrderID,
			   OrderDate ;
	END TRY
	BEGIN CATCH
		SELECT
    ERROR_NUMBER() AS ErrorNumber,
    ERROR_STATE() AS ErrorState,
    ERROR_SEVERITY() AS ErrorSeverity,
    ERROR_PROCEDURE() AS ErrorProcedure,
    ERROR_LINE() AS ErrorLine,
    ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
