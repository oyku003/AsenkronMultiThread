using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ForeachParallelApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Stopwatch a = new Stopwatch();
            //a.Start();
            //string picturesPath = @"C:\Users\oyku.bilen\Pictures\Feedback\{3EF1E687-2D5E-4F64-AD7E-08DCA621EEF3}\";

            //var files = Directory.GetFiles(picturesPath);

            ////multithread
            //Parallel.ForEach(files, (item) =>
            //{
            //    Console.WriteLine("Thread"+ Thread.CurrentThread.ManagedThreadId);
            //    System.Drawing.Image img = new Bitmap(item);
            //    var thumbnail = img.GetThumbnailImage(50, 50, () => false, IntPtr.Zero);

            //    thumbnail.Save(Path.Combine(picturesPath, "thumbnail", Path.GetFileName(item)));

            //});
            //a.Stop();
            //Console.WriteLine("işlem bitti"+ a.ElapsedMilliseconds);

            //a.Reset();
            //a.Start();

            ////tek thread
            //files.ToList().ForEach(item =>
            //{
            //    Console.WriteLine("Thread" + Thread.CurrentThread.ManagedThreadId);
            //    System.Drawing.Image img = new Bitmap(item);
            //    var thumbnail = img.GetThumbnailImage(50, 50, () => false, IntPtr.Zero);

            //    thumbnail.Save(Path.Combine(picturesPath, "thumbnail", Path.GetFileName(item)));

            //});

            //a.Stop();

            //Console.WriteLine("işlem bitti" + a.ElapsedMilliseconds);

            //ParallelForeach-2////////

            //long FilesByte = 0;

            //Stopwatch a = new Stopwatch();
            //a.Start();

            //string picturesPath = @"C:\Users\oyku.bilen\Pictures\Feedback\{3EF1E687-2D5E-4F64-AD7E-08DCA621EEF3}\";

            //var files = Directory.GetFiles(picturesPath);

            ////multithread
            //Parallel.ForEach(files, (item) =>
            //{
            //    Console.WriteLine("Thread" + Thread.CurrentThread.ManagedThreadId);

            //    FileInfo f = new FileInfo(item);

            //    Interlocked.Add(ref FilesByte, f.Length);//herhangi bir threadin bu değer eklenene kadar FilesByte'a erişmesini engeller.

            //    //Interlocked.Exchange(ref FilesByte, 300);//yukardakine alternatif
            //});

            //Console.WriteLine("toplam boyut:" + FilesByte.ToString());


            //Race condition örneği

            //int deger=0;

            //Parallel.ForEach(Enumerable.Range(1, 100000).ToList(), (x) =>
            //  {
            //      deger = x;
            //  });

            //Console.WriteLine(deger);


            //ParallelFor

            //long totalByte = 0;

            //var files = Directory.GetFiles(@"C:\Users\oyku.bilen\Pictures\Feedback\{3EF1E687-2D5E-4F64-AD7E-08DCA621EEF3}\");

            ////Parallel.For<int>() // generic de yazılabilir.

            //Parallel.For(0, files.Length, (index) =>
            //{
            //    var file = new FileInfo(files[index]);

            //    Interlocked.Add(ref totalByte, file.Length);
            //});

            //Console.WriteLine("total byte:" + totalByte);


            //Thread local variables

            int total = 0;

            //Parallel.ForEach(Enumerable.Range(1, 100).ToList(), (item) =>
            // {
            //     //Interlocked.Add(ref total, item);//bu da cok performanaslı degil
            // });

            Parallel.ForEach(Enumerable.Range(1, 100).ToList(), () => 0, (x, loop, subtotal) =>
            {
                subtotal += x;
                return subtotal;// döndükçe subtotale ekleniyor kendi threadi içerisinde
            }, (y) => Interlocked.Add(ref total, y));// local state'inde 0 ile başlasın-> her thread kendi içerisinde gelen degeri toplayarak thread safe sağlıyor
            //for için de yazılabilirdi.
            //shared dataya ulaşılmayacaksa cok da gerek yok
            Console.WriteLine(total);
        }
    }
}
