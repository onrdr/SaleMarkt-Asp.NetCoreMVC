using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.ViewComponents;

public class ShoppingCartCountViewComponent : ViewComponent
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartCountViewComponent(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    { 
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is null)
        {
            return View(0);
        }

        int count = await _shoppingCartService.GetItemCountForUserAsync(Guid.Parse(claim.Value));

        return View(count);
    }
}
