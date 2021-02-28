using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQApp
{
    class Program
    {
        private static bool IsControl(string s)
        {
            try
            {
                return s[2] == 'a';
            }
            catch (Exception)
            {
                Console.WriteLine("Dizi sınırları aşıldı");

                return false;
            }
        }
        static void Main(string[] args)
        {
            var array = Enumerable.Range(1, 500).ToList();

            var newArray = array.AsParallel().Where(Islem);

            //newArray.ToList().ForEach()..//tek threadli  çalışır
            newArray.ForAll(x =>
            {
                Thread.Sleep(500);
                Console.WriteLine(x);
            });//foreachden farkı yok

            //newArray.ToList().ForEach(x =>
            //{
            //    Thread.Sleep(500);
            //    Console.WriteLine(x);
            //});

            //WithDegreeOfParallelism - kaç işlemcide çalışmasını belirlemek için
             newArray = array.AsParallel().WithDegreeOfParallelism(2).Where(Islem);//2 işlemcide çalış dedik

            //WithExecuteMode - Yazılan metodun % 100 paralel çalışmasını sağlamak için, plinqya karar için bırakmadık
            newArray = array.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Where(Islem);//%100 paralel çalışır. Enumda default secseydik kararı plinq'ya bırakmış olacaktık

            //Asordered
            newArray = array.AsParallel().AsOrdered().Where(Islem);//gelen listedeki sırayı koruduk

            //Exception handle//bir hata bile fırlatılsa işlem durur.

            List<string> content = new List<string>();
            var products = content.Take(100).ToArray();
            products[3] = "##";

            var query = products.AsParallel().Where(IsControl);// hata aldığında işleme devam etsin ve durmasın diye metot içine alıp onu kullandık.

            try
            {
                query.ForAll(x =>
                {
                    Console.WriteLine($"{x}");
                });
            }
            catch (AggregateException ex)//Birden fazla hatayı tutup yakalamk için Aggregate exc. kullanılabilir.
            {
                ex.InnerExceptions.ToList().ForEach(x =>
                {
                    if (x is IndexOutOfRangeException)
                    {
                        Console.WriteLine($"Hata:array sınırları dışına cıkıldı");
                    }

                    //Console.WriteLine($"Hata:{ex.Message}");
                });
            }
            
        }

        private static bool Islem(int x)
    => x % 2 == 0;
    }
}
