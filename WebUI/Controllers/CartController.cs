using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;
using System.Transactions;

namespace WebUI.Controllers;

[Authorize]
public class CartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailService _orderDetailsService;

    public CartController(
        IShoppingCartService shoppingCartService,
        UserManager<AppUser> userManager,
        IOrderHeaderService orderHeaderService,
        IOrderDetailService orderDetailsService) : base(userManager: userManager)
    {
        _shoppingCartService = shoppingCartService;
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
    }

    #region Shopping Cart List 
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var shoppingCartListResult = await _shoppingCartService.GetAllShoppingCartsWithProductAsync(s => s.AppUserId == userId);
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
    #endregion

    #region Plus Minus Number of Product 
    [HttpPost]
    public async Task<IActionResult> UpdateProductCount(List<UpdatedCartItem> updatedCarts, string? redirectTo)
    {
        var errors = new List<string>();
        TempData["ErrorMessage"] = errors;

        foreach (var updatedCart in updatedCarts)
        {
            var updateResult = await _shoppingCartService.UpdateShoppingCartCountAsync(updatedCart);
            if (!updateResult.Success)
            {
                errors.Add(updateResult.Message);
            }
        }

        if (redirectTo is "summary")
        {
            return Json(new { redirectTo = Url.Action(nameof(Summary)) });
        }

        return Json(new { redirectTo = Url.Action(action: "ProductList", controller: "Home") });
    }
    #endregion

    #region Delete Product From Shopping Cart 
    public async Task<IActionResult> Delete(Guid cartId)
    {
        var cartFromDbResult = await _shoppingCartService.GetShoppingCartByIdAsync(cartId);
        if (!cartFromDbResult.Success)
        {
            TempData["ErrorMessage"] = cartFromDbResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var deleteResult = await _shoppingCartService.DeleteShoppingCartAsync(cartId);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
        }

        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Order Summary  
    public async Task<IActionResult> Summary()
    {
        Guid userId = GetUserId();
        var shoppingCartVM = new ShoppingCartViewModel()
        {
            ShoppingCartList = (await _shoppingCartService.GetAllShoppingCartsWithProductAsync(s => s.AppUserId == userId)).Data.ToList(),
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
        shoppingCartViewModel.ShoppingCartList = (await _shoppingCartService.GetAllShoppingCartsWithProductAsync(s => s.AppUserId == userId)).Data;
        shoppingCartViewModel.OrderHeader.OrderDate = DateTime.UtcNow;
        shoppingCartViewModel.OrderHeader.ShippingDate = DateTime.UtcNow.AddDays(2);
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

        shoppingCartViewModel.OrderHeader.PaymentStatus = OrderHeaderStatus.Pending;
        shoppingCartViewModel.OrderHeader.OrderStatus = OrderHeaderStatus.Pending;

        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        var headerAddResult = await _orderHeaderService.CreateOrderHeaderAsync(shoppingCartViewModel.OrderHeader);
        if (!headerAddResult.Success)
        {
            TempData["ErrorMessage"] = headerAddResult.Message;
            return RedirectToAction(nameof(Index));
        }

        foreach (var cart in shoppingCartViewModel.ShoppingCartList)
        {
            OrderDetail orderDetails = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };

            var detailsAddResult = await _orderDetailsService.CreateOrderDetailAsync(orderDetails);
            if (!detailsAddResult.Success)
            {
                TempData["ErrorMessage"] = detailsAddResult.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        transactionScope.Complete();
        return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartViewModel.OrderHeader.Id });
    }
    #endregion

    #region Order Confirmation 
    public async Task<IActionResult> OrderConfirmation(Guid id)
    {
        var orderHeaderList = await _orderHeaderService.GetAllOrderHeadersWithAppUserAsync(o => o.Id == id);
        if (orderHeaderList.Data == null)
        {
            TempData["ErrorMessage"] = orderHeaderList.Message;
            return RedirectToAction(nameof(Summary));
        }

        var orderHeader = orderHeaderList.Data.First();
        // TODO:Send Email about Order Confirmed

        var shoppingCartlist = (await _shoppingCartService.GetAllShoppingCartsAsync(u => u.AppUserId == orderHeader.AppUserId)).Data;
        if (shoppingCartlist == null)
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        var deleteResult = await _shoppingCartService.DeleteShoppingCartRangeAsync(shoppingCartlist);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(id);
    }
    #endregion

    #region Helper Methods
    private static void SetOrderHeader(AppUser appUser, ShoppingCartViewModel shoppingCartViewModel)
    {
        shoppingCartViewModel.OrderHeader.AppUser = appUser;
        shoppingCartViewModel.OrderHeader.Name = appUser.Name;
        shoppingCartViewModel.OrderHeader.PhoneNumber = appUser.PhoneNumber;
        shoppingCartViewModel.OrderHeader.Address = appUser.Address;
        shoppingCartViewModel.OrderHeader.City = appUser.City;
        shoppingCartViewModel.OrderHeader.Country = appUser.Country;
        shoppingCartViewModel.OrderHeader.PostalCode = appUser.PostalCode;
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

