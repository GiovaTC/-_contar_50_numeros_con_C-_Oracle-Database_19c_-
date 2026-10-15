using System;
using Oracle.ManagedDataAccess.Client;

namespace Contar50NumerosOracle
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // CONFIGURACION de conexion a ORACLE 19c .
            string cadenaConexion = "User Id=system;" + 
                                    "Password=Tapiero123;" + 
                                    "Data Source=localhost:1521/orcl;";

            // Identificador unico para esta ejecucion .
            string loteId = Guid.NewGuid().ToString();

            int totalProcesado = 0;

            Console.WriteLine("==================================");
            Console.WriteLine("   CONTADOR DE 50 NUMEROS");
            Console.WriteLine("   C# + ORACLE DATABASE 19c");
            Console.WriteLine("==================================");
            Console.WriteLine();

            try
            {
                using (OracleConnection conexion =
                    new OracleConnection(cadenaConexion))
                {
                    conexion.Open();

                    Console.WriteLine("Conexion a Oracle exitosa.");
                    Console.WriteLine();

                    // INSERTAR los 50 numeros en una transaccion .
                    using (OracleTransaction transaccion =
                        conexion.BeginTransaction())
                    {
                        try
                        {
                            string sqlInsertar = @"
                                INSERT INTO NUMEROS_CONTADOS
                                    (LOTE_ID, VALOR)
                                VALUES 
                                    (:loteId, :valor)";

                            using (OracleCommand comando =
                                new OracleCommand(sqlInsertar, conexion))

                            {
                                comando.Transaction = transaccion;

                                comando.Parameters.Add(
                                    "loteId",
                                    OracleDbType.Varchar2,
                                    36).Value = loteId;

                                comando.Parameters.Add(
                                    "valor",
                                    OracleDbType.Int32);

                                for (int numero = 1; numero <= 50; numero++)
                                {
                                    comando.Parameters["valor"].Value = numero;

                                    comando.ExecuteNonQuery();
                                }
                            }

                            transaccion.Commit();
                        }
                        catch
                        {
                            transaccion.Rollback();
                            throw;
                        }
                    }

                    Console.WriteLine(
                        "Numeros guardados correctamente en oracle!");
                    Console.WriteLine();

                    // consultar unicamente lod numeros de esta ejecucion.
                    string sqlConsultar = @"
                        SELECT VALOR
                        FROM NUMEROS_CONTADOS
                        WHERE LOTE_ID = :loteId
                        ORDER BY VALOR";

                    using (OracleCommand comando =
                        new OracleCommand(sqlConsultar, conexion))
                    {
                        comando.Parameters.Add(
                            "loteId",
                            OracleDbType.Varchar2,
                            36).Value = loteId;

                        using (OracleDataReader lector =
                               comando.ExecuteReader())
                        {
                            Console.WriteLine(
                                "Numeros consultados desde Oracle:");
                            Console.WriteLine();

                            while (lector.Read())
                            {
                                int numero = lector.GetInt32(0);

                                Console.WriteLine($"Numero: {numero:D2}");
                                totalProcesado++;
                            }
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("==================================");
                Console.WriteLine($"TOTAL DE NUMEROS: {totalProcesado}");
                Console.WriteLine("PROCESO FINALIZADO CORRECTAMENTE");
                Console.WriteLine("==================================");
            }
            catch (OracleException ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error en el programa:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("presiona una TECLA para salir!");
            Console.ReadKey();
        }
    }
}
