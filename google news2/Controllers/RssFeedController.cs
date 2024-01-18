using DAL;
using Microsoft.AspNetCore.Mvc;

namespace google_news2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssFeedController : ControllerBase
    {
        IRssFeed _rssFeed;
        public RssFeedController(IRssFeed rssFeed)
        {
            _rssFeed = rssFeed;
        }

        //controller to getting the news list.
        [HttpGet]
        [Route("getFeed")]
        public IActionResult GetFeed()
        {
            return Ok(_rssFeed.GetNewsListFromCache());
        }

        //controller to getting the specific post details.
        [HttpGet]
        [Route("getPost/{id}")]
        public IActionResult GetPost([FromRoute] int id)
        {
            try
            {
                return Ok(_rssFeed.GetPost(id));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
