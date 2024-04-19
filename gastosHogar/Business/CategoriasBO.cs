using System;
using gastos_hogar.DataAccess;
using gastos_hogar.Dtos;
using Npgsql;

namespace gastos_hogar.Business
{
	public class CategoriasBO
	{
		CategoriasDa pCategoriasDa;
		public CategoriasBO()
		{
			pCategoriasDa = new CategoriasDa();
		}

        public List<CategoriaGastoDto> GetCategorias()
        {
			return pCategoriasDa.GetCategorias();
        }

        public List<CategoriaGastoDto> GetSubCategorias(int idCategoria)
        {
            return pCategoriasDa.GetSubCategorias(idCategoria);
        }
    }
}

