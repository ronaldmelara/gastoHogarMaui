
using System;
using gastos_hogar.DataAccess;
using gastos_hogar.Dtos;

namespace gastos_hogar.Business
{
	public class GastoBO
	{
		GastosDa gastoBO;
		public GastoBO()
		{
			gastoBO = new GastosDa();
		}

        public ResultDto PutEgreso(int idCategoria, float monto, DateTime date, int idSaldo)
		{
			return gastoBO.PutEgreso(idCategoria, monto, date, idSaldo);
		}

        public SaldoDto GetSaldoActivo()
        {
			return gastoBO.GetSaldoActivo();

        }

        public GraficoTortaDto GetGrafico(int idSaldo)
		{
			return gastoBO.GetGrafico(idSaldo);
		}
    }
}

