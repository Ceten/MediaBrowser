﻿using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using System;
using System.Linq;

namespace MediaBrowser.Api
{
    public interface IHasDtoOptions : IHasItemFields
    {
        bool? EnableImages { get; set; }

        int? ImageTypeLimit { get; set; }

        string EnableImageTypes { get; set; }
    }

    public static class HasDtoOptionsExtensions
    {
        public static DtoOptions GetDtoOptions(this IHasDtoOptions request)
        {
            var options = new DtoOptions();

            options.Fields = request.GetItemFields().ToList();
            options.EnableImages = request.EnableImages ?? true;

            if (request.ImageTypeLimit.HasValue)
            {
                options.ImageTypeLimit = request.ImageTypeLimit.Value;
            }

            if (string.IsNullOrWhiteSpace(request.EnableImageTypes))
            {
                if (options.EnableImages)
                {
                    // Get everything
                    options.ImageTypes = Enum.GetNames(typeof(ImageType))
                        .Select(i => (ImageType)Enum.Parse(typeof(ImageType), i, true))
                        .ToList();
                }
            }
            else
            {
                options.ImageTypes = (request.EnableImageTypes ?? string.Empty).Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).Select(v => (ImageType)Enum.Parse(typeof(ImageType), v, true)).ToList();
            }

            return options;
        }
    }
}
