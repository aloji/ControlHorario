using ControlHorario.Api.Mappers;
using ControlHorario.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        readonly IReportAppService iReportAppService;
        readonly IReportMapper iReportMapper;

        public ReportController(IReportAppService iReportAppService,
            IReportMapper iReportMapper)
        {
            this.iReportAppService = iReportAppService ??
               throw new ArgumentNullException(nameof(iReportAppService));

            this.iReportMapper = iReportMapper ??
              throw new ArgumentNullException(nameof(iReportMapper));
        }

        [HttpGet("{from:datetime}/{to:datetime}")]
        [Authorize(Policy = PolicyNames.Admin)]
        public async Task<IActionResult> GetAsync(DateTime from, DateTime to)
        {
            var reports = await this.iReportAppService.GetAsync(from, to);
            if (reports == null)
                return this.NotFound();

            var response = reports.Select(x => this.iReportMapper.Convert(x));
            return this.Ok(response);
        }
    }
}
