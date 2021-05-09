using Jewelry_Store.DTO;
using Jewelry_Store.Filters;
using Jewelry_Store.ProviderInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace Jewelry_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JewelryController : ControllerBase
    {
        private readonly IHostingEnvironment _environment;
        private readonly IJewelryProvider _jewelryProvider;
        public JewelryController(IJewelryProvider jewelryProvider, IHostingEnvironment environment)
        {
            _environment = environment;
            _jewelryProvider = jewelryProvider;
        }

        [AuthorizationFilter(500, 501)]
        [HttpGet("fileDownload")]
        public object FileDownload([FromQuery]FileDataRequestDTO request)
        {
            if (request.IsValid())
            {
                var path = Path.Combine(_environment.ContentRootPath, "Templates/JewelryTemplate.html");

                string htmlth = "";
                string htmltd = "";
                if (!string.IsNullOrEmpty(request.DiscountInPercentage))
                {
                    htmlth = @"<tr>
                              <th> Gold price (per gram) </th>
                              <th> Weight (grams) </th>
                              <th> Discount % </th>
                              <th> Total price </th>
                            </tr>";

                    htmltd = @"<tr> 
                                 <td>" + request.GoldPrice + "</td>" +
                                 "<td> " + request.Weight + " </td> " +
                                 "<td> " + request.DiscountInPercentage + " </td> " +
                                 "<td> " + request.TotalPrice + " </td> " +
                              "</tr>";
                }
                else
                {
                    htmlth = @"<tr>
                              <th> Gold Price </th>
                              <th> Weight </th>
                              <th> Total Price </th>
                            </tr>";

                    htmltd = @"<tr> 
                                        <td>" + request.GoldPrice + "</td>" +
                                      "<td> " + request.Weight + " </td> " +
                                      "<td> " + request.TotalPrice + " </td> " +
                                      "</tr>";
                }

                StringBuilder body = new StringBuilder();
                using (StreamReader reader = new StreamReader(path))
                {
                    body.Append(reader.ReadToEnd());
                }
                body.Replace("[table-column]", htmlth);
                body.Replace("[table-row]", htmltd);

                byte[] fileBytes = _jewelryProvider.ConvertHTMLToPDF(body.ToString());
                base.Response.Headers.Add("content-disposition", "attachment; filename=JewelryStore.pdf");
                base.Response.ContentType = "application/pdf";
                return File(fileBytes, "application/pdf");
            }
            return null;
        }
    }
}
