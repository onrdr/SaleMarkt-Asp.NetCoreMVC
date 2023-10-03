namespace Business.Services.Abstract;

public interface IViewRenderService
{
    Task<string> RenderViewToStringAsync(string viewName, object model);
}
