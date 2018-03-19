using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using extOSC;

public class ClientManager : MonoBehaviour
{
    //OSC
    private OSCTransmitter _transmitter;
    private OSCReceiver _receiver;

    //Network Data
    public string deviceName;
    public string localIP;
    public string serverIP;

    public bool admin;
    public bool connected;
    public bool ready;

    public List<Client> clients;

    //Player Model ID
    public int modelID = 1;
    public int teamID = 1;

    //Game Mode Setting
    public int gameMode = 0;
    public int ballMode = 0;
    public int winCount = 10;
    public int ballskin = 0;
    public int world = 0;

    //UI Manager
    public UIManager UIManager;

    //Receive
    private const string _osc_network_clients = "/server/networkclients/";
    private const string _osc_connect = "/client/connection/";
    private const string _osc_admin = "/client/admin/";
    private const string _osc_start = "/client/start/";
    private const string _osc_end = "/client/end/";
    private const string _osc_score = "/client/score/";

    //Send            
    //To Server
    private const string _osc_network_data = "/server/networkdata/";
    private const string _osc_gamemode = "/server/gamemode/";
    private const string _osc_replay = "/server/replay/";
    private const string _osc_server_start = "/server/start/";
    private const string _osc_client_leave = "/client/leave/";

    //To Controller
    private const string _osc_control = "/control/controller/";
    private const string _osc_jump = "/control/jump/";



