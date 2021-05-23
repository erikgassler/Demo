
/****** Object:  Table [dbo].[CryptoData]    Script Date: 5/22/2021 7:47:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CryptoData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Unix] [bigint] NOT NULL,
	[Date] [datetime2](4) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Open] [decimal](18, 0) NOT NULL,
	[High] [decimal](18, 0) NOT NULL,
	[Low] [decimal](18, 0) NOT NULL,
	[Close] [decimal](18, 0) NOT NULL,
	[VolumeBTC] [decimal](18, 0) NOT NULL,
	[VolumeUSDT] [decimal](18, 0) NOT NULL
) ON [PRIMARY]
GO
