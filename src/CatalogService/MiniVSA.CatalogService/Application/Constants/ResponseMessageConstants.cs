namespace MiniVSA.CatalogService.Application.Constants
{
    public static class ResponseMessageConstants
    {
        public struct Brand
        {
            public struct Success
            {
                public const string BrandCreated = "Marka başarıyla oluşturuldu.";

                public const string BrandDeleted = "Marka silme işlemi başarılı.";

                public const string BrandFound = "Marka bulundu.";

                public const string BrandUpdated = "Marka güncelleme işlemi başarılı.";
            }

            public struct Error
            {
                public const string BrandAlreadyExists = "Böyle bir marka zaten mevcut.";

                public const string BrandImageUploadError = "Marka resmi yüklenirken bir hata oluştu.";

                public const string BrandNotFound = "Marka bulunamadı.";

                public const string AnyBrandNotFound = "Henüz hiçbir marka bulunmuyor.";

                public const string BrandNotFoundForDelete = "Silinecek marka bulunamadı.";

                public const string BrandNotFoundForUpdate = "Güncellenecek marka bulunamadı.";
            }
        }
    }
}
