using System;
using Embark;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DemoWPFServer
{
    public sealed class MainViewModel : INotifyPropertyChanged, IDisposable
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
            get { return buttonAction; }
            set
            {
                if(buttonAction != value)
                {
                    buttonAction = value;
                    OnPropertyChangedEvent();
                }
            }
        }

        private string textFeedback = "";
        public string TextFeedback
        {
            get { return textFeedback; }
            set
            {
                if (textFeedback != value)
                {
                    textFeedback = value;
                    OnPropertyChangedEvent();
                }
            }
        }
        
        private string directory = @"C:\MyTemp\Embark\Server\";
        public string Directory
        {
            get { return directory; }
            set
            {
                if (directory != value)
                {
                    directory = value;
                    OnPropertyChangedEvent();
                }
            }
        }

        private int portNumber = 8080;
        public string PortNumber
        {
            get { return portNumber.ToString(); }
            set
            {
                if (portNumber.ToString() != value)
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
                    server = new Server(directory, portNumber);
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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            if(server != null)
                server.Dispose();
        }
    }
}
