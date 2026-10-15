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

        }
    }
}
