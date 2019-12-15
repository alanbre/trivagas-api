using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TriVagas.Services.Interfaces;
using TriVagas.Services.Notify;
using TriVagas.Services.Requests;
using TriVagas.WebApi.Filters;

namespace TriVagas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CompanyController : ApiController
    {
        public CompanyController(INotify notify) : base(notify) { }


        [TypeFilter(typeof(JWTokenAuthFilter))]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCompanyRequest company,
            [FromServices] ICompanyService companyService,
            [FromServices] IJWTService jWTService,
            [FromServices] IUserService userService)
        {
            var claims = jWTService.GetTokenClaims(Request.Headers["JWToken"]);
            var email = claims.First().Value;
            var user = await userService.GetByEmail(email);

            if (user == null)
            {
                return Response(code:404);
            }

            var createdCompany = await companyService.Register(company, user);

            if (createdCompany != null)
            {
                return Response(createdCompany, 201);
            }
            else
            {
                return Response(code:400);
            }
        }
    }
}