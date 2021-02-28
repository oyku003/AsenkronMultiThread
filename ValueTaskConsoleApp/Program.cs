using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueTaskConsoleApp
{
    class Program
    {
        public static int cacheData { get; set; } = 150;
        private async static Task Main(string[] args)
        {
            var myTask = await GetData();
            
        }

        public static ValueTask<int> GetData()
        {
            return new ValueTask<int>(cacheData);//kısa sürede döneceğini bildiğiiz datalar için kullanabiliriz.
        }
    }
}
