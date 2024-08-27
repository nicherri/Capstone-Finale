using TravelEasy.Models.DTO;

public interface IOrderService
{
    Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
    Task<OrderDTO> GetOrderByIdAsync(int id);
    Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
    Task<OrderDTO> UpdateOrderAsync(int id, OrderDTO orderDto);
    Task<bool> DeleteOrderAsync(int id);
    Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId);
}
