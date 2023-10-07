using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Security.Claims;

namespace WebUI.Controllers;
public class OrderController : Controller
{
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailService _orderDetailsService;

    public OrderController(IOrderHeaderService orderHeaderService, IOrderDetailService orderDetailsService)
    {
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details(Guid orderHeaderId)
    {
        var orderHeaderResult = await _orderHeaderService.GetByIdWithAppUserAsync(orderHeaderId);
        if (!orderHeaderResult.Success)
        {
            TempData["ErrorMessage"] = orderHeaderResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var orderDetailResult = await _orderDetailsService.GetAllWithProductAsync(o => o.OrderHeaderId == orderHeaderId);
        if (!orderDetailResult.Success)
        {
            TempData["ErrorMessage"] = orderDetailResult.Message;
            return RedirectToAction(nameof(Index));
        }

        var orderVM = new OrderViewModel()
        {
            OrderHeader = orderHeaderResult.Data,
            OrderDetailList = orderDetailResult.Data
        };

        return View(orderVM);
    }

    public async Task<IActionResult> ConfirmOrder(Guid orderHeaderId)
    {
        var result = await _orderHeaderService.UpdateOrderStatus(orderHeaderId, OrderHeaderStatus.Approved);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { orderHeaderId });
        }

        TempData["SuccessMessage"] = "Order Confirmed";
        return RedirectToAction(nameof(Details), new { orderHeaderId });
    }

    public async Task<IActionResult> ConfirmPayment(Guid orderHeaderId)
    {
        var result = await _orderHeaderService.UpdatePaymentStatus(orderHeaderId, OrderHeaderStatus.Approved);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { orderHeaderId });
        }

        TempData["SuccessMessage"] = "Payment Confirmed";
        return RedirectToAction(nameof(Details), new { orderHeaderId });
    }
    public async Task<IActionResult> CancelOrder(Guid orderHeaderId)
    {
        var result = await _orderHeaderService.UpdateOrderStatus(orderHeaderId, OrderHeaderStatus.Pending);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { orderHeaderId });
        }

        TempData["SuccessMessage"] = "Order Status Updated to Pending";
        return RedirectToAction(nameof(Details), new { orderHeaderId });
    }

    public async Task<IActionResult> CancelPayment(Guid orderHeaderId)
    {
        var result = await _orderHeaderService.UpdatePaymentStatus(orderHeaderId, OrderHeaderStatus.Pending);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { orderHeaderId });
        }

        TempData["SuccessMessage"] = "Payment Status Updated to Pending";
        return RedirectToAction(nameof(Details), new { orderHeaderId });
    }

    #region API Calls

    [HttpGet]
    public async Task<IActionResult> GetAll()
    { 
        var orderHeaderResult = await _orderHeaderService.GetAllWithAppUserAsync(o => true);
        if (!orderHeaderResult.Success)
        {
            TempData["ErrorMessage"] = orderHeaderResult.Message;
        }

        if (User.IsInRole(RoleNames.Customer))
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(new { data = orderHeaderResult.Data.Where(o => o.AppUserId == userId) });
        }

        return Json(new { data = orderHeaderResult.Data });
    }
    #endregion 
}
