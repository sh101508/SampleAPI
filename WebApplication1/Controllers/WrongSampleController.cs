using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class WrongSampleController : ControllerBase
    {

        [HttpGet("Get/Something")]
        public async Task<string> GetSomething()
        {
            Task.Delay(100).Wait();

            return "GetSomething";
        }

        [HttpPost("Create/Something/{id}")]
        public async Task CreateSomething(int id)
        {
            Task.Delay(100).Wait();
        }

        [HttpPut("Update/Something/{id}")]
        public async Task UpdateSomething( int id)
        {
            Task.Delay(100).Wait();
        }

        [HttpDelete("Delete/Something/{id}")]
        public async Task DeleteSomething(int id)
        {
            Task.Delay(100).Wait();
        }
    }
}
