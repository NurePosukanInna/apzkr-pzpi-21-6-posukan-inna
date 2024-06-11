using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionType>> GetSubscriptionTypes();
        Task<SubscriptionType> AddSubscriptionType(SubscriptionType subscriptionType);
        Task<Subscription> AddSubscriptionToUser(int userid, Subscription subscription);
        Task<IEnumerable<Subscription>> GetActiveSubscriptionsForUser(int userId);
    }
}
