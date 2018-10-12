
using Lidgren.Network;
using Nashet.GameLogicAbstraction;
using SamplesCommon;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Nashet.UnitGameGUI
{
    internal static class Program
    {
        private const int Port = 14242;
        private const int SleepBetweenTicks = 300;
        private const int MaximumConnections = 100;
        private static Form1 s_form;
        private static NetServer s_server;
        private static NetPeerSettingsWindow s_settingsWindow;
        private static IGameLogic gameLogic;

        [STAThread]
        private static void Main()
        {           
            gameLogic = GameLogicManager.GetOne();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s_form = new Form1();

            // set up network
            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.MaximumConnections = MaximumConnections;
            config.Port = Port;
            s_server = new NetServer(config);

            Application.Idle += new EventHandler(Application_Idle);
            Application.Run(s_form);
        }

        private static void Output(string text)
        {
            NativeMethods.AppendText(s_form.richTextBox1, text);
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            while (NativeMethods.AppStillIdle)
            {
                CheckClientCommand();
                gameLogic.SimulateOneTick();
                SendWorldData();
                
                Thread.Sleep(SleepBetweenTicks);
            }
        }        

        private static void CheckClientCommand()
        {
            NetIncomingMessage im;
            while ((im = s_server.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        string text = im.ReadString();
                        Output(text);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        string reason = im.ReadString();
                        Output(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                        if (status == NetConnectionStatus.Connected)
                            Output("Remote hail: " + im.SenderConnection.RemoteHailMessage.ReadString());

                        UpdateConnectionsList();
                        break;
                    case NetIncomingMessageType.Data:
                        // incoming message from a client
                        string data = im.ReadString();

                        Output("Received command from a client...");
                        //Output(data);
                        
                        byte[] byteArray = Encoding.ASCII.GetBytes(data);
                        MemoryStream stream = new MemoryStream(byteArray);
                        IFormatter formatter = new SoapFormatter();

                        var command = (ICommand)formatter.Deserialize(stream);
                        gameLogic.ProcessCommand(command);
                        
                        break;
                    default:
                        Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                        break;
                }
                s_server.Recycle(im);
            }
        }

        private static void SendWorldData()
        {            
            if (s_server.Connections.Count > 0)
            {
                IFormatter formatter = new SoapFormatter();
                MemoryStream ms = new MemoryStream();
                NetOutgoingMessage om = s_server.CreateMessage();
                formatter.Serialize(ms, gameLogic);
                om.Write(Encoding.ASCII.GetString(ms.ToArray()));
                s_server.SendMessage(om, s_server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }        

        private static void UpdateConnectionsList()
        {
            s_form.listBox1.Items.Clear();

            foreach (NetConnection conn in s_server.Connections)
            {
                string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndPoint.ToString() + " [" + conn.Status + "]";
                s_form.listBox1.Items.Add(str);
            }
        }

        // called by the UI
        public static void StartServer()
        {
            s_server.Start();
        }

        // called by the UI
        public static void Shutdown()
        {
            s_server.Shutdown("Requested by user");
        }

        // called by the UI
        public static void DisplaySettings()
        {
            if (s_settingsWindow != null && s_settingsWindow.Visible)
            {
                s_settingsWindow.Hide();
            }
            else
            {
                if (s_settingsWindow == null || s_settingsWindow.IsDisposed)
                    s_settingsWindow = new NetPeerSettingsWindow("Game server settings", s_server);
                s_settingsWindow.Show();
            }
        }
    }
}
