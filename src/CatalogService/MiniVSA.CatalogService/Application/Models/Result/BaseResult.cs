using System.Text.Json.Serialization;

namespace MiniVSA.CatalogService.Application.Models.Result
{
    public class BaseResult<TData>
    {
        public TData Data { get; set; }

        public string Message { get; set; }

        public int HttpStatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public ErrorResult ErrorDto { get; set; }
    }
}
