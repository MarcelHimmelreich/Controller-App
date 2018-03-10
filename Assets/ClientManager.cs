using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace extOSC
{

    public class ClientManager : MonoBehaviour
    {

        //OSC
        private OSCTransmitter _transmitter;
        private OSCReceiver _receiver;

        //Network Data
        public string localIP;
        public bool admin;
        public bool connected;

        //Player Model ID
        public int modelID = 1;
        public int teamID = 1;

        //Game Mode Setting
        public int gameMode = 0;
        public int ballMode = 0;
        public int winCount = 10;

        //UI Manager
        public GameObject UIManager;

        //Receive
        private const string _osc_admin = "/client/admin/";
        private const string _osc_start = "/client/start/";
        private const string _osc_end = "/client/end/";
        private const string _osc_score = "/client/score/";

        //Send
        private const string _osc_player = "/server/player/";
        private const string _osc_gamemode = "/server/gamemode/";

        private const string _osc_control = "/control/controller/";
        private const string _osc_jump = "/control/jump/";



        // Use this for initialization
        void Start()
        {
            localIP = GetLocalIPAddress();

            _receiver = gameObject.AddComponent<OSCReceiver>();

            _receiver.LocalPort = 7001;

            _receiver.Bind(_osc_admin, ReceiveAdmin);
            _receiver.Bind(_osc_start, ReceiveStart);
            _receiver.Bind(_osc_end, ReceiveEnd);
            _receiver.Bind(_osc_score, ReceiveScoreUpdate);
        }

        // Update is called once per frame
        void Update()
        {

        }



        void ReceiveAdmin(OSCMessage message)
        {
            Debug.Log("Received Message: " + message);
            if (!connected)
            {
                if (message.Values[0].ToString() == localIP)
                {
                    admin = message.Values[1].BoolValue;
                    connected = true;
                    if (admin)
                    {
                        UIManager.GetComponent<UIManager>().SwitchScreen(1);
                    }
                    else
                    {
                        UIManager.GetComponent<UIManager>().SwitchScreen(2);
                    }
                }
            }
        }

        void ReceiveStart(OSCMessage message)
        {
            UIManager.GetComponent<UIManager>().StartGame();
        }

        void ReceiveEnd(OSCMessage message)
        {
            UIManager.GetComponent<UIManager>().EndGame(message.Values[0].BoolValue);
        }

        void ReceiveScoreUpdate(OSCMessage message)
        {

        }


        public void SendPlayer()
        {
            Debug.Log("Sending Player Setting Message");

            OSCMessage message = new OSCMessage(_osc_player);
            message.AddValue(OSCValue.Int(modelID));
            message.AddValue(OSCValue.Int(teamID));

            _transmitter.Send(message);

            UIManager.GetComponent<UIManager>().SwitchScreen(3);
        }
        public void SendGameMode()
        {
            Debug.Log("Sending Game Mode Setting Message");

            OSCMessage message = new OSCMessage(_osc_admin);
            message.AddValue(OSCValue.String(localIP));
            message.AddValue(OSCValue.Int(gameMode));
            message.AddValue(OSCValue.Int(ballMode));
            message.AddValue(OSCValue.Int(winCount));

            _transmitter.Send(message);

            UIManager.GetComponent<UIManager>().SwitchScreen(2);
        }

        public void SendJump()
        {
            Debug.Log("Sending Jump Message");

            OSCMessage message = new OSCMessage(_osc_jump);
            message.AddValue(OSCValue.String(localIP));

            _transmitter.Send(message);
        }

        public string GetLocalIPAddress()
        {
            Debug.Log("Get Local IP Adress...");

            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public void AddTeam(int number)
        {
            teamID += number;
            if (teamID < 1)
            {
                teamID = 2;
            }
            else if (teamID > 2)
            {
                teamID = 1;
            }
        }

        public void AddWinCount(int number)
        {
            winCount += number;
            if (winCount < 0)
            {
                winCount = 99;
            }
            else if (winCount > 99)
            {
                winCount = 0;
            }
        }

        public void AddGameMode(int number)
        {
            gameMode += number;
            if (gameMode < 0)
            {
                gameMode = 0;
            }
            else if (gameMode > 0)
            {
                gameMode = 0;
            }
        }

        public void AddBallMode(int number)
        {
            ballMode += number;
            if (ballMode < 0)
            {
                ballMode = 0;
            }
            else if (ballMode > 0)
            {
                ballMode = 0;
            }
        }
    }
}
