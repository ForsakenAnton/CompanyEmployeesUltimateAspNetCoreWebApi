﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Company, CompanyDto>()
        //    .ForMember(
        //        destinationMember: c => c.FullAddress,
        //        memberOptions: opt => opt
        //            .MapFrom(x => string.Join(' ', x.Address, x.Country)));

        CreateMap<Company, CompanyDto>()
            .ForCtorParam(
                ctorParamName: "FullAddress",
                paramOptions: opt => 
                    opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
    }
}

