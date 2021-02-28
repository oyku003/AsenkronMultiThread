using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaskWebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetContent(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Request started");
                //Thread.Sleep(5000);
                await Task.Delay(5000, cancellationToken);

                var mytask = new HttpClient().GetStringAsync("https://www.google.com");

                var data = await mytask;
                logger.LogInformation("Request finished");
                return Ok(data);
            }
            catch (TaskCanceledException ex)
            {
                logger.LogInformation($"Request denied {ex.Message}");

                return BadRequest();
            }
           
        }

        [HttpGet]
        public IActionResult GetContent2(CancellationToken cancellationToken)
        {
            /*CancellationTokenSource token = new CancellationTokenSource();
              token.Cancel();//request kullanıcı tarafından iptal edildiğinde arkada otomatik olarak bu metot çalıştırılır
            */
            try
            {
                logger.LogInformation("Request started");
                Enumerable.Range(1, 10).ToList().ForEach(x =>
                {
                    Thread.Sleep(1000);

                    cancellationToken.ThrowIfCancellationRequested();//Her seferinde kullanıcı tarafından iptal edilip edilmediğine bakar.
                });
                //cancellationToken.ThrowIfCancellationRequested();//senkron metotlar için exc. fırlatılarak işlem sonlandırılabilir.
             
                logger.LogInformation("Request finished");
                return Ok("Finished");
            }
            catch (TaskCanceledException ex)
            {
                logger.LogInformation($"Request denied {ex.Message}");

                return BadRequest();
            }

        }
    }
}