SET XSD_EXE="C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd.exe"

SET TARGET_DIR="..\Sage.Integration.Northwind.Feeds"
SET CRM_XSD="..\Sage.Integration.Northwind.Adapter\crmErp.xsd"
SET COMMON_XSD="..\Sage.Integration.Northwind.Adapter\common.xsd"

%XSD_EXE% %CRM_XSD% %COMMON_XSD% /c /n:Sage.Integration.Northwind.Feeds /o:%TARGET_DIR%

cd %TARGET_DIR%
del Payloads.cs
rename common.cs Payloads.cs
pause