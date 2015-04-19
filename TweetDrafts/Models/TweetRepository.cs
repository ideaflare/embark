using Embark;
using Embark.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TweetDrafts.Models
{
    public class TweetStore
    {
        public TweetStore()
        {
            var tweetData = Directory.GetCurrentDirectory();
            var client = Client.GetLocalDB(tweetData);

            tweets = client.GetCollection<Tweet>("tweets");
        }

        private Collection<Tweet> tweets;

        public DocumentWrapper<Tweet> InsertIdea(Tweet idea)
        {
            var id = tweets.Insert(idea);
            return tweets.GetWrapper(id);
        }
           
        internal IEnumerable<DocumentWrapper<Tweet>> GetTweets()
        {
            return tweets.GetAll();
        }

        public IEnumerable<DocumentWrapper<Tweet>> GetTweetWithStatus(TweetStatus status)
        {
            var ideaTweets = tweets.GetWhere(new { Status = status });
            return ideaTweets;
        }
    }
}
