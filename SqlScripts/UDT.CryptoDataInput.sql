
-- Create the data type
CREATE TYPE dbo.CryptoDataInput AS TABLE 
(
	[Id] [bigint] PRIMARY KEY NOT NULL,
	[Unix] [bigint] NOT NULL,
	[Date] [datetime2](4) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Open] [decimal](18, 0) NOT NULL,
	[High] [decimal](18, 0) NOT NULL,
	[Low] [decimal](18, 0) NOT NULL,
	[Close] [decimal](18, 0) NOT NULL,
	[VolumeBTC] [decimal](18, 0) NOT NULL,
	[VolumeUSDT] [decimal](18, 0) NOT NULL
)
