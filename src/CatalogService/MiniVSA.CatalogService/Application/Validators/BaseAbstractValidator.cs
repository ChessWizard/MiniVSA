using FluentValidation;
using MiniVSA.CatalogService.Application.Models.Common.Request;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Application.Validators
{
    public abstract class BaseAbstractValidator<TModel> : AbstractValidator<TModel>
    {
        // File Validation
        protected void RuleForFileRequired(Func<TModel, FileUploadRequestModel> selector, string message)
        {
            RuleFor(model => selector(model).Base64File)
                .NotNull().WithMessage(message)
                .NotEmpty().WithMessage(message);
        }

        protected void RuleForFileSize(Func<TModel, FileUploadRequestModel> selector, double maxValue, string message)
        {
            RuleFor(x => selector(x).Size)
                .LessThanOrEqualTo(maxValue).WithMessage(message);
        }

        protected void RuleForFileAsImage(Func<TModel, FileUploadRequestModel> selector, string message)
        {
            RuleFor(model => selector(model).Base64File)
                .Must(ValidateFileAsImage).WithMessage(message);
        }

        #region Private Methods

        // File Validation
        private static bool ValidateFileAsImage(byte[] base64File)
        {
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            var mimeType = FileHelper.GetFileAttributesFromByteArray(base64File)?.MimeType;
            return allowedMimeTypes.Contains(mimeType);
        }

        #endregion
    }
}
