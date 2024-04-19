using System;
namespace gastos_hogar.Dtos
{
	public class GraficoTortaDto
	{
		public GraficoTortaDto()
		{
		}

		public string Categoria { get; set; }
		public int Registros { get; set; }
		public int Monto { get; set; }
	}
}

