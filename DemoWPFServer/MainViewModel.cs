using System;
using Embark;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoWPFServer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            try { TextFeedback = "This PC: " + System.Net.Dns.GetHostName(); }
            finally { } //meh
            server = new Server();
        }

        Embark.Server server;

        public ICommand StartStopCommand
        {
            get
            {
                return new ActionCommand(StartStop);
            }
        }

        private string buttonAction = "Start";
        public string ButtonAction
        {
            get { return this.buttonAction; }
            set
            {
                if(this.buttonAction != value)
                {
                    this.buttonAction = value;
                    OnPropertyChangedEvent();
                }
            }
        }

        private string textFeedback = "";
        public string TextFeedback
        {
            get { return this.textFeedback; }
            set
            {
                if (this.textFeedback != value)
                {
                    this.textFeedback = value;
                    OnPropertyChangedEvent();
                }
            }
        }

        void StartStop()
        {
            if(ButtonAction == "Start")
            {
                try
                {
                    server.Start();
                    ButtonAction = "Stop";
                }
                catch (Exception e)
                {
                    TextFeedback = e.ToString();
                }
            }
            else if(ButtonAction == "Stop")
            {
                server.Stop();
                ButtonAction = "Start";
            }
        }

        private void OnPropertyChangedEvent([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
