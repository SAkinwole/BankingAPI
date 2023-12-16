using AutoMapper;
using BankingAPI.DTOs;
using BankingAPI.Models;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/v3/[controller]")]
    public class AccountController : ControllerBase
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
            var result = _service.Create(newAccount, request.Pin, request.ConfirmPin);
            return Ok(result);

        }

        [HttpGet]
        [Route("get-all-accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _service.GetAllAccount();
            var allAccounts = _mapper.Map<IList<GetAccountDto>>(accounts);
            return Ok(allAccounts);
        }

        [HttpPost]
        [Route("Authenticate-account")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            return Ok(_service.Authenticate(model.AccountNumber, model.Pin));
        }

        [HttpGet]
        [Route("get-by-account-number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            //try
            //{
            //    if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{0}$")) return BadRequest();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            var account = _service.GetByAccountNumber(AccountNumber);
            var result = _mapper.Map<GetAccountDto>(account);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-by-account-Id")]
        public IActionResult GetAccountById(int Id)
        {
            var account = _service.GetById(Id);
            var result = _mapper.Map<GetAccountDto>(account);
            return Ok(result);
        }

        [HttpPut]
        [Route("update-account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountDto model)
        {
            if (!ModelState.IsValid) return BadRequest("model");

            var account = _mapper.Map<Account>(model);
            _service.Update(account, model.Pin);
            return Ok();
        }

    }
}
