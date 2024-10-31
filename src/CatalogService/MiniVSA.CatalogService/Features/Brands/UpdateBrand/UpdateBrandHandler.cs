using FluentValidation;
using Marten;
using MediatR;
using MiniVSA.CatalogService.Application.Constants;
using MiniVSA.CatalogService.Application.Interfaces.CQRS.Command;
using MiniVSA.CatalogService.Application.Models.Common.Request;
using MiniVSA.CatalogService.Application.Models.Result;
using MiniVSA.CatalogService.Application.Utilities;
using MiniVSA.CatalogService.Application.Validators;
using MiniVSA.CatalogService.Domain.Entities;
using MiniVSA.CatalogService.Domain.Enums;
using System.Net;

namespace MiniVSA.CatalogService.Features.Brands.UpdateBrand
{
    public record UpdateBrandCommand(string? Name,
                                     FileUploadRequestModel? Image) : ICommand
    {
        public Guid Id { get; private set; }

        public UpdateBrandCommand Build(Guid id)
            => this with { Id = id };
    }

    public class UpdateBrandCommandValidator : BaseAbstractValidator<UpdateBrandCommand>
    {
        private const double MAX_FILE_SIZE = 10.0;
        private const int MAX_BRAND_NAME_LENGTH = 50;
        public UpdateBrandCommandValidator()
        {
            RuleFor(command => command.Name)
                .MaximumLength(MAX_BRAND_NAME_LENGTH)
                .WithMessage(ValidationMessageConstants.Brand.BrandNameMaxLength(MAX_BRAND_NAME_LENGTH))
                .When(command => !string.IsNullOrWhiteSpace(command.Name));

            RuleForFileSize(command => command.Image, MAX_FILE_SIZE, ValidationMessageConstants.Brand.BrandImageMaxSize(MAX_FILE_SIZE));
            RuleForFileAsImage(command => command.Image, ValidationMessageConstants.Brand.BrandFileAsImage);
        }
    }

    public class UpdateBrandCommandHandler(IDocumentSession documentSession) : ICommandHandler<UpdateBrandCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = await documentSession.LoadAsync<Brand>(command.Id, token: cancellationToken);

            if (brand is null || brand.Deleted)
                return Result<Unit>.Error(ResponseMessageConstants.Brand.Error.BrandNotFoundForDelete, (int)HttpStatusCode.NotFound);

            if (!string.IsNullOrWhiteSpace(command.Name))
                brand.Name = command.Name;

            if (IsValidForImageUpdate(command.Image))
            {
                try
                {
                    (string FileName, string FilePath) = await FileHelper.UploadFileToLocalAsync(command.Image.Base64File, FileType.Image, FilePathConstants.BrandFilePaths.Image);
                    
                    var brandFile = brand.Files
                                 .First(file => file.FileType == FileType.Image);

                    brandFile.Name = FileName;
                    brandFile.Path = FilePath;
                    brandFile.Size = FileHelper.GetFileSizeAsMB(command.Image.Base64File);
                }
                catch (Exception)
                {
                    return Result<Unit>.Error(ResponseMessageConstants.Brand.Error.BrandImageUploadError, (int)HttpStatusCode.InternalServerError);
                }
            }

            documentSession.Store(brand);
            await documentSession.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(ResponseMessageConstants.Brand.Success.BrandUpdated, (int)HttpStatusCode.NoContent);
        }

        private static bool IsValidForImageUpdate(FileUploadRequestModel image)
         => image is not null && 
            image.Base64File is not null && 
            image.Base64File.Any();
    }
}
