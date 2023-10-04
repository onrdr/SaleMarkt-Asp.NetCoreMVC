using Models.Entities.Concrete;

namespace Models.ViewModels;

public class ShoppingCartViewModel
{
    public ShoppingCartViewModel()
    {
        ShoppingCartList = new List<ShoppingCart>();
        OrderHeader = new OrderHeader();
    }

    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

    public OrderHeader OrderHeader { get; set; }
}
