using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TaskConsoleApp
{
    public class Content
    {
        public string Site { get; set; }
        public int Len { get; set; }
    }

    public class Status
    {
        public int threadId { get; set; }
        public DateTime date { get; set; }
    }
    internal class  Program
    {
        private async static Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ////-------------ContinueWith
            var mytask = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((x) =>
            {
                Console.WriteLine(x.Result.Length);
            });//x parametresi alan isimsiz metot tanımladık.googleın datası geldikten sonra continuwith calışacak.googledan dönen string dataya eşit

            ////.ContinueWith(calis);// bu şekilde de başka metot cağırılabilir.


            Console.WriteLine("Arada yapılacak işler");
            await mytask;

            Console.ReadLine();

            ////////----------------whenAll (tüm işleri aynı anda çalıştır)

            Console.WriteLine("Main Thread:" + Thread.CurrentThread.ManagedThreadId);// Ana thread id'si ekrana yazdırılır

           // List<string> urlsList = new List<string>()
            {
                "https://www.google.com",
                "https://www.microsoft.com",
                "https://www.amazon.com",
                "https://www.n11.com",
                "https://www.haberturk.com"

            };

           // List<Task<Content>> taskList = new List<Task<Content>>();

            urlsList.ToList().ForEach(x =>
            {
                taskList.Add(GetContentAsync(x));
            });

            var contents = Task.WhenAll(taskList.ToArray());//await keywordu kullanılırsa arraydeki tüm işler tamamlanınca bu satırdaki işlem bitmiş olur

            Console.WriteLine("whenall metodundan sonra başka işler yaptım");//arada çalışmasını istediğim kodlar

            var data = await contents;//tamamlanmayan iş varsa onun da tamamlanmasını beklicek
            data.ToList().ForEach(x =>
            {
                Console.WriteLine($"{x.Site} boyut:{x.Len}");
            });

            ////-----whenany(ilk biten işin sonucunu döndürür)
            ///

            Console.WriteLine("Main Thread:" + Thread.CurrentThread.ManagedThreadId);// Ana thread id'si ekrana yazdırılır

           // List<string> urlsList = new List<string>()
            {
                "https://www.google.com",
                "https://www.microsoft.com",
                "https://www.amazon.com",
                "https://www.n11.com",
                "https://www.haberturk.com"

            };

           // List<Task<Content>> taskList = new List<Task<Content>>();

            urlsList.ToList().ForEach(x =>
            {
                taskList.Add(GetContentAsync(x));
            });

            var FirstData = await Task.WhenAny(taskList.ToArray());

            Console.WriteLine($"{FirstData.Result.Site}-{FirstData.Result.Len}");//İlk tamamlanan işin sonucu ekrana basıldı


            ////----WaitAll (whenall ile aynı tek farkı ana threadi bloklar (yani ui threadi), kullanılması cok tavsiye edilmiyor.)

            Console.WriteLine("WaitAll metodundan önce");
            bool result = Task.WaitAll(taskList.ToArray(), 300);//zamansız da kullanabilirdik o zaman boolean dönmeyecekti

            Console.WriteLine("3 saniyede geldi mi?" + result);
            Console.WriteLine("WaitAll metodundan sonra");

            Console.WriteLine($"{taskList.First().Result.Site}-{taskList.First().Result.Len}");


            ///----WaitAny (Kullanıldığı threadi bloklar, hernahgi biri tamalandığı zaman tamamlanan taskin index numarasını döner, overaloadunda milisniye de verilebilir.Bu süre zarfında döndü mü döndüyse dönennin indexini döner)
            ///

             List<string> urlsList = new List<string>()
            {
                "https://www.google.com",
                "https://www.microsoft.com",
                "https://www.amazon.com",
                "https://www.n11.com",
                "https://www.haberturk.com"

            };

             List<Task<Content>> taskList = new List<Task<Content>>();

            urlsList.ToList().ForEach(x =>
            {
                taskList.Add(GetContentAsync(x));
            });

            var firstTaskIndex = Task.WaitAny(taskList.ToArray());
            Console.WriteLine($"{taskList[firstTaskIndex].Result.Site}-{taskList[firstTaskIndex].Result.Len}");

            ////-----Delay ()
            var content = await Task.WhenAll(taskList.ToArray());

            content.ToList().ForEach(x =>
            Task.Delay(3000);
                Console.WriteLine(x.Site)
            );
        }

        public static void calis(Task<string> data)
        { 
            //100 satırlık kod
        }

        public static async Task<Content> GetContentAsync(string url)
        {
            Content c = new Content();
            var data = await new HttpClient().GetStringAsync(url);

            await Task.Delay(5000);//(her thread aşağı aynı anda girecegi için 4 metot da çağırılsa toplam bekleme 5 saniye olacak. Asenkron olarak düşün gecikme sağlar bloklamaz

            //Thread.Sleep() güncel threadi bloklar senkron olarak düşünebiliriz
            c.Site = url;
            c.Len = data.Length;
            Console.WriteLine("GetContentAsync thread:" + Thread.CurrentThread.ManagedThreadId);//çalışan threadin id'si alındı

            return c;
        }


        //private async static Task Main(string[] args)
        //{
        //    var myTask = Task.Factory.StartNew((Obj) =>
        //    {
        //        Console.WriteLine("myTask çalıştı");
        //        var status = Obj as Status;//objeyi Statuse ceviremezse null döner. Ek olarak is keywordu cevirebilirse true degilse false döner.
        //        status.threadId = Thread.CurrentThread.ManagedThreadId;
        //    }, new Status() { date = DateTime.Now});

        //    await myTask;

        //    Status s = myTask.AsyncState as Status;

        //    Console.WriteLine(s.date);
        //    Console.WriteLine(s.threadId);
        //    Console.ReadLine();
        //}
    }

    internal class Program2
    {
        public static string CacheData { get; set; }
        private async static Task Main(string[] args)
        {
            CacheData = await GetDataAsync();
            Console.WriteLine(CacheData);
            Console.ReadLine();
        }

        public static Task<string> GetDataAsync()
        {
            if (String.IsNullOrWhiteSpace(CacheData))
            {
                return File.ReadAllTextAsync("dosya.txt");
            }

            return Task.FromResult<string>(CacheData);//Generic yapmasak da olurdu. Tip güvenliği için generic yaptık
        }
    }
}
