using OnlineShop.Domain;
using OnlineShop.Domain.Entities;
using static System.Guid;

namespace OnlineShop.Domain.Test;

public class CartTest
{
    [Fact]
    public void New_cart_is_empty()
    { 
        //Arrange
        var cart = new Cart()
        {
            Id = Guid.Empty,
            AccountId = Guid.Empty,
            Items = new List<Cart.CartItem>()
        };
        //Act and Assert
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void New_item_is_addit_to_cart()
    {
        //Arrange
        var cart = new Cart()
        {
            Id = Guid.Empty,
            AccountId = Guid.Empty,
            Items = new List<Cart.CartItem>()
        };
        var product = new Product(Guid.NewGuid(), "Собачье сердце", 500);
        var quantity = 1d;
        //Act
        cart.Add(product, 1);
        //Assert 
        Assert.Single(cart.Items);
        Cart.CartItem item = cart.Items.First();
        Assert.NotNull(item);
        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(product.Price, item.Price);
    }

    [Fact]
    public void Adding_empty_item_in_cart_is_impossible()
    {
        //Arrange
        var cart = new Cart()
        {
            Id = Guid.Empty,
            AccountId = Guid.Empty,
            Items = new List<Cart.CartItem>()
        };
        //Act and Assert
        Assert.Throws<ArgumentNullException>(() => cart.Add(null));
    }

    [Fact]
    public void Adding_existed_product_to_cart_changes_item_quantity()
    {
        //Arrange
        var cart = new Cart()
        {
            Id = Guid.Empty,
            AccountId = Guid.Empty,
            Items = new List<Cart.CartItem>()
        };
        var product = new Product(Guid.NewGuid(), "Собачье сердце", 500);
        var quantity = 2d;
        //Act
        cart.Add(product, 1);
        cart.Add(product, 1);
        //Assert
        Cart.CartItem item = cart.Items.First();
        Assert.NotNull(item);
        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(product.Price, item.Price);
    }

    [Fact]
    public void Adding_more_than_a_thousand_items_is_not_possible()
    {
        var cart = new Cart()
        {
            Id = Guid.Empty,
            AccountId = Guid.Empty,
            Items = new List<Cart.CartItem>()
        };
        var product = new Product(Guid.NewGuid(), "Собачье сердце", 500);
        var quantity = 1000;
        for (int i = 0; i < quantity; i++)
        {
            cart.Add(product, 1);
        }

        Assert.Throws<InvalidOperationException>(() => cart.Add(product, 1));
    }
}