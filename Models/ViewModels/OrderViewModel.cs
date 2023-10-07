using Models.Entities.Concrete;

namespace Models.ViewModels;

public class OrderViewModel
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetail> OrderDetailList { get; set; }
}
