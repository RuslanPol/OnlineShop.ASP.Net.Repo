using Moq;
using Bogus;
using OnlineShop.Data;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;
using OnlineShop.Domain.Services;

namespace OnlineShop.Domain.Test;

public class AccountServiceTestMoq
{
    private readonly Faker _faker = new Faker();
    [Fact]
    private async void Register_new_user_succeeded()
    {
        //Arrange:Create Mocks
        var cartRepositoryMock = new Mock<ICartRepository>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var uowMock = new Mock<IUnitOfWork>();
        var passwordHasherMock = new Mock<IPasswordHasherService>();
        var tokenServiceMock = new Mock<ITokenService>();
        
        //Arrange
        //Настраиваем чтобы uow возвращал замоканные репозитории(подделки)
        uowMock.Setup(u => u.AccountRepository)
            .Returns(accountRepositoryMock.Object);
        uowMock.Setup(u => u.CartRepository).Returns(cartRepositoryMock.Object);
        //Настраивается  чтобы  HashPassword()) возвращал то, что ему передали
        passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>()))
            .Returns<string>(p => p);
        var accountService = new AccountService(accountRepositoryMock.Object,
            passwordHasherMock.Object, tokenServiceMock.Object, uowMock.Object);
        var account = await accountService.Register(_faker.Person.FullName, _faker.Person.Email,
            _faker.Internet.Password(), default);
        //Assert
        //1 проверить ,что в БД появился новый аккаунт
        //метод accountRepository.Add был вызван один раз и ему передали именно тот аккаунт
        accountRepositoryMock.Verify(
            x=>x.Add(It.Is<Account>(a=>a==account),default),
            Times.Once);
        //2 проверить ,что в БД появилась корзина
        //метод CartRepository.Add был вызван один раз и ему передали корзину с Id аккаунта, который вернул Register
        cartRepositoryMock.Verify(
            x=>x.Add(It.Is<Cart>(c=>c.AccountId==account.Id),default),
            Times.Once);
        //валидируем UnitOfWork
        uowMock.Verify(
            x=>x.CommitAsync(default),Times.AtLeastOnce);
            
    }
}