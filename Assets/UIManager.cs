using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public ClientManager ClientManager;
    public CameraManager CameraManager;
    //0 Home Screen
    //1 Character Selection
    //2 Team Selection
    //3 Game Screen

    public List<GameObject> ModelList;
    public List<GameObject> BallList;
    public List<GameObject> WorldList;
    public List<Text> Modelist;
    public GameObject Ball;
    public Transform BallSpawn;
    public List<GameObject> Character;
    public List<Text> Team;
    public List<Transform> Spawn;
    public List<GameObject> ScreenList;
    public GameObject GameScreen;
    public GameObject WaitingScreen;
    public List<Text> Score;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Ready
    public void UpdateClients()
    {
        for (int i = 0; i< ClientManager.clients.Count; ++i)
        {
            if( ClientManager.clients[i].localip != ClientManager.localIP)
            {
                if (Character[i] != ModelList[ClientManager.clients[i].modelID])
                {
                    Destroy(Character[i]);
                    Character.Add(Instantiate(ModelList[ClientManager.clients[i].modelID]));
                }
                if (Team[i] != Team[ClientManager.clients[i].teamID])
                {
                    Team[i].text = ClientManager.clients[i].teamID.ToString();
                }
            }
        }
    }

    //Ready
    public void UpdateScoreText(List<int> scores)
    {
        for (int i = 0; i < Score.Count; ++i)
        {
            Score[i].text = scores[i].ToString();
        }
    }

    //
    public void UpdateModeText(int mode, int value)
    {
        if (mode == 0)
        {
            //Win Count
            Modelist[mode].text = value.ToString();
        }
        else if (mode == 1)
        {
            //Game Mode
            if (value == 0)
            {
                //Normal
                Modelist[mode].text = "Normal";
            }
            else if (value == 1)
            {
                //Fury
                Modelist[mode].text = "Fury";
            }
            else if (value == 2)
            {
                //Insane
                Modelist[mode].text = "Insane";
            }
        }
        else if (mode == 2)
        {
            //World
            if (value == 0)
            {
                //Island
                Modelist[mode].text = "Island";
            }
            else if (value == 1)
            {
                //Ocean
                Modelist[mode].text = "Ocean";
            }
            else if (value == 2)
            {
                //Ancient
                Modelist[mode].text = "Ancient";
            }
        }
        else if (mode == 3)
        {
            //Ball Mode
            if (value == 0)
            {
                //small
                Modelist[mode].text = "Tiny";
            }
            else if (value == 1)
            {
                //normal
                Modelist[mode].text = "Normal";
            }
            else if (value == 2)
            {
                //large
                Modelist[mode].text = "Large";
            }
        }
        else if (mode == 4)
        {
            //Ball Skin
            if (value == 0)
            {
                //Beach
                Modelist[mode].text = "Beach";
            }
            else if (value == 1)
            {
                //Stone
                Modelist[mode].text = "Stone";
            }
            else if (value == 2)
            {
                //Balloon
                Modelist[mode].text = "Balloon";
            }
        }
    }

    //Ready
    public void SwitchScreen(int number)
    {
        for (int i = 0; i<ScreenList.Count; ++i)
        {
            ScreenList[i].active = false;
        }
        ScreenList[number].active = true;
    }

    //Ready
    public void Connected()
    {
        CameraManager.ChangePosition(1);
    }

    //Ready
    public void CharacterSelected()
    {
        CameraManager.ChangePosition(2);
    }

    //Ready
    public void ChooseCharacter()
    {
        if (Character[0])
        {
            Destroy(Character[0]);
        }
        Character[0] = Instantiate(ModelList[ClientManager.modelID],Spawn[0]);
    }

    //Ready
    public void ChooseBall(int value)
    {
        if (Ball !=null)
        {
            Destroy(Ball);
        }
        Ball = Instantiate(BallList[value],BallSpawn);
    }

    //Ready
    public void LeaveGame()
    {
        ClientManager.LeaveGame();
        CameraManager.ChangePosition(0);
        SwitchScreen(0);
        CameraManager.ChangePosition(0);
    }

    //Ready
    public void ReadyToPlay(bool ready)
    {
        //Replay or apply Game
        if (ready)
        {
            SwitchScreen(3);
            CameraManager.ChangePosition(3);

        }
        //Switch Character after Game
        else
        {
            SwitchScreen(2);
        }
        GameScreen.active = false;
        WaitingScreen.active = true;
        ClientManager.ready = ready;
        ClientManager.SendClient();

    }

    //Ready
    public void EndGame(bool win)
    {
        //Switch to End Screen
        SwitchScreen(4);
        GameScreen.active = false;
        WaitingScreen.active = true;
    }

    //Ready
    public void StartGame()
    {
        //Switch to Controller Screen   
        GameScreen.active = true;
        WaitingScreen.active = false;
    }
        
}
