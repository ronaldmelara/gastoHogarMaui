using System;
using Npgsql;
using gastos_hogar.Dtos;

namespace gastos_hogar.DataAccess
{
	public class GastosDa
	{
		string conn;
		
		public GastosDa()
		{
			conn = "Host=ep-dry-wood-a5j6mty6.us-east-2.aws.neon.fl0.io;Port=5432;Database=gastos;Username=fl0user;Password=eqS9Lbh0pTiU";

            
        }

		public SaldoDto GetSaldoActivo()
		{
            SaldoDto result = new SaldoDto();
            var connection = new NpgsqlConnection(conn);
            try
            {
                
                var sql = "SELECT id_saldo, capital, saldo FROM capital_saldos WHERE activo=1";
                connection.Open();
                using var cmd = new NpgsqlCommand(sql, connection);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Acceder a las columnas de la fila actual
                    result.IdSaldo = reader.GetInt32(0); // Ejemplo de obtener un valor de la primera columna como cadena
                    result.Capital = reader.GetInt32(1);
                    result.Saldo = reader.GetInt32(2);
                    //var column2Value = reader.GetFloat(1); // Ejemplo de obtener un valor de la segunda columna como entero

                    // Realizar el procesamiento deseado con los valores de las columnas
                    //Console.WriteLine($"Valor de columna 1: {column1Value}, Valor de columna 2: {column2Value}");
                }
            }
            catch(Exception ex)
            {
                var a = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return result;
            
        }

        public ResultDto PutEgreso(int idCategoria, float monto, DateTime date, int idSaldo)
        {
            var connection = new NpgsqlConnection(conn);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            ResultDto result = new ResultDto();
            try
            {

                var sql = "INSERT INTO gastos (id_categoria, monto, fecha, id_saldo) VALUES (@idCategoria, @monto,@fecha,@idSaldo)";
                
                using var cmd = new NpgsqlCommand(sql, connection, transaction);
                cmd.Parameters.Add(new NpgsqlParameter("@idCategoria", NpgsqlTypes.NpgsqlDbType.Integer) {  Value = idCategoria });
                cmd.Parameters.Add(new NpgsqlParameter("@monto", NpgsqlTypes.NpgsqlDbType.Integer) { Value = monto });
                cmd.Parameters.Add(new NpgsqlParameter("@fecha", NpgsqlTypes.NpgsqlDbType.Date) { Value = date });
                cmd.Parameters.Add(new NpgsqlParameter("@idSaldo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = idSaldo });
                cmd.ExecuteNonQuery();

                sql = "UPDATE capital_saldos SET saldo=(saldo-@monto) where id_saldo=@idSaldo";
                using var cmdUp = new NpgsqlCommand(sql, connection, transaction);
                cmdUp.Parameters.Add(new NpgsqlParameter("@idSaldo", NpgsqlTypes.NpgsqlDbType.Integer) { Value = idSaldo });
                cmdUp.Parameters.Add(new NpgsqlParameter("@monto", NpgsqlTypes.NpgsqlDbType.Integer) { Value = monto });
                cmdUp.ExecuteNonQuery();

                transaction.Commit();

                result.CodeStatus = 1;
                result.Message = "Egreso guardado!!";
            }
            catch (Exception ex)
            {
                result.CodeStatus = 0;
                result.Message = "No pudo ser guardado. Inténte más tarde.";
                result.ex = ex;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return result;

        }

        public GraficoTortaDto GetGrafico(int idSaldo)
        {
            GraficoTortaDto grf = new GraficoTortaDto();

            try
            {
                var connection = new NpgsqlConnection(conn);
                connection.Open();
                var sql = "select distinct cg.categoria,\n" +
                    "(select count(1) from gastos g where g.id_categoria = cg.id_categoria and g.id_saldo = g1.id_saldo) as registros," +
                    "\n(select sum(monto) from gastos g where g.id_categoria = cg.id_categoria and g.id_saldo = g1.id_saldo) as monto\n" +
                    "from categorias_gastos cg  " +
                    "\ninner join gastos g1 on g1.id_categoria = cg.id_categoria " +
                    "\ninner join capital_saldos cs on cs.id_saldo = g1.id_saldo\n" +
                    "where cs.activo = 1 AND cg.id_categoria_padre is null and cs.id_saldo=@idSaldo\n" +
                    "and cg.id_categoria not in (select DISTINCT cg2.id_categoria_padre from categorias_gastos cg2 where cg2.id_categoria_padre  is not null)\n" +
                    "group by cg.id_categoria,cg.categoria ,g1.id_saldo\nu" +
                    "nion \n" +
                    "select   catPadre as categoria, sum(registros) as registros, sum(monto) as monto from(\n" +
                    "select distinct  cg.id_categoria_padre , cg.id_categoria, cg.categoria,\n" +
                    "(select count(1) from gastos g where g.id_categoria = cg.id_categoria and g.id_saldo = g1.id_saldo) as registros,\n" +
                    "(select sum(monto) from gastos g where g.id_categoria = cg.id_categoria and g.id_saldo = g1.id_saldo) as monto,\n" +
                    "(select cg2.categoria from categorias_gastos cg2 where cg2.id_categoria =cg.id_categoria_padre) as catPadre\n" +
                    "from categorias_gastos cg  \n" +
                    "inner join gastos g1 on g1.id_categoria = cg.id_categoria \n" +
                    "inner join capital_saldos cs on cs.id_saldo = g1.id_saldo\n" +
                    "where cs.activo = 1 AND cg.id_categoria_padre is not null and cs.id_saldo=@idSaldo\n" +
                    "group by cg.id_categoria_padre,cg.id_categoria,cg.categoria ,g1.id_saldo) as hijos\n" +
                    "group by catPadre";
                using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.Add(new NpgsqlParameter("", NpgsqlTypes.NpgsqlDbType.Integer, idSaldo));

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Acceder a las columnas de la fila actual
                    grf.Categoria = reader.GetString(0); // Ejemplo de obtener un valor de la primera columna como cadena
                    grf.Registros = reader.GetInt32(1);
                    grf.Monto = reader.GetInt32(2);
                    //var column2Value = reader.GetFloat(1); // Ejemplo de obtener un valor de la segunda columna como entero

                    // Realizar el procesamiento deseado con los valores de las columnas
                    //Console.WriteLine($"Valor de columna 1: {column1Value}, Valor de columna 2: {column2Value}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return grf;
        }
    }
}

