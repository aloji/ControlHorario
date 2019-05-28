using ControlHorario.Application.Options;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ControlHorario.Application.Services
{
    public class FaceAppService : IFaceAppService
    {
        readonly IOptionsMonitor<FaceOptions> options;

        public FaceAppService(IOptionsMonitor<FaceOptions> options)
        {
            this.options = options ??
                throw new ArgumentNullException(nameof(options));
        }

        IFaceClient iFaceClient;
        private IFaceClient Client
        {
            get
            {
                if (iFaceClient == null)
                {
                    this.iFaceClient = new FaceClient(
                        new ApiKeyServiceClientCredentials(options.CurrentValue.SubscriptionKey))
                    {
                        Endpoint = options.CurrentValue.Endpoint
                    };
                }
                return this.iFaceClient;
            }
        }

        private bool exitPersonGroup;
        private async Task CreatePersonGroupIfNotExist()
        {
            if (exitPersonGroup)
                await Task.CompletedTask;

            try
            {
                await this.Client.PersonGroup.CreateAsync(
                    personGroupId: options.CurrentValue.PersonGroupId,
                    name: options.CurrentValue.PersonGroupName,
                    recognitionModel: RecognitionModel.Recognition02);
            }
            catch (APIErrorException ex) when (ex.Response.StatusCode == HttpStatusCode.Conflict)
            {
            }

            exitPersonGroup = true;
        }

        public async Task<Guid> CreateAsync(Domain.Entities.Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            await CreatePersonGroupIfNotExist();

            var faceperson = await Client.PersonGroupPerson.CreateAsync(
                options.CurrentValue.PersonGroupId,
                person.Name,
                person.Id.ToString());

            return faceperson.PersonId;
        }

        public async Task AddFaceAsync(Guid facePersonId, byte[] data)
        {
            var detectedFace = await DetectAsync(data);

            if (detectedFace != null && detectedFace.FaceId.HasValue)
            {
                using (var stream = new MemoryStream(data))
                {
                    await this.Client.PersonGroupPerson.AddFaceFromStreamAsync(
                        options.CurrentValue.PersonGroupId, facePersonId, stream, null,
                        new List<int> {
                                detectedFace.FaceRectangle.Left,
                                detectedFace.FaceRectangle.Top,
                                detectedFace.FaceRectangle.Width,
                                detectedFace.FaceRectangle.Height
                        });
                }
            }
        }

        public async Task<Guid?> GetFacePersonId(byte[] data)
        {
            var result = default(Guid?);

            if(data == null)
                throw new ArgumentNullException(nameof(data));

            var detectedFace = await DetectAsync(data);
            if (detectedFace != null && detectedFace.FaceId.HasValue)
            {
                var identificationResults = await this.Client.Face.IdentifyAsync(
                    new List<Guid> { detectedFace.FaceId.Value },
                    this.options.CurrentValue.PersonGroupId);

                if (identificationResults != null && identificationResults.Any())
                {
                    var candidate = identificationResults.First()
                        .Candidates.FirstOrDefault();

                    if (candidate != null
                        && candidate.Confidence > 0.5)
                    {
                        result = candidate.PersonId;
                    }
                }
            }
            return result;
        }

        private async Task<DetectedFace> DetectAsync(byte[] data)
        {
            var result = default(DetectedFace);
            using (var stream = new MemoryStream(data))
            {
                var detectedFaces = await this.Client.Face.DetectWithStreamAsync(stream,
                    returnFaceId: true,
                    recognitionModel: RecognitionModel.Recognition02);

                if (detectedFaces != null && detectedFaces.Any())
                {
                    result = detectedFaces.First();
                }
            }
            return result;
        }
    }
}
