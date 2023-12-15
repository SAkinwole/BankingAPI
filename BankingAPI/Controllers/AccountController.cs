using AutoMapper;
using BankingAPI.DTOs;
using BankingAPI.Models;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [ApiController]
    public class AccountController
    {
        private readonly IAccountService _service;
        private readonly IMapper _mapper;
        public AccountController(IAccountService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("Create-new-account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterAccountDto request)
        {
            if (!ModelState.IsValid) return BadRequest(request);

            var newAccount = _mapper.Map<Account>(request);
            return Ok(_service.Create(newAccount, request.Pin, request.ConfirmPin));

        }

        [HttpGet]
        [Route("get-all-accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _service.GetAllAccount();
            var allAccounts = _mapper.Map<GetAccountDto>(accounts);
            return Ok(allAccounts);
        }

    }
}
