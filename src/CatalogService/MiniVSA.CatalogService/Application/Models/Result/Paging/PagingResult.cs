namespace MiniVSA.CatalogService.Application.Models.Result.Paging
{
    public class PagingResult<TData> : BaseResult<TData>
    {
        public PagingMetaData PagingMetaData { get; set; }

        public static PagingResult<TData> Success(TData data, PagingMetaData pagingMetaData, int statusCode)
        {
            return new PagingResult<TData> { Data = data, PagingMetaData = pagingMetaData, HttpStatusCode = statusCode };
        }

        public static PagingResult<TData> Success(string message, int statusCode)
        {
            return new PagingResult<TData> { Message = message, HttpStatusCode = statusCode };
        }

        public static PagingResult<TData> Success(string message, TData data, PagingMetaData pagingMetaData, int statusCode)
        {
            return new PagingResult<TData> { Message = message, Data = data, PagingMetaData = pagingMetaData, HttpStatusCode = statusCode };
        }

        public static PagingResult<TData> Success(int statusCode)
        {
            return new PagingResult<TData> { Data = default, PagingMetaData = default, HttpStatusCode = statusCode };
        }

        public static PagingResult<TData> Error(ErrorResult errorDto, int statusCode)
        {
            return new PagingResult<TData> { ErrorDto = errorDto, HttpStatusCode = statusCode, IsSuccessful = false };
        }

        public static PagingResult<TData> Error(string errorMessage, int statusCode, bool isShow = true)
        {
            return new PagingResult<TData>
            {
                Data = default,
                PagingMetaData = default,
                ErrorDto = new(errorMessage, isShow),
                HttpStatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
