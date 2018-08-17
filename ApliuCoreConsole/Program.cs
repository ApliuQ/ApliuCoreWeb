using System;

namespace ApliuCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Apliu Core Console Hello World!");
            try
            {
                //ApliuCoreConsole.RunFuction.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Run：" + ex.Message);
            }
            Console.ReadKey();
        }
    }
}
