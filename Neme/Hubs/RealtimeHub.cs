using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NemeShop.Hubs
{
    public class RealtimeHub : Hub
    {
        // ====== PRODUCT ======
        public async Task NotifyProductUpdated(int productId)
        {
            await Clients.All.SendAsync("productUpdated", productId);
        }

        // ====== REVIEW ======
        public async Task NotifyReviewAdded(int productId, int reviewId)
        {
            await Clients.Group($"product_{productId}")
                         .SendAsync("reviewAdded", productId, reviewId);
        }

        // ====== ORDER ======
        public async Task JoinOrderGroup(int orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"order_{orderId}");
        }

        public async Task LeaveOrderGroup(int orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order_{orderId}");
        }

        public async Task NotifyOrderStatusChanged(int orderId, int status)
        {
            await Clients.Group($"order_{orderId}")
                         .SendAsync("orderStatusChanged", orderId, status);
        }
    }
}
