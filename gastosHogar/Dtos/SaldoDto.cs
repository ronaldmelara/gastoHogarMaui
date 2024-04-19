using System;
namespace gastos_hogar.Dtos
{
	public class SaldoDto
	{
		public SaldoDto()
		{
		}

		public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
		public float Capital { get; set; }
		public float Saldo { get; set; }
		public int IdSaldo { get; set; }
		public int Activo { get; set; }
    }
}

