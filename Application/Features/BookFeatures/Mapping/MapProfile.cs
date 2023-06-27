using Application.Features.BookFeatures.Commands;
using Application.Features.BookFeatures.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Features.BookFeatures.Mapping;

internal class MapProfile : Profile
{
	public MapProfile()
	{
		CreateMap<Book, BookDto>()
            .ForMember(dis => dis.Title, opt => opt.MapFrom(src => src.Title.Value));
		CreateMap<BookCreateCommand, Book>()
			.ForMember(dis => dis.Title, opt => opt.MapFrom(src => BookTitle.Create(src.Title).Value));
        CreateMap<BookTranslation, BookTranslationDto>().ReverseMap();
    }
}
