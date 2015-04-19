using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetDrafts.ViewModels
{
    public class MainViewModel
    {
        public TweetBoardViewModel TweetBoardViewModel
        {
            get
            {
                return new TweetBoardViewModel();
            }
        }

        public string WindowTitle
        {
            get { return "TweetDrafts"; }
        }
 
    }
}
