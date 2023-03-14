using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Services;

namespace OnlineShop.WebApi.Controllers;


[ApiController]
[Route("carts")]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;
    

    public CartController(CartService cartService, CancellationToken cancellationToken)
    {
        _cartService = cartService;
    }
    [Authorize]
    [HttpGet("get_cart")]
    public Task<Cart> GetCart(Guid accountId, CancellationToken cancellationToken)
    {
        return _cartService.GetCartForAccount(accountId);
    }
}