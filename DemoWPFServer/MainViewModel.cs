using System;
using Embark;
using System.ComponentModel;
using System.Windows.Input;
using Embark.Interaction.MVVM;

namespace DemoWPFServer
{
    public sealed class MainViewModel : PropertyChangeBase, INotifyPropertyChanged, IDisposable
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

        Server server;

        public ICommand StartStopCommand => new ActionCommand(StartStop);

        private string buttonAction = "Start";
        public string ButtonAction
        {
            get { return buttonAction; }
            set
            {
                if(buttonAction != value)
                {
                    buttonAction = value;
                    RaisePropertyChangedEvent();
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
                    RaisePropertyChangedEvent();
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
                    RaisePropertyChangedEvent();
                }
            }
        }

        private int portNumber = 8030;
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
                    RaisePropertyChangedEvent();
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

        public void Dispose()
        {
            if(server != null)
                server.Dispose();
        }
    }
}
