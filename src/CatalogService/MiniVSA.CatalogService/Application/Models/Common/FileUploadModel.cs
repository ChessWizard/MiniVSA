﻿using MiniVSA.CatalogService.Application.Utilities;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MiniVSA.CatalogService.Application.Models.Common
{
    public class FileUploadModel
    {
        public byte[] Base64File { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        [SwaggerIgnore]
        public double Size => FileHelper.GetFileSizeAsMB(Base64File);
    }
}