    // Use this for initialization
    void Start()
    {
        deviceName = SystemInfo.deviceName;
        localIP = GetLocalIPAddress();

        _transmitter = gameObject.AddComponent<OSCTransmitter>();

        _transmitter.RemoteHost = serverIP;

        _transmitter.RemotePort = 7000;

        _receiver = gameObject.AddComponent<OSCReceiver>();

        _receiver.LocalPort = 6969;

        _receiver.Bind(_osc_admin, ReceiveAdmin);
        _receiver.Bind(_osc_start, ReceiveStart);
        _receiver.Bind(_osc_end, ReceiveEnd);
        _receiver.Bind(_osc_score, ReceiveScoreUpdate);
        _receiver.Bind(_osc_network_clients, ReceiveNetworkClients);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Ready
    public void SetServerIP(string ip)
    {
        serverIP = ip;
        _transmitter.RemoteHost = serverIP;
    }

    //Ready
    void ReceiveAdmin(OSCMessage message)
    {
        Debug.Log("Received Message: " + message);
        if (!connected)
        {
            admin = message.Values[0].BoolValue;
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

    //Ready
    void ReceiveNetworkClients(OSCMessage message)
    {
        int clientcount = message.Values.Count / 5;
        for (int i = 0; i < clientcount; ++i)
        {
            bool found = false;
            int clientid = 0;
            for (int o = 0; o < clients.Count; ++i)
            {
                if (clients[o].localip == message.Values[i * 5].StringValue)
                {
                    found = true;
                    clientid = o;
                }
            }

            if (found)
            {
                //Update Client
                clients[clientid].localip = message.Values[i * 5 + 0].StringValue;
                clients[clientid].name = message.Values[i * 5 + 1].StringValue;
                clients[clientid].modelID = message.Values[i * 5 + 2].IntValue;
                clients[clientid].teamID = message.Values[i * 5 + 3].IntValue;
                clients[clientid].ready = message.Values[i * 5 + 4].BoolValue;

                UIManager.GetComponent<UIManager>().UpdateClients();
            }
            else
            {
                //Create new Client
                Client newclient = new Client();
                newclient.localip = message.Values[i * 5 + 0].StringValue;
                newclient.name = message.Values[i * 5 + 1].StringValue;
                newclient.modelID = message.Values[i * 5 + 2].IntValue;
                newclient.teamID = message.Values[i * 5 + 3].IntValue;
                newclient.ready = message.Values[i * 5 + 4].BoolValue;

                clients.Add(newclient);

                UIManager.GetComponent<UIManager>().UpdateClients();
            }
        }

    }

    //Ready
    void ReceiveStart(OSCMessage message)
    {
        UIManager.GetComponent<UIManager>().StartGame();
    }

    //Ready
    void ReceiveEnd(OSCMessage message)
    {
        UIManager.GetComponent<UIManager>().EndGame(message.Values[0].BoolValue);
    }

    //Ready
    void ReceiveScoreUpdate(OSCMessage message)
    {
        int teams = message.Values.Count;
        List<int> score = new List<int>();
        for (int i = 0; i < teams; ++i)
        {
            score.Add(message.Values[i].IntValue);
        }
        UIManager.GetComponent<UIManager>().UpdateScoreText(score);
    }

    //Ready
    public void SendClient()
    {
        Debug.Log("Sending Client Message");

        OSCMessage message = new OSCMessage(_osc_network_data);
        message.AddValue(OSCValue.String(deviceName));
        message.AddValue(OSCValue.String(localIP));
        message.AddValue(OSCValue.Int(modelID));
        message.AddValue(OSCValue.Int(teamID));
        message.AddValue(OSCValue.Bool(ready));

        _transmitter.Send(message);
    }

    //Ready
    public void SendGameMode()
    {
        if (admin)
        {
            Debug.Log("Sending Game Mode Setting Message");

            OSCMessage message = new OSCMessage(_osc_gamemode);
            message.AddValue(OSCValue.Int(gameMode));
            message.AddValue(OSCValue.Int(ballMode));
            message.AddValue(OSCValue.Int(winCount));
            message.AddValue(OSCValue.Int(ballskin));
            message.AddValue(OSCValue.Int(world));

            _transmitter.Send(message);

            UIManager.GetComponent<UIManager>().SwitchScreen(2);
        }
    }

    //Ready
    public void SendReplay()
    {
        OSCMessage message = new OSCMessage(_osc_replay);
        message.AddValue(OSCValue.String(localIP));
        message.AddValue(OSCValue.Bool(ready));

        _transmitter.Send(message);
    }

    //Ready
    public void SendStart()
    {
        OSCMessage message = new OSCMessage(_osc_server_start);

        _transmitter.Send(message);
    }

    //Ready
    public void SendMovement(int value)
    {
        Debug.Log("Sending Jump Message");

        OSCMessage message = new OSCMessage(_osc_control);
        message.AddValue(OSCValue.Int(value));

        _transmitter.Send(message);
    }

    //Ready
    public void SendJump()
    {
        Debug.Log("Sending Jump Message");

        OSCMessage message = new OSCMessage(_osc_jump);

        _transmitter.Send(message);
    }

    //Ready
    public void LeaveGame()
    {
        OSCMessage message = new OSCMessage(_osc_client_leave);
        message.AddValue(OSCValue.String(localIP));
        _transmitter.Send(message);
    }

    //Ready
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

    //Ready
    public void SelectTeam(int number)
    {
        teamID += number;
        if (teamID < 0)
        {
            teamID = clients.Count - 1;
        }
        else if (teamID > clients.Count - 1)
        {
            teamID = 0;
        }
        UIManager.UpdateClients();
    }

    //Ready
    public void SelectWinCount(int number)
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
        UIManager.UpdateModeText(0, winCount);
    }

    //Ready
    public void SelectGameMode(int number)
    {
        gameMode += number;
        if (gameMode < 0)
        {
            gameMode = 2;
        }
        else if (gameMode > 2)
        {
            gameMode = 0;
        }
        UIManager.UpdateModeText(1, gameMode);
    }

    //Ready
    public void SelectWorld(int number)
    {
        world += number;
        if (world < 0)
        {
            world = UIManager.WorldList.Count - 1;
        }
        else if (world > UIManager.WorldList.Count)
        {
            world = 0;
        }
        UIManager.UpdateModeText(2, world);
    }

    //Ready
    public void SelectBallMode(int number)
    {
        ballMode += number;
        if (ballMode < 0)
        {
            ballMode = 2;
        }
        else if (ballMode > 2)
        {
            ballMode = 0;
        }
        UIManager.UpdateModeText(3, ballMode);
    }

    //Ready
    public void SelectBallSkin(int number)
    {
        ballskin += number;
        if (ballskin < 0)
        {
            ballskin = UIManager.BallList.Count - 1;
        }
        else if (ballskin > UIManager.BallList.Count)
        {
            ballskin = 0;
        }
        UIManager.UpdateModeText(4, ballskin);
        UIManager.ChooseBall(ballskin);
    }

    //Ready
    public void SelectCharacter(int value)
    {
        ballskin += value;
        if (modelID < 0)
        {
            modelID = UIManager.ModelList.Count - 1;
        }
        else if (modelID > UIManager.ModelList.Count)
        {
            modelID = 0;
        }
        UIManager.ChooseCharacter();
    }


}

