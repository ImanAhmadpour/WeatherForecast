using Domain.Utility;
using System.Text.Json;

namespace API.APIConfig
{
    public class ResultModel
    {
        public object Entries { get; set; }
        public object Result { get; set; }
        public ResultStatusInfo Status { get; set; }

        public ResultModel() { }
        public ResultModel(object entries) { Entries = entries; }

        public ResultModel AddStatus(ResultStatus status)
        {
            this.Status = status;
            return this;
        }

        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
