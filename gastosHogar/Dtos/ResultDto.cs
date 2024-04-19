using System;
namespace gastos_hogar.Dtos
{
	public class ResultDto
	{
		public ResultDto()
		{
		}

		public int CodeStatus { get; set; }
		public string Message { get; set; }
		public Exception ex { get; set; }
	}
}

