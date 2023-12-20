using AutoMapper;
using BankingAPI.DTOs;
using BankingAPI.Models;

namespace BankingAPI.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegisterAccountDto, Account>().ReverseMap();
            CreateMap<UpdateAccountDto, Account>().ReverseMap();
            CreateMap<GetAccountDto, Account>().ReverseMap();
            CreateMap<CreateTransactionDto, Transaction>().ReverseMap();
        }
    }
}
