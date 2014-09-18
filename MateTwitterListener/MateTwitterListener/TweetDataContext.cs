using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateTwitterListener
{
    public class TweetDataContext:DbContext
    {
        public DbSet<TweetData> TweetDatas { get; set; }
    }
}
