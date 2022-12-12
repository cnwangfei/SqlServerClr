USE [Func]
GO

/****** Object:  StoredProcedure [dbo].[Init]    Script Date: 2022/12/12 17:16:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Version:		22.09.02.001
-- Author:		王飞
-- Description:	初始化Clr函数
-- =============================================
CREATE PROCEDURE [dbo].[Init]
AS
BEGIN
	SELECT '手动执行'
/*
-- =============================================
-- 初始化
-- =============================================
EXEC sp_configure 'clr enabled' , 1;
GO 
--安装CLR功能
RECONFIGURE;

--打开数据库权限
ALTER DATABASE Func SET TRUSTWORTHY ON;
GO 
--重置数据库所有者，若需要
sp_changedbowner 'sa' , true; 
GO 



-- =============================================
-- 重置Json
-- =============================================
DROP PROCEDURE Prc_JsonToTable;
DROP FUNCTION Func_QueryJsonStrWithJsonPath;

DROP ASSEMBLY SqlServerClr;
DROP ASSEMBLY [Newtonsoft.Json];
--创建CLR程序集


CREATE ASSEMBLY [Newtonsoft.Json]
FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Newtonsoft.Json.dll' 
WITH PERMISSION_SET=UNSAFE;
GO 

CREATE ASSEMBLY SqlServerClr
FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\SqlServerClr.dll' 
WITH PERMISSION_SET=UNSAFE;
GO 


--		解析Json
CREATE PROCEDURE Prc_JsonToTable
(
  @inJsonStr AS NVARCHAR(MAX) ,
  @inColumnsStr AS NVARCHAR(MAX), --列名，用逗号拼接：id,pzt_id,pzt_name
  @outResult BIT OUTPUT ,
  @outRsg NVARCHAR(MAX) OUTPUT
)
AS EXTERNAL NAME
	[SqlServerClr].[StoredProcedures].[JsonToTable];
GO 

--		使用Json路径查询Json Str
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


