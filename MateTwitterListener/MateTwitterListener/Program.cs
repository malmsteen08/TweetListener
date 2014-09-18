using LinqToTwitter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateTwitterListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new TweetDataContext();
            var context = ConnectToTwitter();
            var keywords = "integral,fibonacci,pisagor,öklid";
            ListenTweeterStream(dbContext, context, keywords);
        }

        private static void ListenTweeterStream(TweetDataContext dbContext, TwitterContext context, string keywords)
        {
            try
            {
                context.Streaming.Where(x => x.Type == StreamingType.Filter && x.Track == keywords)
                                 .StartAsync(async y =>
                                 {
                                     try
                                     {
                                         dynamic obj = JsonConvert.DeserializeObject(y.Content);
                                         if (obj != null
                                             && obj.user != null)
                                         {
                                             string text = obj.text;
                                             string userName = obj.user.screen_name;
                                             string statusId = obj.id_str;

                                             Console.WriteLine(string.Format("{0} > {1}-{2}", userName, statusId, text));

                                             PersistToDb(dbContext, statusId, userName, text);
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         string a = ex.Message;
                                     }
                                 }).Wait();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private static void PersistToDb(TweetDataContext dbContext, string statusId, string userName, string text)
        {
            var tweetUrl = string.Format("https://twitter.com/{0}/status/{1}", userName, statusId);
            var data = new TweetData
            {
                StatusId = statusId,
                UserName = userName,
                Text = text,
                TweetUrl = tweetUrl
            };
            dbContext.TweetDatas.Add(data);
            dbContext.SaveChanges();
        }

        private static TwitterContext ConnectToTwitter()
        {
            Console.WriteLine("Connecting to twitter");
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["apiKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["apiSecret"],
                    AccessToken = ConfigurationManager.AppSettings["accessToken"],
                    AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
                }
            };

            var context = new TwitterContext(auth);

            Console.WriteLine("Connected to twitter");
            return context;
        }
    }
}
