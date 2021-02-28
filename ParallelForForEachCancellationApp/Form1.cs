using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParallelForForEachCancellationApp
{
    public partial class Form1 : Form
    {
        CancellationTokenSource ct;
        int counter = 0;
        public Form1()
        {
            InitializeComponent();
            ct = new CancellationTokenSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ct = new CancellationTokenSource();//her seferinde başka token oalcagından dolayı getir butonuna basınca yeni işlem yapılması için 
            List<string> urls = new List<string>()
            {
                "https://www.google.com",
                "https://microsoft.com",
                "https://amazon.com",
                "https://www.google.com",
                "https://microsoft.com",
                "https://amazon.com",
                "https://www.google.com",
                "https://microsoft.com",
                "https://amazon.com",
                "https://www.google.com",
                "https://microsoft.com",
                "https://amazon.com",
            };

            HttpClient httpClient = new HttpClient();

            ParallelOptions parallelOptions = new ParallelOptions();

            parallelOptions.CancellationToken = ct.Token;

            Task.Run(() =>//paralel farklı threadde degil, içindeki metotlar aynı threadde o yüzden ana thread donmasın diye task run koyduk
            {
                try
                {
                    Parallel.ForEach<string>(urls, parallelOptions, (url) =>
                    {
                        string content = httpClient.GetStringAsync(url).Result;//her bir threadi kendi içinde result yazarak blokladık ki bir hata fırlatıldığında tüm döngü hata fırlatacagı için handle edemeyecegimizden ve uygulama kapanacagından dolayı. Hata fırlamazsa asenkron calışır sorun yok.

                        string data = $"{url}:{content.Length}";

                        ct.Token.ThrowIfCancellationRequested();//ayrı threadler çalışacagı için manuel hata fırlatmamız lazım
                        //parallelOptions.CancellastionToken.Throw... //CancellationTokenSource 'a erişemediğimiz durumda manuel hata fırlatmak için parallelOptions kullanabiliriz.
                        listBox1.Invoke((MethodInvoker)delegate { listBox1.Items.Add(data); });
                    });
                }
                catch(OperationCanceledException ex2)
                {
                    MessageBox.Show("İşlem iptal edildi:" + ex2.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata meydana geldi" + ex.Message);
                }
              
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ct.Cancel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = counter++.ToString();
        }
    }
}
