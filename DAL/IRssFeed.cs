using DAL.Models;

namespace DAL
{
    public interface IRssFeed
    {
        public RssFeedModel GetPost(int id);
        public List<RssFeedModel> GetNewsListFromCache();
    }
}
