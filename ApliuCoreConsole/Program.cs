using System;

namespace ApliuCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Apliu Core Console Hello World!");
            Console.WriteLine("------------------------------------------------");
            try
            {
                ApliuCoreConsole.RunFuction.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR：" + ex.Message);
            }
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Apliu Core Console End!");
            Console.ReadKey();
        }
    }
}
