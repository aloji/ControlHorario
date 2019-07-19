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
            Validate();

            await CreatePersonGroupIfNotExist();

            var faceperson = await Client.PersonGroupPerson.CreateAsync(
                options.CurrentValue.PersonGroupId,
                person.Name,
                person.Id.ToString());

            return faceperson.PersonId;

            void Validate()
            {
                if (person == null)
                    throw new ArgumentNullException(nameof(person));
            }
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

        public async Task AddFaceAsync(Guid facePersonId, string url)
        {
            var detectedFace = await DetectAsync(url);

            if (detectedFace != null && detectedFace.FaceId.HasValue)
            {
                await this.Client.PersonGroupPerson.AddFaceFromUrlAsync(
                    options.CurrentValue.PersonGroupId, facePersonId, url, null,
                    new List<int> {
                            detectedFace.FaceRectangle.Left,
                            detectedFace.FaceRectangle.Top,
                            detectedFace.FaceRectangle.Width,
                            detectedFace.FaceRectangle.Height
                    });
            }
        }

        public async Task<Guid?> GetFacePersonId(byte[] data)
        {
            var detectedFace = await DetectAsync(data);
            var result = await GetFacePersonId(detectedFace);
            return result;
        }

        public async Task<Guid?> GetFacePersonId(string url)
        {
            var detectedFace = await DetectAsync(url);
            var result = await GetFacePersonId(detectedFace);
            return result;
        }

        private async Task<Guid?> GetFacePersonId(DetectedFace detectedFace)
        {
            var result = default(Guid?);
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

        private async Task<DetectedFace> DetectAsync(string url)
        {
            var result = default(DetectedFace);
            var detectedFaces = await this.Client.Face.DetectWithUrlAsync(url,
                returnFaceId: true,
                recognitionModel: RecognitionModel.Recognition02);

            if (detectedFaces != null && detectedFaces.Any())
            {
                result = detectedFaces.First();
            }
            return result;
        }

        public async Task<bool> TrainAndWaitAsync()
        {
            var personGroupId = this.options.CurrentValue.PersonGroupId;
            var millisecondsDelay = 1000;

            await CreatePersonGroupIfNotExist();

            await this.Client.PersonGroup.TrainAsync(personGroupId);

            var trainingStatus = await this.Client.PersonGroup.GetTrainingStatusAsync(personGroupId);

            while (trainingStatus?.Status != null
                   && !trainingStatus.Status.Equals(TrainingStatusType.Succeeded)
                   && !trainingStatus.Status.Equals(TrainingStatusType.Failed))
            {
                await Task.Delay(millisecondsDelay);

                trainingStatus = await this.Client.PersonGroup.GetTrainingStatusAsync(personGroupId);
            }

            var result = trainingStatus?.Status == TrainingStatusType.Succeeded;
            return result;
        }
    }
}
