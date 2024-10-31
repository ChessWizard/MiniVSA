using FluentValidation;
using MiniVSA.CatalogService.Application.Models.Common;
using MiniVSA.CatalogService.Application.Utilities;

namespace MiniVSA.CatalogService.Application.Validators
{
    public abstract class BaseAbstractValidator<TModel> : AbstractValidator<TModel>
    {
        // File Validation
        protected void RuleForFileRequired(Func<TModel, FileUploadModel> selector, string message)
        {
            RuleFor(model => selector(model).Base64File)
                .NotNull().WithMessage(message)
                .Must(file => file.Any()).WithMessage(message);
        }

        protected void RuleForFileSize(Func<TModel, FileUploadModel> selector, double maxValue, string message)
        {
            RuleFor(x => selector(x).Size)
                .LessThanOrEqualTo(maxValue).WithMessage(message);
        }

        protected void RuleForFileAsImage(Func<TModel, FileUploadModel> selector, string message)
        {
            RuleFor(model => selector(model).Base64File)
                .Must(ValidateFileAsImage).WithMessage(message);
        }

        #region Private Methods

        // File Validation
        private static bool ValidateFileAsImage(byte[] base64File)
        {
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            var mimeType = FileHelper.GetFileAttributesFromByteArray(base64File).MimeType;
            return allowedMimeTypes.Contains(mimeType);
        }

        #endregion
    }
}
