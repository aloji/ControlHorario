using ControlHorario.Domain.Events;
using System;

namespace ControlHorario.Application.Events
{
    public class FaceDetectedEvent : IEvent
    {
        public Guid FacePersonId { get; }
        public double? Smile { get; }
        public double? Anger { get; }
        public double? Contempt { get; }
        public double? Disgust { get; }
        public double? Fear { get; }
        public double? Happiness { get; }
        public double? Neutral { get; }
        public double? Sadness { get; }
        public double? Surprise { get; }

        public FaceDetectedEvent(Guid facePersonId, double? smile, double? anger, 
            double? contempt, double? disgust, double? fear, double? happiness, double? neutral,
            double? sadness, double? surprise)
        {
            this.FacePersonId = facePersonId;
            this.Smile = smile;
            this.Anger = anger;
            this.Contempt = contempt;
            this.Disgust = disgust;
            this.Fear = fear;
            this.Happiness = happiness;
            this.Neutral = neutral;
            this.Sadness = sadness;
            this.Surprise = surprise;
        }
    }
}
