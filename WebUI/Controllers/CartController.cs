using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;
using System.Security.Claims;

namespace WebUI.Controllers;

[Authorize]
public class CartController : BaseController
{
    public ShoppingCartViewModel ShoppingCartVM { get; set; }
    private readonly IShoppingCartService _shoppingCartService;

    public CartController(
        IShoppingCartService shoppingCartService,
        UserManager<AppUser> userManager) : base(userManager: userManager)
    {
        _shoppingCartService = shoppingCartService;
        ShoppingCartVM = new ShoppingCartViewModel();
    }

    public async Task<IActionResult> Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

        var shoppingCartList = (await _shoppingCartService.GetAllWithProductAsync(s => s.AppUserId == userId)).Data;
        ShoppingCartVM = new()
        {
            ShoppingCartList = shoppingCartList,
            OrderHeader = new()
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            double price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += price * cart.Count;
        }

        return View(ShoppingCartVM);
    }

    public async Task<IActionResult> PlusMinus(Guid cartId, int change)
    {
        var cartFromDbResult = await _shoppingCartService.GetByIdAsync(cartId);
        if (!cartFromDbResult.Success)
        {
            TempData["ErrorMessage"] = cartFromDbResult.Message;
            return RedirectToAction(nameof(Index));
        }

        cartFromDbResult.Data.Count += change;

        var updateResult = await _shoppingCartService.UpdateShoppingCart(cartFromDbResult.Data);
        if (!updateResult.Success)
        {
            TempData["ErrorMessage"] = updateResult.Message;
        }

        return RedirectToAction(nameof(Index));
    } 

    public async Task<IActionResult> Delete(Guid cartId)
    {
        var cartFromDbResult = await _shoppingCartService.GetByIdAsync(cartId);
        if (!cartFromDbResult.Success)
        {
            TempData["ErrorMessage"] = cartFromDbResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var deleteResult = await _shoppingCartService.DeleteShoppingCart(cartId);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
        }
         
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

        ShoppingCartVM = new()
        {
            ShoppingCartList = (await _shoppingCartService.GetAllWithProductAsync(s => s.AppUserId == userId)).Data,
            OrderHeader = new()
        };

        var appUser = await UserManager.FindByIdAsync(userId.ToString());
        if (appUser == null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound; 
            return RedirectToAction(nameof(Index));
        }

        ShoppingCartVM.OrderHeader.AppUser = appUser;

        ShoppingCartVM.OrderHeader.Name = appUser.Name;
        ShoppingCartVM.OrderHeader.PhoneNumber = appUser.PhoneNumber;
        ShoppingCartVM.OrderHeader.StreetAddress = appUser.Address;
        ShoppingCartVM.OrderHeader.City = appUser.City;
        ShoppingCartVM.OrderHeader.State = appUser.Country;
        ShoppingCartVM.OrderHeader.PostalCode = appUser.PostalCode;


        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
        }
        return View(ShoppingCartVM);
    }

    private static double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
        {
            shoppingCart.Price = shoppingCart.Product.Price;
            return shoppingCart.Product.Price;
        }

        else if (shoppingCart.Count <= 100)
        {
            shoppingCart.Price = shoppingCart.Product.Price50;
            return shoppingCart.Product.Price50;
        }

        shoppingCart.Price = shoppingCart.Product.Price100;
        return shoppingCart.Product.Price100;
    }
}
