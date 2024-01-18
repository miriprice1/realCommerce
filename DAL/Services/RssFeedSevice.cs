using Microsoft.Extensions.Caching.Memory;
using System.Xml;

namespace DAL.Models
{
    public class RssFeedService : IRssFeed
    {

        private static int counterId;
        //defineing Http Cache memory
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "RssFeedData";

        public RssFeedService(IMemoryCache memoryCache)
        {
            counterId = 0;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// Returns data about a specific post by id.
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>The post data (direct access because the ID's are automatically numbered)</returns>
        public RssFeedModel GetPost(int id)
        {
            List<RssFeedModel> list = GetNewsListFromCache();
            return list[id];
        }
        /// <summary>
        /// A function for retrieving the data from the cache and returning it.
        /// </summary>
        /// <returns>The data list.</returns>
        public List<RssFeedModel> GetNewsListFromCache()
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<RssFeedModel> cachedNewsList))
            {
                // Data found in cache, return it
                return cachedNewsList;
            }

            // Data not found in cache, fetch and cache it
            List<RssFeedModel> newsList = FetchAndCacheNewsList();

            return newsList;
        }
        /// <summary>
        /// A function for loading the XML file, decoding, building the list and returning the data.
        /// </summary>
        /// <returns>The data list.</returns>
        private List<RssFeedModel> FetchAndCacheNewsList()
        {
            List<RssFeedModel> list = new List<RssFeedModel>();
            string url = "https://news.google.com/rss?pz=1&cf=all&hl=en-IL&gl=IL&ceid=IL:en";

            try
            {
                //loading the xml file.
                XmlDocument doc = new XmlDocument();
                doc.Load(url);
                XmlNodeList itemNodes = doc.SelectNodes("/rss/channel/item");

                //building the list.
                foreach (XmlNode itemNode in itemNodes)
                {
                    RssFeedModel xmlItem = new RssFeedModel()
                    {
                        Id = counterId++,
                        Title = GetNodeText(itemNode, "title"),
                        Description = GetNodeText(itemNode, "description"),
                        Link = GetNodeText(itemNode, "link")
                    };
                    list.Add(xmlItem);
                }

                // Cache the data
                _memoryCache.Set(CacheKey, list, TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error reading xml file: {ex.Message}");
            }

            return list;
        }

        private string GetNodeText(XmlNode parent, string nodeName)
        {
            XmlNode node = parent.SelectSingleNode(nodeName);
            return node != null ? node.InnerText : null;
        }
    }

}