using System;
using gastos_hogar.Dtos;
using Npgsql;

namespace gastos_hogar.DataAccess
{
	public class CategoriasDa
	{
        string conn;

        public CategoriasDa()
        {
            conn = "Host=ep-dry-wood-a5j6mty6.us-east-2.aws.neon.fl0.io;Port=5432;Database=gastos;Username=fl0user;Password=eqS9Lbh0pTiU";


        }

        public List<CategoriaGastoDto> GetCategorias()
        {
            List<CategoriaGastoDto> lst = new List<CategoriaGastoDto>();
            var connection = new NpgsqlConnection(conn);
            try
            {

                var sql = "SELECT id_categoria, categoria FROM categorias_gastos WHERE id_categoria_padre IS NULL ORDER BY id_categoria";
                connection.Open();
                using var cmd = new NpgsqlCommand(sql, connection);

                using var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    CategoriaGastoDto row = new CategoriaGastoDto();
                    // Acceder a las columnas de la fila actual
                    row.IdCtaegoria = reader.GetInt32(0); // Ejemplo de obtener un valor de la primera columna como cadena
                    row.Categoria = reader.GetString(1); // Ejemplo de obtener un valor de la segunda columna como entero
                    lst.Add(row);
                }
            }
            catch (Exception ex)
            {
                var a = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return lst;
        }

        public List<CategoriaGastoDto> GetSubCategorias(int idCategoria)
        {
            List<CategoriaGastoDto> lst = new List<CategoriaGastoDto>();
            var connection = new NpgsqlConnection(conn);
            try
            {

                var sql = "SELECT id_categoria, categoria FROM categorias_gastos WHERE id_categoria_padre = " + idCategoria + " ORDER BY id_categoria";
                connection.Open();
                using var cmd = new NpgsqlCommand(sql, connection);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CategoriaGastoDto row = new CategoriaGastoDto();
                    // Acceder a las columnas de la fila actual
                    row.IdCtaegoria = reader.GetInt32(0); // Ejemplo de obtener un valor de la primera columna como cadena
                    row.Categoria = reader.GetString(1); // Ejemplo de obtener un valor de la segunda columna como entero
                    lst.Add(row);
                }
            }
            catch (Exception ex)
            {
                var a = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return lst;
        }
    }
}

