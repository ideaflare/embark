using Embark.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TweetDrafts.Models;
using TweetDrafts.MVVM;

namespace TweetDrafts.ViewModels
{
    public class TweetViewModel : NotifyPropertyBase
    {
        public TweetViewModel(DocumentWrapper<Tweet> tweetWrapper)
        {
            this.TweetWrapper = tweetWrapper;

            tweetWrapper.Value.PropertyChanged += Tweet_PropertyChanged;
        }

        void Tweet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.NotifyOfPropertyChange(e.PropertyName);
            TweetWrapper.Update();
        }

        public DocumentWrapper<Tweet> TweetWrapper { get; private set; }

        public string Text
        {
            get { return TweetWrapper.Value.Text; }
            set { TweetWrapper.Value.Text = value; }
        }

        public int Characters { get { return TweetWrapper.Value.Characters; } }
        public TweetStatus Status { get { return TweetWrapper.Value.Status; } }
        public bool IsDeleted { get { return TweetWrapper.Value.IsDeleted; } }

        public void DeleteTweet()
        {
            if(MessageBox.Show("Are you sure you want to delete the tweet?","Delete Tweet",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                TweetWrapper.Value.IsDeleted = true;
            }
        }

        public bool CanPreviousBoard()
        {
            return TweetWrapper.Value.Status != TweetStatus.Idea;
        }
        
        public bool CanNextBoard()
        {
            return TweetWrapper.Value.Status != TweetStatus.Sent;
        }

        public void PreviousBoard()
        {
            switch (TweetWrapper.Value.Status)
            {
                case TweetStatus.Idea:
                    throw new InvalidOperationException();
                case TweetStatus.Review:
                    TweetWrapper.Value.Status = TweetStatus.Idea;
                    break;
                case TweetStatus.Sent:
                    TweetWrapper.Value.Status = TweetStatus.Review;
                    break;
                default:
                    break;
            }
        }

        public void NextBoard() 
        {
            switch (TweetWrapper.Value.Status)
            {
                case TweetStatus.Idea:
                    TweetWrapper.Value.Status = TweetStatus.Review;
                    break;
                case TweetStatus.Review:
                    TweetWrapper.Value.Status = TweetStatus.Sent;
                    break;
                case TweetStatus.Sent:
                    throw new InvalidOperationException();
                default:
                    break;
            }
        }

    }
}
