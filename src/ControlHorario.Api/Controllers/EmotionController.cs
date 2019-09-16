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
    [Authorize(Policy = PolicyNames.Admin)]
    public class EmotionController : ControllerBase
    {
        readonly IEmotionMapper iEmotionMapper;
        readonly IEmotionAppService iEmotionAppService;

        public EmotionController(IEmotionAppService iEmotionAppService,
            IEmotionMapper iEmotionMapper)
        {
            this.iEmotionAppService = iEmotionAppService ??
                throw new ArgumentNullException(nameof(iEmotionAppService));
            this.iEmotionMapper = iEmotionMapper ??
                throw new ArgumentNullException(nameof(iEmotionMapper));
        }

        [HttpGet("{from:datetime}/{to:datetime}")]
        public async Task<IActionResult> GetAsync(DateTime from, DateTime to)
        {
            var emotions = await this.iEmotionAppService.GetAsync(from, to);

            if (emotions == null || !emotions.Any())
                return this.NotFound();

            var response = emotions.Select(x => this.iEmotionMapper.Convert(x));
            return this.Ok(response);
        }
    }
}
