using Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Services.Concrete;

public class ViewRenderService : IViewRenderService
{
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly IServiceProvider _serviceProvider;

    public ViewRenderService(IRazorViewEngine razorViewEngine, IServiceProvider serviceProvider)
    {
        _razorViewEngine = razorViewEngine;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ActionDescriptor());

        using var sw = new StringWriter();
        var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if (viewResult.View == null)
        {
            throw new ArgumentNullException($"{viewName} does not match any available view");
        }

        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, _serviceProvider.GetService<ITempDataProvider>()),
            sw,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }
}
