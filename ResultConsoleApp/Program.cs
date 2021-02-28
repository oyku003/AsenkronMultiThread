using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ResultConsoleApp
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var task = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((data) =>
            {
                Console.WriteLine(data.Result);
            });//Aysnc metot çalıştıktan sonra ContinueWith ile dataya ulaşılabilinir.
        }

        public static string GetData()
        {
            var task = new HttpClient().GetStringAsync("https://www.google.com");
            return task.Result;// sonuc gelene kadar ilgili thread bloklanır.
        }

        public async static Task<string> GetDataAsync()
        {
            var task = new HttpClient().GetStringAsync("https://www.google.com");
            await task;

            return task.Result;//datanın geldiğinden emin olduktan sonra alınabilir.
        }
       
    }
}
