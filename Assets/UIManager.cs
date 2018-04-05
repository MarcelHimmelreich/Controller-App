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
    public Text WinCount;
    public Text ModeText;
    public Text BallSkinText;
    public Text WorldmodeText;
    public Text BallModeText;
    public GameObject Ball;
    public Transform BallSpawn;

    public List<GameObject> Character;
    public GameObject SelectedCharacter;
    public GameObject SelectedSpawn;

    //Team Selection
    public List<Text> Name;
    public List<Text> Team;
    public List<Text> Ready;

    public List<Transform> Spawn;
    public List<GameObject> ScreenList;

    public GameObject GameScreen;
    public GameObject WaitingScreen;
    public List<Text> Score;
    public GameObject StartButton;

    //Debug
    public bool clear = false;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (clear)
        {
            UpdateClients();
            clear = false;
        }       
	}

    //Ready
    public void UpdateClients()
    {
        foreach (GameObject character in Character)
        {
            Destroy(character);
        }
        Character.Clear();
        for (int i = 0; i< ClientManager.clients.Count; ++i)
        {
            Character.Add(Instantiate(ModelList[ClientManager.clients[i].modelID],Spawn[i]) as GameObject);
            Name[i].text = ClientManager.clients[i].name;
            Team[i].text = "Team " + (1 + ClientManager.clients[i].teamID).ToString();
            if (ClientManager.clients[i].ready)
            {
                Ready[i].text = "Ready";
                Character[i].GetComponent<Animator>().Play("cheer");
            }
            else
            {
                Ready[i].text = "";
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
            WinCount.text = value.ToString();
        }
        else if (mode == 1)
        {
            //Game Mode
            if (value == 0)
            {
                //Normal
                ModeText.text = "Normal";
            }
            else if (value == 1)
            {
                //Fury
                ModeText.text = "Fury";
            }
            else if (value == 2)
            {
                //Insane
                ModeText.text = "Insane";
            }
        }
        else if (mode == 2)
        {
            //World
            if (value == 0)
            {
                //Island
                WorldmodeText.text = "Island";
            }
            else if (value == 1)
            {
                //Ocean
                WorldmodeText.text = "Ocean";
            }
            else if (value == 2)
            {
                //Ancient
                WorldmodeText.text = "Ancient";
            }
        }
        else if (mode == 3)
        {
            //Ball Mode
            if (value == 0)
            {
                //small
                BallModeText.text = "Tiny";
            }
            else if (value == 1)
            {
                //normal
                BallModeText.text = "Normal";
            }
            else if (value == 2)
            {
                //large
                BallModeText.text = "Large";
            }
        }
        else if (mode == 4)
        {
            //Ball Skin
            if (value == 0)
            {
                //Beach
                BallSkinText.text = "Beach";
            }
            else if (value == 1)
            {
                //Stonee
                BallSkinText.text = "Stone";
            }
            else if (value == 2)
            {
                //Balloon
                BallSkinText.text = "Balloon";
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
        //Start Player Selection
        CameraManager.ChangePosition(1);
    }

    //Ready
    public void CharacterSelected()
    {
        //Change to Team Selection
        CameraManager.ChangePosition(2);
        SwitchScreen(3);
        ClientManager.SendClient();
        UpdateClients();
        if (ClientManager.admin)
        {
            StartButton.SetActive(true);
        }
    }

    //Ready
    public void ChooseCharacter()
    {
        if (SelectedCharacter != null)
        {
            Destroy(SelectedCharacter);
        }
        SelectedCharacter = Instantiate(ModelList[ClientManager.modelID],SelectedSpawn.transform) as GameObject;
        
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
    public void ChangeCharacter()
    {
        SwitchScreen(2);
        CameraManager.ChangePosition(1);
        ClientManager.ready = false;
        GameScreen.active = false;
        WaitingScreen.active = true;
        ClientManager.SendClient();

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
    public void ReadyButton(bool ready)
    {
        ClientManager.ready = ready;
        ClientManager.SendClient();
        UpdateClients();
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
        CameraManager.ChangePosition(2);
        GameScreen.active = false;
        WaitingScreen.active = true;
    }

    //Ready
    public void StartGame()
    {
        //Switch to Controller Screen   
        CameraManager.ChangePosition(3);
        GameScreen.active = true;
        WaitingScreen.active = false;
    }
        
}
