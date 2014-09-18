using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateTwitterListener
{
    public class TweetData
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string StatusId { get; set; }
        public string Text { get; set; }
        public string TweetUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        public TweetData()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
