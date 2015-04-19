using Caliburn.Micro;
using Embark.Conversion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetDrafts.Models;
using TweetDrafts.MVVM;

namespace TweetDrafts.ViewModels
{
    public class TweetBoardViewModel : NotifyPropertyBase
    {
        public TweetBoardViewModel()
        {
            tweetStore = new TweetStore();
            var tweets = tweetStore.GetTweets();

            Ideas = new ObservableCollection<TweetViewModel>();
            Review = new ObservableCollection<TweetViewModel>();
            Sent = new ObservableCollection<TweetViewModel>();            

            foreach(var tweetWrapper in tweets)
            {
                var tweetViewModel = new TweetViewModel(tweetWrapper);
                tweetViewModel.PropertyChanged += tweetViewModel_PropertyChanged;
                GetTweetCollection(tweetViewModel.Status).Add(tweetViewModel);
            }
        }

        void tweetViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var tweetVM = (TweetViewModel) sender;            
            if(e.PropertyName == "Status" || e.PropertyName == "IsDeleted")
            {
                Ideas.Remove(tweetVM);
                Review.Remove(tweetVM);
                Sent.Remove(tweetVM);

                if (e.PropertyName == "Status")
                    GetTweetCollection(tweetVM.Status).Add(tweetVM);
                else if (tweetVM.IsDeleted)
                    tweetVM.TweetWrapper.Delete();
            }
        }
        
        private TweetStore tweetStore;

        public ObservableCollection<TweetViewModel> Ideas { get; set; }
        public ObservableCollection<TweetViewModel> Review { get; set; }
        public ObservableCollection<TweetViewModel> Sent { get; set; }

        public void NewTweet()
        {
            var idea = new Tweet { Status = TweetStatus.Idea };
            var tweetWrapper = tweetStore.InsertIdea(idea);

            var tweetVM = new TweetViewModel(tweetWrapper);
            Ideas.Add(tweetVM);
        }

        private IEnumerable<TweetViewModel> ToTweetViewModels(IEnumerable<DocumentWrapper<Tweet>> wrappers)
        {
            return wrappers.Select(w => new TweetViewModel(w));
        }

        private ObservableCollection<TweetViewModel> GetTweetCollection(TweetStatus tweetStatus)
        {
            switch (tweetStatus)
            {
                case TweetStatus.Idea:
                    return Ideas;
                case TweetStatus.Review:
                    return Review;
                case TweetStatus.Sent:
                    return Sent;
                default:
                    throw new InvalidOperationException();
            }
        }

    }
}
