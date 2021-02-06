using System.Linq;
using MovieStore.Models;

namespace MovieStore.Repository
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}
