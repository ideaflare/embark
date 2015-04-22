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
            try
            {
                TextFeedback = "This PC: " + System.Net.Dns.GetHostName();
            }
            catch (Exception e)
            {
                TextFeedback = "Couldn't guess server name: " + e.ToString();
            } 
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
        
        private string directory = @"C:\MyTemp\Embark\Server\";
        public string Directory
        {
            get { return this.directory; }
            set
            {
                if (this.directory != value)
                {
                    this.directory = value;
                    OnPropertyChangedEvent();
                }
            }
        }

        private int portNumber = 8080;
        public string PortNumber
        {
            get { return this.portNumber.ToString(); }
            set
            {
                if (this.portNumber.ToString() != value)
                {
                    int newNumber;
                    if (int.TryParse(value, out newNumber) && newNumber > 0)
                    {
                        portNumber = newNumber;
                    }
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
                    server = new Server(this.directory, this.portNumber);
                    server.Start();
                    TextFeedback = "Server started.";
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
                TextFeedback = "Server stopped";
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
