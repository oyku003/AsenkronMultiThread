using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlinqFormApp
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cts;
        public Form1()
        {
           cts = new CancellationTokenSource();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(()=>
           {
               try
               {
                   Enumerable.Range(1, 100000).AsParallel().WithCancellation(cts.Token).Where(Hesapla).ToList().ForEach(x =>
                   {
                       Thread.Sleep(100);
                       listBox1.Invoke((MethodInvoker)delegate { listBox1.Items.Add(x); });
                   });
               }
               catch(OperationCanceledException ex)//sadece iptal işlemini yakalamak için
               {
                   MessageBox.Show("İşlem iptal edildi");
               }
               catch (Exception)
               {
                   MessageBox.Show("Genel bir hata meydana geldi");
               }
               
           });//uı threadi bloklamasın diye task.run ile yani ayrı thread ile çalışmasını sağladık
        }

        private bool Hesapla(int x)
        {
            Thread.SpinWait(500000);//verdiğimiz iterasyon kadar while döngüsü dönüyormuş gibi düşünebiliriz.Ana threadi de bloklamış oluruz.
            return x % 12 == 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
