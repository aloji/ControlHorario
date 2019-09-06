using ControlHorario.Domain.Entities;
using ControlHorario.Domain.Events;
using ControlHorario.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ControlHorario.Application.Events.Handlers
{
    public class SaveEmotionWhenFaceDetectedHandler : IEventHandler<FaceDetectedEvent>
    {
        readonly IPersonRepository iPersonRepository;
        readonly IEmotionRepository iEmotionRepository;

        public SaveEmotionWhenFaceDetectedHandler(
            IPersonRepository iPersonRepository, IEmotionRepository iEmotionRepository)
        {
            this.iPersonRepository = iPersonRepository ??
                throw new ArgumentNullException(nameof(iPersonRepository));

            this.iEmotionRepository = iEmotionRepository ??
               throw new ArgumentNullException(nameof(iEmotionRepository));
        }
        public async Task HandleAsync(FaceDetectedEvent iEvent)
        {
            if (iEvent != null)
            {
                var person = await this.iPersonRepository.GetByFacePersonId(iEvent.FacePersonId);
                if (person != null)
                {
                    var emotion = new Emotion
                    {
                        Anger = iEvent.Anger,
                        Contempt = iEvent.Contempt,
                        DateTimeUtc = DateTime.UtcNow,
                        Disgust = iEvent.Disgust,
                        Fear = iEvent.Fear,
                        Happiness = iEvent.Happiness,
                        Id = Guid.NewGuid(),
                        Neutral = iEvent.Neutral,
                        PersonId = person.Id,
                        Sadness = iEvent.Sadness,
                        Smile = iEvent.Smile,
                        Surprise = iEvent.Surprise
                    };

                    await this.iEmotionRepository.CreateAsync(emotion);
                }
            }
        }
    }
}
