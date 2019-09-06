using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ControlHorario.AzureTable.DataAccess.DbEntities
{
    public class EmotionDb : TableEntity
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public double? Smile { get; set; }
        public double? Anger { get; set; }
        public double? Contempt { get; set; }
        public double? Disgust { get; set; }
        public double? Fear { get; set; }
        public double? Happiness { get; set; }
        public double? Neutral { get; set; }
        public double? Sadness { get; set; }
        public double? Surprise { get; set; }
    }
}
