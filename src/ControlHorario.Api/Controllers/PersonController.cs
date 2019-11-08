using ControlHorario.Api.Extensions;
using ControlHorario.Api.Mappers;
using ControlHorario.Api.Models.Request;
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
    public class PersonController : ControllerBase
    {
        readonly IPersonAppService iPersonAppService;
        readonly IRecordAppService iRecordAppService;
        readonly IFaceAppService iFaceAppService;
        readonly IEmotionAppService iEmotionAppService;
        readonly IReportAppService iReportAppService;
        readonly IPersonMapper iPersonMapper;
        readonly IRecordMapper iRecordMapper;
        readonly IEmotionMapper iEmotionMapper;
        readonly IReportMapper iReportMapper;
        public PersonController(IPersonAppService iPersonAppService,
            IRecordAppService iRecordAppService,
            IFaceAppService iFaceAppService,
            IEmotionAppService iEmotionAppService,
            IReportAppService iReportAppService,
            IPersonMapper iPersonMapper, 
            IRecordMapper iRecordMapper, 
            IEmotionMapper iEmotionMapper,
            IReportMapper iReportMapper)
        {
            this.iPersonAppService = iPersonAppService ??
                throw new ArgumentNullException(nameof(iPersonAppService));
            this.iRecordAppService = iRecordAppService ??
               throw new ArgumentNullException(nameof(iRecordAppService));
            this.iFaceAppService = iFaceAppService ??
                throw new ArgumentNullException(nameof(iFaceAppService));
            this.iEmotionAppService = iEmotionAppService ??
               throw new ArgumentNullException(nameof(iEmotionAppService)); 
            this.iReportAppService = iReportAppService ??
                throw new ArgumentNullException(nameof(iReportAppService));
            this.iPersonMapper = iPersonMapper ??
                throw new ArgumentNullException(nameof(iPersonMapper));
            this.iRecordMapper = iRecordMapper ??
                throw new ArgumentNullException(nameof(iRecordMapper));
            this.iEmotionMapper = iEmotionMapper ??
              throw new ArgumentNullException(nameof(iEmotionMapper));
            this.iReportMapper = iReportMapper ??
             throw new ArgumentNullException(nameof(iReportMapper));
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
        [Authorize(Policy = PolicyNames.Admin)]
        public async Task<IActionResult> GetAsync()
        {
            var persons = await this.iPersonAppService.GetAsync();
            if (persons == null || !persons.Any())
                return this.NotFound();

            var response = persons.Select(x => this.iPersonMapper.Convert(x));
            return this.Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.Admin)]
        public async Task<IActionResult> PostAsync(PersonRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return this.BadRequest();

            var person = this.iPersonMapper.Convert(request);
            await this.iPersonAppService.CreateAsync(person);

            var response = this.iPersonMapper.Convert(person);
            return this.CreatedAtRoute(RouteNames.PersonGetById, new { id = response.Id }, response);
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
        public async Task<IActionResult> PostRecordAsync(Guid id, [FromBody] RecordRequest request)
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

        [HttpDelete("{id}/record/{recordId}")]
        public async Task<IActionResult> DeleteRecordAsync(Guid id, Guid recordId)
        {
            await this.iRecordAppService.DeleteAsync(id, recordId);
            return this.NoContent();
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
        [Authorize(Policy = PolicyNames.Admin)]
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
        [Authorize(Policy = PolicyNames.Admin)]
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

        [HttpGet("{id}/emotion")]
        public async Task<IActionResult> GetEmotionAsync(Guid id, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var emotions = from.HasValue && to.HasValue
                ? await this.iEmotionAppService.GetAsync(id, from.Value, to.Value)
                : await this.iEmotionAppService.GetAsync(id);

            if (emotions == null || !emotions.Any())
                return this.NotFound();

            var response = emotions.Select(x => this.iEmotionMapper.Convert(x));
            return this.Ok(response);
        }

        [HttpGet("{id}/report")]
        public async Task<IActionResult> GetReportAsync(Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var report = await this.iReportAppService.GetAsync(id, from, to);

            if (report == null)
                return this.NotFound();

            var response = this.iReportMapper.Convert(report);
            return this.Ok(response);
        }
    }
}
