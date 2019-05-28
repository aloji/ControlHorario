using ControlHorario.Api.Extensions;
using ControlHorario.Api.Mappers;
using ControlHorario.Api.Models.Request;
using ControlHorario.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ControlHorario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        readonly IPersonAppService iPersonAppService;
        readonly IFaceAppService iFaceAppService;
        readonly IPersonMapper iPersonMapper;
        readonly IRecordMapper iRecordMapper;
        public PersonController(IPersonAppService iPersonAppService,
            IFaceAppService iFaceAppService,
            IPersonMapper iPersonMapper, IRecordMapper iRecordMapper)
        {
            this.iPersonAppService = iPersonAppService ??
                throw new ArgumentNullException(nameof(iPersonAppService));
            this.iFaceAppService = iFaceAppService ??
                throw new ArgumentNullException(nameof(iFaceAppService));
            this.iPersonMapper = iPersonMapper ??
                throw new ArgumentNullException(nameof(iPersonMapper));
            this.iRecordMapper = iRecordMapper ??
                throw new ArgumentNullException(nameof(iRecordMapper));
        }

        [HttpGet("{id}", Name = RouteNames.PersonGetById)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var person = await this.iPersonAppService.GetByIdAsync(id);
            if (person == null)
                return this.NotFound();

            var response = this.iPersonMapper.Convert(person);
            return this.Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PersonRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return this.BadRequest();

            var person = this.iPersonMapper.Convert(request);
            await this.iPersonAppService.CreateAsync(person);

            var response = this.iPersonMapper.Convert(person);
            return this.CreatedAtRoute(RouteNames.PersonGetById, response.Id, response);
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

        [HttpPost("identify")]
        public async Task<IActionResult> FaceIdentify([FromBody] string dataUrl)
        {
            byte[] data = dataUrl.FromDataUrl();

            if (data == default(byte[]))
                return this.BadRequest();

            var facePersonId = await this.iFaceAppService.GetFacePersonId(data);
            if(facePersonId.HasValue)
            {
                var person = await this.iPersonAppService.GetByFacePersonIdAsync(facePersonId.Value);
                if (person != null)
                {
                    var response = this.iPersonMapper.Convert(person);
                    return this.Ok(response);
                }
            }

            return this.NotFound();
        }

        [HttpPost("{id}/face")]
        public async Task<IActionResult> AddFace(Guid id, [FromBody] string dataUrl)
        {
            byte[] data = dataUrl.FromDataUrl();

            if (data == default(byte[]))
                return this.BadRequest();

            var person = await this.iPersonAppService.GetByIdAsync(id);
            if (person == null)
                return this.NotFound();

            if (!person.FacePersonId.HasValue)
            {
                person.FacePersonId = await this.iFaceAppService.CreateAsync(person);
                await this.iPersonAppService.UpdateAsync(person);
            }

            await this.iFaceAppService.AddFaceAsync(person.FacePersonId.Value, data);

            return this.Ok();
        }
    }
}
