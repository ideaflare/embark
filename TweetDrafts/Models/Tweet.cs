using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetDrafts.MVVM;

namespace TweetDrafts.Models
{
    public class Tweet : NotifyPropertyBase
    {
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (SetProperty(ref text, value))
                    this.Characters = 140 - value.Length;
            }
        }

        private int characters;
        public int Characters
        {
            get { return characters; }
            set { SetProperty(ref characters, value); }
        }

        private TweetStatus status;
        public TweetStatus Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private bool isDeleted;
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { SetProperty(ref isDeleted, value); }
        }

    }
}
