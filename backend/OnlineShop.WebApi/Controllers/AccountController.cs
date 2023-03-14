using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Services;
using OnlineShop.Models.Requests;
using OnlineShop.Models.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]

    [Route("Accounts")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DuplicationOfAccountException"></exception>//Exeption, если аккаунт уже зарегистрирован.
        ///

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(
            RegisterRequest request, CancellationToken cancellationToken){
        
                var (account, token) =
                    await _accountService.Register(
                        request.Name, request.Email, request.PasswordHash, cancellationToken);
                //var token = _tokenSevice.GenerateToken(account);
                return new RegisterResponse(account.Id, account.Name, account.Email, token);
           
        }

        [HttpPost("lo_gin")]
        public async Task<ActionResult<LogInResponse>> Login(LogInRequest request,
            CancellationToken cancellationToken = default)
        {
          
                var (account, token) =
                    await _accountService.LogIn(
                        request.Email, request.PasswordHash, cancellationToken);
                return new LogInResponse(account.Name, account.Email, token);
           
        }


        [HttpGet("get_all")]
        public async Task<IEnumerable<Account>> GetAccounts(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAccounts(cancellationToken);
            return accounts;
        }
        
        [Authorize]
        [HttpGet("get_current")]
        public async Task<ActionResult<Account>> GetCurrentAccount()
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (strId is null)
            {
                Unauthorized();
            }
            var userId = Guid.Parse(strId);
            Account account = await _accountService.GetAccount(userId);
            if (account == null) throw new ArgumentNullException(nameof(account));
            return account;

        }


//     [HttpGet("get_by_id/{id:guid}")]
//     public async Task<Account> GetAccountById(Guid id, CancellationToken cancellationToken)
//     {
//         var account = await _accountRepository.GetById(id, cancellationToken); 
//         return account;
//     }
//
//     [HttpPost("add")]
//     public async Task AddAccount(Account account, CancellationToken cancellationToken)
//     {
//         if (account == null) throw new ArgumentNullException(nameof(account));
//         await _accountRepository.Add(account, cancellationToken);
//     }
//
//     [HttpPut("update")]
//     public async Task UpdateAccount(Account account, CancellationToken cancellationToken)
//     {
//         if (account == null) throw new ArgumentNullException(nameof(account));
//         await _accountRepository.Update(account, cancellationToken);
//     }
//
//     [HttpDelete("delete/{id:guid}")]
//     public async Task DeleteAccount(Guid id, CancellationToken cancellationToken)
//     {
//         await _accountRepository.Delete(id, cancellationToken);
//     }
//     [HttpGet("get_by_email/{email}")]
//     public async Task<Account> GetAccountByEmail(string email,CancellationToken cancellationToken)
//         => await _accountRepository.GetByEmail(email,cancellationToken);
//     
//
//     
    }
}