using ControlHorario.Api.Mappers;
using ControlHorario.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ControlHorario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        readonly IPersonAppService iPersonAppService;
        readonly IPersonMapper iPersonMapper;
        readonly IRecordMapper iRecordMapper;
        public PersonController(IPersonAppService iPersonAppService,
            IPersonMapper iPersonMapper, IRecordMapper iRecordMapper)
        {
            this.iPersonAppService = iPersonAppService ??
                throw new ArgumentNullException(nameof(iPersonAppService));
            this.iPersonMapper = iPersonMapper ??
                throw new ArgumentNullException(nameof(iPersonMapper));
            this.iRecordMapper = iRecordMapper ??
                throw new ArgumentNullException(nameof(iRecordMapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var person = await this.iPersonAppService.GetByIdAsync(id);
            if (person == null)
                return this.NotFound();

            var response = this.iPersonMapper.Convert(person);
            return this.Ok(response);
        }

        [HttpGet("{id}/records")]
        public async Task<IActionResult> GetRecords(Guid id)
        {
            var records = await this.iPersonAppService.GetRecordsAsync(id);
            if (records == null || !records.Any())
                return this.NotFound();

            var response = records.Select(x => this.iRecordMapper.Convert(x));
            return this.Ok(response);
        }
    }
}
