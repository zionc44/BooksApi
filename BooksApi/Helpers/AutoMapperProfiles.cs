using AutoMapper;
using BooksApi.DTOs;
using BooksApi.Models;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<GridFSFileInfo, DocumentInfo>()
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentFileName, opt => opt.MapFrom(src => src.Filename))
                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.Metadata.GetValue(4)))
                .ForMember(dest => dest.FormFileName, opt => opt.MapFrom(src => src.Metadata.GetValue(3)))
                .ForMember(dest => dest.DocumentFileType, opt => opt.MapFrom(src => src.Metadata.GetValue(0)));
        }

    }
}
//.ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(
//    src => src.Photos.FirstOrDefault(x => x.IsMain).Url))

//DocumentId = fileInfo.Id.ToString(),
//DocumentFileName = fileInfo.Filename,
//DocumentFileType = (string)fileInfo.Metadata.GetValue(0),
//DocumentName = (string)fileInfo.Metadata.GetValue(4),
//FormFileName = (string)fileInfo.Metadata.GetValue(3)