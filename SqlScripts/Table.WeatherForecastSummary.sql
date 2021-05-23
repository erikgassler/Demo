
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WeatherForecastSummary](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[Summary] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
