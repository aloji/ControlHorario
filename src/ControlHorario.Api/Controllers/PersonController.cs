using ControlHorario.Api.Extensions;
using ControlHorario.Api.Mappers;
using ControlHorario.Api.Models.Request;
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
        readonly IRecordAppService iRecordAppService;
        readonly IFaceAppService iFaceAppService;
        readonly IPersonMapper iPersonMapper;
        readonly IRecordMapper iRecordMapper;
        public PersonController(IPersonAppService iPersonAppService,
            IRecordAppService iRecordAppService,
            IFaceAppService iFaceAppService,
            IPersonMapper iPersonMapper, IRecordMapper iRecordMapper)
        {
            this.iPersonAppService = iPersonAppService ??
                throw new ArgumentNullException(nameof(iPersonAppService));
            this.iRecordAppService = iRecordAppService ??
               throw new ArgumentNullException(nameof(iRecordAppService));
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

        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            var persons = await this.iPersonAppService.GetAsync();
            if (persons == null || !persons.Any())
                return this.NotFound();

            var response = persons.Select(x => this.iPersonMapper.Convert(x));
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

        [HttpGet("{id}/record")]
        public async Task<IActionResult> GetRecordAsync(Guid id, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var records = from.HasValue && to.HasValue 
                ? await this.iRecordAppService.GetAsync(id, from.Value, to.Value) 
                : await this.iRecordAppService.GetAsync(id);

            if (records == null || !records.Any())
                return this.NotFound();

            var response = records.Select(x => this.iRecordMapper.Convert(x));
            return this.Ok(response);
        }

        [HttpPost("{id}/record")]
        public async Task<IActionResult> PostRecordAsyn(Guid id, [FromBody] RecordRequest request)
        {
            if (request == null)
                return this.BadRequest();

            var record = this.iRecordMapper.Convert(request, id);
            try
            {
                await this.iRecordAppService.CreateAsync(record);
            }
            catch(ArgumentOutOfRangeException)
            {
                return this.BadRequest();
            }
            var response = this.iRecordMapper.Convert(record);
            return this.CreatedAtRoute("", response);
        }

        [HttpPost("identifybydata")]
        public async Task<IActionResult> FaceIdentifyByData([FromBody] string dataUrl)
        {
            byte[] data = dataUrl.FromDataUrl();
            if (data == default(byte[]))
                return this.BadRequest();

            var result = await this.FaceIdentify(
                async () => await this.iFaceAppService.GetFacePersonId(data));

            return result;
        }

        [HttpPost("identifybyurl")]
        public async Task<IActionResult> FaceIdentifyByUrl([FromQuery] string url)
        {
            if (!url.IsUrl())
                return this.BadRequest();

            var result = await this.FaceIdentify(
                async () => await this.iFaceAppService.GetFacePersonId(url));

            return result;
        }

        private async Task<IActionResult> FaceIdentify(Func<Task<Guid?>> getFacePersonId)
        {
            var facePersonId = await getFacePersonId();
            if (facePersonId.HasValue)
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

        [HttpPost("{id}/facebydata")]
        public async Task<IActionResult> AddFaceByData(Guid id, [FromBody] string dataUrl)
        {
            byte[] data = dataUrl.FromDataUrl();
            if (data == default(byte[]))
                return this.BadRequest();

            var result = await this.AddFace(id,
                async (facePersonId) => await this.iFaceAppService.AddFaceAsync(facePersonId, data));

            return result;
        }

        [HttpPost("{id}/facebyurl")]
        public async Task<IActionResult> AddFaceByUrl(Guid id, [FromQuery] string url)
        {
            if (!url.IsUrl())
                return this.BadRequest();

            var result = await this.AddFace(id,
                async (facePersonId) => await this.iFaceAppService.AddFaceAsync(facePersonId, url));

            return result;
        }

        private async Task<IActionResult> AddFace(Guid id, Func<Guid, Task> addFace)
        {
            var person = await this.iPersonAppService.GetByIdAsync(id);
            if (person == null)
                return this.NotFound();

            if (!person.FacePersonId.HasValue)
            {
                person.FacePersonId = await this.iFaceAppService.CreateAsync(person);
                await this.iPersonAppService.UpdateAsync(person);
            }

            await addFace(person.FacePersonId.Value);

            return this.Ok();
        }
    }
}
