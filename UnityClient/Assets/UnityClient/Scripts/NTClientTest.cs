using Lidgren.Network;
using Nashet.GameLogicAbstraction;
using Nashet.UnitSelection;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Nashet.UnitsGameClient
{
    /// <summary>
    /// Deals with sending/receiving data from server
    /// </summary>
    public class NTClientTest : MonoBehaviour
    {
        protected const int Port = 14242;
        protected const string Host = "localhost";
        protected static NetClient s_client;

        protected Text UIText;

        [SerializeField] private MapVisualisator mapVisualisator;
        [SerializeField] private SelectionComponent selector;

        // Use this for initialization
        protected void Start()
        {
            UIText = GetComponent<Text>();

            NetPeerConfiguration config = new NetPeerConfiguration("chat");
            config.AutoFlushSendQueue = false;
            s_client = new NetClient(config);

            //asynchronous net work. Can be non asynchronous, see Update()
            s_client.RegisterReceivedCallback(new SendOrPostCallback(CheckMessageFromServer));

            selector.GoToCommandMade += OnGoToCommandMade;

        }

        protected static void Send(string text)
        {
            NetOutgoingMessage om = s_client.CreateMessage(text);
            s_client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            s_client.FlushSendQueue();
        }

        /// <summary>
        /// Called then there is command to send to server
        /// </summary>        
        protected void OnGoToCommandMade(object sender, EventArgs e)
        {
            var arg = e as SelectionComponent.GoToCommandMadeArgs;
            var command = new Command(arg.Destination, arg.list);
            IFormatter formatter = new SoapFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, command);
            Send(Encoding.ASCII.GetString(ms.ToArray()));
        }

        /// <summary>
        /// Is callback
        /// </summary>        
        protected void CheckMessageFromServer(object peer)
        {
            NetIncomingMessage im;
            if ((im = s_client.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        if (UIText != null)
                        {
                            string text = im.ReadString();
                            UIText.text = text;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                        //if (status != NetConnectionStatus.Connected)
                        //    Connect("localhost", 14242);
                        if (UIText != null)
                        {
                            string reason = im.ReadString();
                            UIText.text = status.ToString() + ": " + reason;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        string chat = im.ReadString();
                        {
                            if (mapVisualisator != null) // not destroyed yet
                                mapVisualisator.ReceiveNetData(chat);
                        }
                        break;
                    default:
                        UIText.text = "Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes";
                        break;
                }
                s_client.Recycle(im);
            }
        }

        protected static void Connect(string host, int port)
        {
            s_client.Start();
            NetOutgoingMessage hail = s_client.CreateMessage("This is the hail message");
            s_client.Connect(host, port, hail);
        }

        protected void Update()
        {
            if (s_client.Status != NetPeerStatus.Running)
                Connect(Host, Port);
            //else //non asynchronous net work 
            //    CheckMessageFromServer(s_client);
        }

        protected void OnDestroy()
        {
            s_client.Shutdown("Client is shut down");
        }
    }
}