using Microsoft.EntityFrameworkCore;
using TravelEasy.Data;
using TravelEasy.Models;
using TravelEasy.Models.DTO;

public class OrderService : IOrderService
{
    private readonly TravelEasyContext _context;

    public OrderService(TravelEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Product.Price
                }).ToList()
            }).ToListAsync();
    }

    public async Task<OrderDTO> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        return new OrderDTO
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            UserId = order.UserId,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
            {
                ProductId = oi.ProductId,
                ProductTitle = oi.Product.Title,
                Quantity = oi.Quantity,
                Price = oi.Product.Price
            }).ToList()
        };
    }

    public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
    {
        // Verifica se l'utente esiste
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == orderDto.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Creare la lista di OrderItem con i dettagli dei prodotti
        var orderItems = new List<OrderItem>();

        foreach (var oi in orderDto.OrderItems)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == oi.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with ID {oi.ProductId} not found");
            }

            var orderItem = new OrderItem
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                Product = product // Associa il prodotto caricato
            };

            orderItems.Add(orderItem);
        }

        // Creare l'ordine
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            Status = orderDto.Status,
            ShippingAddress = orderDto.ShippingAddress,
            UserId = orderDto.UserId,
            OrderItems = orderItems
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Popola il DTO di ritorno
        orderDto.Id = order.Id;
        orderDto.UserName = $"{user.Nome} {user.Cognome}"; // Popola il nome dell'utente

        // Popola i dettagli degli articoli dell'ordine
        orderDto.OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
        {
            ProductId = oi.ProductId,
            ProductTitle = oi.Product.Title, // Popola il titolo del prodotto
            Quantity = oi.Quantity,
            Price = oi.Product.Price
        }).ToList();

        return orderDto;
    }





    public async Task<OrderDTO> UpdateOrderAsync(int id, OrderDTO orderDto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return null;

        order.Status = orderDto.Status;
        order.ShippingAddress = orderDto.ShippingAddress;

        await _context.SaveChangesAsync();
        return orderDto;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Select(order => new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product.Title,
                    Quantity = oi.Quantity,
                    Price = oi.Product.Price
                }).ToList()
            }).ToListAsync();
    }
}
