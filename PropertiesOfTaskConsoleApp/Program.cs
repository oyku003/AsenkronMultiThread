using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesOfTaskConsoleApp
{
    class Program
    {
        private async static Task Main(string[] args)
        {

            Task myTask = Task.Run(() =>
            {
                throw new ArgumentException("hata");
            });

            await myTask;

            Console.WriteLine("işlem bitti");
        }
    }
}
