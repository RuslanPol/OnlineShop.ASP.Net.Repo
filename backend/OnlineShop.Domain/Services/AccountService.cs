using OnlineShop.Data;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;


namespace OnlineShop.Domain.Services
{
    /// <summary>
    /// <exception cref="DuplicationOfAccountException">
    /// </exception>//Exeption, если аккаунт уже зарегистрирован.
    /// </summary>
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenService _tokenSevice;
        private readonly IUnitOfWork _uow;

        public AccountService(IAccountRepository accountRepository,
            IPasswordHasherService passwordHasherService,ITokenService tokenSevice,IUnitOfWork uow)

        {
            _accountRepository = accountRepository;
            _passwordHasherService = passwordHasherService;
            _tokenSevice = tokenSevice;
            _uow = uow;
        }

        public virtual async Task<(Account account,string token)> Register(
            string name, string email, string password, CancellationToken cancellationToken)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            Account? existedAccount = await _accountRepository.FindByEmail(email, cancellationToken);

            if (existedAccount is not null)
            {
                throw new DuplicationOfAccountException();
            }

            var hashedPassword = _passwordHasherService.HashPassword(password);
            var account = new Account(Guid.NewGuid(), name, email, hashedPassword);
            var cart = new Cart( ){Id=Guid.NewGuid(), AccountId=account.Id};
            if (cart == null) throw new ArgumentNullException(nameof(cart));
            await _uow.AccountRepository.Add(account, cancellationToken);
            await _uow.CartRepository.Add(cart, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            
            var token = _tokenSevice.GenerateToken(account);
            return (account,token);
        }

        public virtual async Task<(Account account,string token)> LogIn(
            string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));
            var account = await _uow.AccountRepository.FindByEmail (email, cancellationToken);
            if (account is null)
            {
                throw new EmailNotFoundExeption(email);
            }
            var result = _passwordHasherService.VerifyPassword(account.PasswordHash, password);
            if (!result)
            {
                throw new IncorrectPasswordExeption();
            }
            else
            {
                var token = _tokenSevice.GenerateToken(account);
                return (account,token);
            
            }
        }
    
        public async Task<IEnumerable<Account>> GetAccounts(CancellationToken cancellationToken)
        {
            var accounts=await _uow.AccountRepository.GetAll(cancellationToken);
            return accounts;
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            return await _uow.AccountRepository.GetById(accountId);
        }
    }

    public class IncorrectPasswordExeption : Exception
    {
    }

    public class EmailNotFoundExeption : Exception
    {
        public EmailNotFoundExeption(string email) : base(email)
        {
        }
    }

    public class DuplicationOfAccountException : Exception
    {
        public DuplicationOfAccountException(string? message = null) : base(message)
        {
        }
    }
}