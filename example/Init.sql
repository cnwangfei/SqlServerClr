USE [Func]
GO

/****** Object:  StoredProcedure [dbo].[Init]    Script Date: 2022/12/12 17:16:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Version:		22.09.02.001
-- Author:		����
-- Description:	��ʼ��Clr����
-- =============================================
CREATE PROCEDURE [dbo].[Init]
AS
BEGIN
	SELECT '�ֶ�ִ��'
/*
-- =============================================
-- ��ʼ��
-- =============================================
EXEC sp_configure 'clr enabled' , 1;
GO 
--��װCLR����
RECONFIGURE;

--�����ݿ�Ȩ��
ALTER DATABASE Func SET TRUSTWORTHY ON;
GO 
--�������ݿ������ߣ�����Ҫ
sp_changedbowner 'sa' , true; 
GO 



-- =============================================
-- ����Json
-- =============================================
DROP PROCEDURE Prc_JsonToTable;
DROP FUNCTION Func_QueryJsonStrWithJsonPath;

DROP ASSEMBLY SqlServerClr;
DROP ASSEMBLY [Newtonsoft.Json];
--����CLR����


CREATE ASSEMBLY [Newtonsoft.Json]
FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Newtonsoft.Json.dll' 
WITH PERMISSION_SET=UNSAFE;
GO 

CREATE ASSEMBLY SqlServerClr
FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\SqlServerClr.dll' 
WITH PERMISSION_SET=UNSAFE;
GO 


--		����Json
CREATE PROCEDURE Prc_JsonToTable
(
  @inJsonStr AS NVARCHAR(MAX) ,
  @inColumnsStr AS NVARCHAR(MAX), --�������ö���ƴ�ӣ�id,pzt_id,pzt_name
  @outResult BIT OUTPUT ,
  @outRsg NVARCHAR(MAX) OUTPUT
)
AS EXTERNAL NAME
	[SqlServerClr].[StoredProcedures].[JsonToTable];
GO 

--		ʹ��Json·����ѯJson Str
CREATE FUNCTION Func_QueryJsonStrWithJsonPath
(
  @json [NVARCHAR](MAX) ,
  @path [NVARCHAR](1000)
)
RETURNS [NVARCHAR](MAX)
	WITH EXECUTE AS CALLER
AS EXTERNAL NAME
	[SqlServerClr].[UserDefinedFunctions].[QueryJsonStrWithJsonPath];
GO

*/
END
GO


