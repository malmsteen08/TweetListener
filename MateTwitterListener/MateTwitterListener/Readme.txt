//twitter context otantikasyonunu oluşrma
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

//twitterda takipçi ekleme
context.CreateFriendshipAsync("hserdarb", true).Wait(); 

//tweet atma
context.TweetAsync("twitter api denemesi").Wait();

//tweet yakalama
context.Streaming.Where(x => x.Type == StreamingType.Filter && x.Track == "fibonacci")
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
                                             string userImgUrl = obj.user.profile_image_url_https;
                                             string statusId = obj.id_str;
                                 
                                             Console.WriteLine(string.Format("{0} > {1}-{2}", userName, statusId, text));
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         string a = ex.Message;
                                     }
                                 }).Wait();