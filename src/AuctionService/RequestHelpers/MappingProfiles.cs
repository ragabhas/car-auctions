using System;
using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<AuctionDto, Auction>()
        .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
        CreateMap<AuctionDto, Item>();

        CreateMap<CreateAuctionDto, Auction>()
        .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));
        CreateMap<CreateAuctionDto, Item>();
    }
}
