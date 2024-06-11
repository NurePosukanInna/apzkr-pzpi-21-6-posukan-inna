using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;
using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly InventoryContext _context;

        public SubscriptionService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubscriptionType>> GetSubscriptionTypes()
        {
            return await _context.SubscriptionTypes.ToListAsync();
        }

        public async Task<SubscriptionType> AddSubscriptionType(SubscriptionType subscriptionType)
        {
            _context.SubscriptionTypes.Add(subscriptionType);
            await _context.SaveChangesAsync();
            return subscriptionType;
        }

        public async Task<Subscription> AddSubscriptionToUser(int userid, Subscription subscription)
        {
            var user = await _context.Users.FindAsync(userid);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var subscriptionType = await _context.SubscriptionTypes.FindAsync(subscription.SubscriptionTypeId);
            if (subscriptionType == null)
            {
                throw new ArgumentException("Subscription type not found");
            }

            subscription.StartDate = DateTime.Now;
            subscription.EndDate = subscription.StartDate?.AddMonths(1);
            subscription.User = user;

            if (subscription.StartDate <= DateTime.Now && subscription.EndDate >= DateTime.Now)
            {
                subscription.SubscriptionStatus = "Active";
            }
            else
            {
                subscription.SubscriptionStatus = "Canceled";
            }

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }


        public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsForUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var activeSubscriptions = await _context.Subscriptions
                .Include(s => s.SubscriptionType)
                .Where(s => s.UserId == userId && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now)
                .ToListAsync();

            return activeSubscriptions;
        }
    }
}
