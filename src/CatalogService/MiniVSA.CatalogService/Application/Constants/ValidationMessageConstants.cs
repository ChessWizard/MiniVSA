namespace MiniVSA.CatalogService.Application.Constants
{
    public static class ValidationMessageConstants
    {
        public struct Brand
        {
            public const string BrandNameRequired = "Marka adı zorunludur.";

            public static string BrandNameMaxLength(int maxLength) => $"Marka adı en fazla {maxLength} karakter olabilir.";

            public const string BrandFileAsImage = "Yüklenen dosya bir görsel olmalıdır.";

            public const string BrandImageRequired = "Markaya ait bir görsel yüklenmelidir.";

            public static string BrandImageMaxSize(double maxSize) => $"Yüklenen görsel boyutu {maxSize}MB'dan büyük olamaz.";
        }
    }
}
