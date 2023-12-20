using AutoMapper;
using BankingAPI.DTOs;
using BankingAPI.Models;
using BankingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/v3/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IMapper _mapper;
        public TransactionController(ITransactionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create-new-transaction")]
        public IActionResult CreateNewTransaction([FromBody] CreateTransactionDto request)
        {
            if (!ModelState.IsValid) return BadRequest(request);
            var transaction = _mapper.Map<Transaction>(request);

            return Ok(_service.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!ModelState.IsValid) return BadRequest();

           // if (!Regex.IsMatch(request.AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{0}$")) return BadRequest();

            return Ok(_service.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("withdraw")]
        public IActionResult MakeWithdraw(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!ModelState.IsValid) return BadRequest();

            //if (!Regex.IsMatch(request.AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{0}$")) return BadRequest();

            return Ok(_service.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("transfer-funds")]
        public IActionResult TransferFunds(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!ModelState.IsValid) return BadRequest();
            return Ok(_service.MakeFundsTransfer(FromAccount, ToAccount, Amount,TransactionPin));
        }
    }
}
