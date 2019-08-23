using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PieshopFunctions.Contract;
using PieshopFunctions.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PieshopFunctions
{
    public class GetAllPies
    {

        private readonly AppDbContext _appDbContext;

        public GetAllPies(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [FunctionName("GetAllPies")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<PieContract>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/getallpies")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var pies = await _appDbContext.Pies.ToListAsync();
           
            if (pies == null) { new NotFoundResult(); }

            //convert to contract
            var response = new List<PieContract>();
            foreach (var pie in pies)
            {
                response.Add(new PieContract
                {
                     Id = pie.Id, ImageThumbnailUrl = pie.ImageThumbnailUrl, ImageUrl = pie.ImageUrl, IsInStock = pie.IsInStock, IsPieOfTheWeek = pie.IsPieOfTheWeek,
                      LongDescription = pie.LongDescription, Name = pie.Name, Price = pie.Price, ShortDescription = pie.ShortDescription
                });
            }

            return new OkObjectResult(response);

        }

    }
}

