USE [DataBase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Говорков Максим
-- Create date: 
-- Description:	Получить табличные данные карты
-- =============================================
CREATE PROCEDURE [dbo].[Map.Get] 

AS BEGIN SET NOCOUNT ON;

SELECT Id
FROM [dbo].[Map]

END
