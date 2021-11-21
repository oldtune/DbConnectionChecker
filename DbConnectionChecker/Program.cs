using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        /// <summary>
        /// ./DbConnectionChecker.exe "connectionstring here"
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide connection string");
                return;
            }

            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            Console.WriteLine($"Connecting to {args[0]}");
            Connect(args[0], cts.Token).Wait();
        }

        public static async Task Connect(string connectionString, CancellationToken cancellationToken)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync(cancellationToken);
            }
            catch(OperationCanceledException _)
            {
                Console.WriteLine("Timed out after 10s");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}