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

namespace TaskCancellationFormApp
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task<HttpResponseMessage> myTask;

                myTask = new HttpClient().GetAsync("https://localhost:44300/api/home", cancellationTokenSource.Token);

                await myTask;

                var content = await myTask.Result.Content.ReadAsStringAsync();

                richTextBox1.Text = content;
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }     
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();//exception fırlatarak işlemi durduruyoruz.
        }
    }
}
