USE [Func]
GO

/****** Object:  StoredProcedure [dbo].[Prc_JsonToTable_Test]    Script Date: 2022/12/12 17:17:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Prc_JsonToTable_Test]
AS
BEGIN
    DECLARE @json NVARCHAR(MAX),@columns NVARCHAR(MAX)
			,@funRelust INT --1.≥…π¶ 2. ß∞‹
			,@funMsg VARCHAR(200)
	SET @columns='key,value'
	SET @json=' [
			  {
				"key": "0001",
				"value": "  ≤‚ ‘1  "
			  },
			  {
				"key": "0002",
				"value": "≤‚ ‘2"
			  },
			  {
				"key": "0003",
				"value": "≤‚ ‘3"
			  }
			]'
	EXEC	dbo.Prc_JsonToTable @json,@columns,@funRelust OUTPUT ,@funMsg OUTPUT 
	SELECT	@funRelust AS relust,@funMsg AS msg 
END
GO


