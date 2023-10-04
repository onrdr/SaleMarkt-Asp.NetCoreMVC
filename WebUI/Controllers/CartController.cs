using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;
using System.Security.Claims;
using System.Transactions;

namespace WebUI.Controllers;

[Authorize]
public class CartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailsService _orderDetailsService;

    public CartController(
        IShoppingCartService shoppingCartService,
        UserManager<AppUser> userManager,
        IOrderHeaderService orderHeaderService,
        IOrderDetailsService orderDetailsService) : base(userManager: userManager)
    {
        _shoppingCartService = shoppingCartService;
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var shoppingCartListResult = await _shoppingCartService.GetAllWithProductAsync(s => s.AppUserId == userId);
        if (!shoppingCartListResult.Success)
        {
            TempData["ErrorMessage"] = shoppingCartListResult.Message;
            return RedirectToAction(nameof(Index), "Home");
        }

        var ShoppingCartVM = new ShoppingCartViewModel()
        {
            ShoppingCartList = shoppingCartListResult.Data,
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
        Guid userId = GetUserId();
        var shoppingCartVM = new ShoppingCartViewModel()
        {
            ShoppingCartList = (await _shoppingCartService.GetAllWithProductAsync(s => s.AppUserId == userId)).Data.ToList(),
            OrderHeader = new()
        };

        var appUser = await UserManager.FindByIdAsync(userId.ToString());
        if (appUser == null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return RedirectToAction(nameof(Index));
        }

        SetOrderHeader(appUser, shoppingCartVM);
        foreach (var cart in shoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            shoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
        }

        return View(shoppingCartVM);
    }

    [HttpPost]
    [ActionName("Summary")]
    public async Task<IActionResult> SummaryPOST(ShoppingCartViewModel shoppingCartViewModel)
    {
        var userId = GetUserId();
        shoppingCartViewModel.ShoppingCartList = (await _shoppingCartService.GetAllWithProductAsync(s => s.AppUserId == userId)).Data;
        shoppingCartViewModel.OrderHeader.OrderDate = DateTime.UtcNow;
        shoppingCartViewModel.OrderHeader.AppUserId = userId;

        var appUser = await UserManager.FindByIdAsync(userId.ToString());
        if (appUser == null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return RedirectToAction(nameof(Index));
        }
        shoppingCartViewModel.OrderHeader.AppUser = appUser;

        foreach (var cart in shoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            shoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        shoppingCartViewModel.OrderHeader.PaymentStatus = "Pending";
        shoppingCartViewModel.OrderHeader.OrderStatus = "Approved";

        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        var headerAddResult = await _orderHeaderService.CreateOrderHeader(shoppingCartViewModel.OrderHeader);
        if (!headerAddResult.Success)
        {
            TempData["ErrorMessage"] = headerAddResult.Message;
            return RedirectToAction(nameof(Index));
        }

        foreach (var cart in shoppingCartViewModel.ShoppingCartList)
        {
            OrderDetails orderDetails = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };

            var detailsAddResult = await _orderDetailsService.CreateOrderDetails(orderDetails);
            if (!detailsAddResult.Success)
            {
                TempData["ErrorMessage"] = detailsAddResult.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        transactionScope.Complete();
        return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartViewModel.OrderHeader.Id });
    }

    public async Task<IActionResult> OrderConfirmation(Guid id)
    {
        var orderHeaderList = await _orderHeaderService.GetAllWithAppUserAsync(o => o.Id == id);
        if (orderHeaderList.Data == null)
        {
            TempData["ErrorMessage"] = orderHeaderList.Message;
            return RedirectToAction(nameof(Summary));
        }

        var orderHeader = orderHeaderList.Data.First();
        // TODO:Send Email about Order Confirmed

        var shoppingCartlist = (await _shoppingCartService.GetAllAsync(u => u.AppUserId == orderHeader.AppUserId)).Data;
        if (shoppingCartlist == null)
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        var deleteResult = await _shoppingCartService.DeleteShoppingCartRange(shoppingCartlist);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(id);
    }

    #region Private Helper Methods
    private static void SetOrderHeader(AppUser? appUser, ShoppingCartViewModel shoppingCartViewModel)
    {
        shoppingCartViewModel.OrderHeader.AppUser = appUser;
        shoppingCartViewModel.OrderHeader.Name = appUser.Name;
        shoppingCartViewModel.OrderHeader.PhoneNumber = appUser.PhoneNumber;
        shoppingCartViewModel.OrderHeader.Address = appUser.Address;
        shoppingCartViewModel.OrderHeader.City = appUser.City;
        shoppingCartViewModel.OrderHeader.Country = appUser.Country;
        shoppingCartViewModel.OrderHeader.PostalCode = appUser.PostalCode;
    }

    private Guid GetUserId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        return userId;
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
    #endregion
}
