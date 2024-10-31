namespace MiniVSA.CatalogService.Application.Constants
{
    public static class ResponseMessageConstants
    {
        public struct Brand
        {
            public struct Success
            {
                public const string BrandCreated = "Marka başarıyla oluşturuldu.";
            }

            public struct Error
            {
                public const string BrandAlreadyExists = "Böyle bir marka zaten mevcut.";

                public const string BrandImageUploadError = "Marka resmi yüklenirken bir hata oluştu.";

                public const string AnyBrandNotFound = "Henüz marka bulunmuyor.";
            }
        }
    }
}
