namespace MiniVSA.CatalogService.Application.Models.Exception
{
    public record GlobalExceptionResponseModel(string Title,
                                               List<Error> Errors);

    public record Error(string PropertyName,
                        string Message);
}
