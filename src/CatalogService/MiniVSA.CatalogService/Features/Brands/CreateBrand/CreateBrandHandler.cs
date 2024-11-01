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
using File = MiniVSA.CatalogService.Domain.Entities.File;

namespace MiniVSA.CatalogService.Features.Brands.CreateBrand
{
    public record CreateBrandCommand(string Name,
                                     FileUploadRequestModel Image) : ICommand;

    public class CreateBrandCommandValidator : BaseAbstractValidator<CreateBrandCommand>
    {
        private const double MAX_FILE_SIZE = 10.0;
        private const int MAX_BRAND_NAME_LENGTH = 50;
        public CreateBrandCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage(ValidationMessageConstants.Brand.BrandNameRequired)
                .MaximumLength(MAX_BRAND_NAME_LENGTH).WithMessage(ValidationMessageConstants.Brand.BrandNameMaxLength(MAX_BRAND_NAME_LENGTH));

            RuleForFileRequired(command => command.Image, ValidationMessageConstants.Brand.BrandImageRequired);
            RuleForFileSize(command => command.Image, MAX_FILE_SIZE, ValidationMessageConstants.Brand.BrandImageMaxSize(MAX_FILE_SIZE));
            RuleForFileAsImage(command => command.Image, ValidationMessageConstants.Brand.BrandFileAsImage);
        }
    }

    public class CreateBrandHandler(IDocumentSession documentSession) : ICommandHandler<CreateBrandCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            var isExistBrand = await documentSession.Query<Brand>()
                                              .AnyAsync(brand => brand.Name == command.Name, cancellationToken);

            if (isExistBrand)
                return Result<Unit>.Error(ResponseMessageConstants.Brand.Error.BrandAlreadyExists, (int)HttpStatusCode.BadRequest);

            (string FileName, string FilePath) fileData; 
            try
            {
                fileData = await FileHelper.UploadFileToLocalAsync(command.Image.Base64File, FileType.Image, FilePathConstants.BrandFilePaths.Image);
            }
            catch (Exception)
            {
                return Result<Unit>.Error(ResponseMessageConstants.Brand.Error.BrandImageUploadError, (int)HttpStatusCode.InternalServerError);
            }

            Brand brand = new()
            {
                Name = command.Name,
                Files =
                [
                    new File
                    {
                        Id = Guid.NewGuid(),
                        Name = fileData.FileName,
                        FileType = FileType.Image,
                        Size = command.Image.Size,
                        CreatedDate = DateTimeOffset.Now,
                        Path = fileData.FilePath
                    }
                ]
            };

            documentSession.Insert(brand);
            await documentSession.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(ResponseMessageConstants.Brand.Success.BrandCreated, (int)HttpStatusCode.Created);
        }
    }
}
