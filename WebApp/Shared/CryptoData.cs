using System;

namespace WebApp.Shared
{
	public class CryptoData
	{
		public long Id { get; set; }
		public long Unix { get; set; }
		public DateTime Date { get;set;}
		public string Symbol { get; set; }
		public decimal Open { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Close { get; set; }
		public decimal VolumeBTC { get; set; }
		public decimal VolumeUSDT { get; set; }
	}
}
