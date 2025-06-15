using System.Text.Json.Serialization;

namespace HomeProject.Models.Response
{
    public class ResponseModel<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
    }
}