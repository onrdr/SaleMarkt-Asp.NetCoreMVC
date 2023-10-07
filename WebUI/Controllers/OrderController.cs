﻿using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Models.ViewModels; 

namespace WebUI.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderHeaderService _orderHeaderService;
    private readonly IOrderDetailService _orderDetailsService;

    public OrderController(IOrderHeaderService orderHeaderService, IOrderDetailService orderDetailsService)
    {
        _orderHeaderService = orderHeaderService;
        _orderDetailsService = orderDetailsService;
    }

    #region Order List 
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    #region Order Details 
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
    #endregion

    #region Order Confirm & Cancel 
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
    #endregion

    #region Payment Confirm & Cancel 
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
    #endregion

    #region Complete Order 
    public async Task<IActionResult> CompleteOrder(Guid orderHeaderId)
    {
        var result = await _orderHeaderService.CompleteOrder(orderHeaderId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { orderHeaderId });
        }

        TempData["SuccessMessage"] = "Order Completed!!!";
        return RedirectToAction(nameof(Details), new { orderHeaderId });
    }
    #endregion

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
            return Json(new { data = orderHeaderResult.Data.Where(o => o.AppUserId == GetUserId()) });
        }

        return Json(new { data = orderHeaderResult.Data });
    }
    #endregion 
}
