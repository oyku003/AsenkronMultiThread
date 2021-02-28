using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static int Counter { get; set; } =0;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string data = string.Empty;
            Task<string> okuma =  ReadFileAsync(); //ReadFile();

            richTextBox2.Text = await new HttpClient().GetStringAsync("https://www.google.com");
            data = await okuma;

            richTextBox1.Text = data;
        }

        private void BtnCounter_Click(object sender, EventArgs e)
        {
            textBoxCounter.Text =  Counter++.ToString();
        }

        private string ReadFile()//senkron
        {
            string data = string.Empty;

            using(StreamReader s = new StreamReader("dosya.txt"))
            {
                Thread.Sleep(5000);//ana threadi bloklar
                data = s.ReadToEnd();
            }

            return data;
        }

        private async Task<string> ReadFileAsync()
        {
            string data = string.Empty;

            using (StreamReader s = new StreamReader("dosya.txt"))
            {
                Task<string> myTask = ReadFileAsync2();//s.ReadToEndAsync();

                //bu mettola ilgisi olmayan başka işler yapabiliriz.
                await Task.Delay(5000);//5 saniye cevabı alana kadar bekle. await cevabı al oyle devam et demek
                data = await myTask;//aradaki işlem 10 saniye sürdü, okuma işlemi de 15 ise kod buraya gelince 5 saniye daha bekler
            }

            return data;
        }

        private Task<string> ReadFileAsync2()// direkt geriye döndüğümüzde async await kullanmamıza gerek yok
        {
            //using( )//ReadFileAsync2 cağırıldığı yerde (59.satır) patlar cunku using bloğu s yi bellekten düşürmüştü o yüzden using içine almadan kullanmak zorunda kaldık
            //{
            StreamReader s = new StreamReader("dosya.txt");
            return s.ReadToEndAsync();
           // }
        }
    }
}
