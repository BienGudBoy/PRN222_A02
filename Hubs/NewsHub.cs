using Microsoft.AspNetCore.SignalR;

namespace NguyenVanBien_SE18C.NET_A02.Hubs
{
    public class NewsHub : Hub
    {
        public async Task NewsUpdated()
        {
            await Clients.All.SendAsync("RefreshNewsList");
        }

        public async Task NewsCreated(string newsId, string newsTitle)
        {
            await Clients.All.SendAsync("NewsCreated", newsId, newsTitle);
        }

        public async Task NewsModified(string newsId, string newsTitle)
        {
            await Clients.All.SendAsync("NewsModified", newsId, newsTitle);
        }

        public async Task NewsDeleted(string newsId)
        {
            await Clients.All.SendAsync("NewsDeleted", newsId);
        }
    }
}
