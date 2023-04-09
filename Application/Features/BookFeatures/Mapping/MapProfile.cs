using Application.Features.BookFeatures.Commands;
using Application.Features.BookFeatures.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.BookFeatures.Mapping;

internal class MapProfile : Profile
{
	public MapProfile()
	{
		CreateMap<Book, BookDto>();
		CreateMap<BookCreateCommand, Book>();
        CreateMap<BookTranslation, BookTranslationDto>().ReverseMap();
    }
}
