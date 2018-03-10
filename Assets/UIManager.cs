using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public List<GameObject> ModelList;
    public GameObject Character;
    public Transform Spawn;
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

    public void UpdateScore(List<int> scores)
    {
        for (int i = 0; i < Score.Count; ++i)
        {
            Score[i].text = scores[i].ToString();
        }
    }

    public void SwitchScreen(int number)
    {
        for (int i = 0; i<ScreenList.Count; ++i)
        {
            ScreenList[i].active = false;
        }
        ScreenList[number].active = true;
    }

    public void ChooseCharacter(int number)
    {
        if (Character)
        {
            Destroy(Character);
        }
        Instantiate(ModelList[number],Spawn);
    }

    public void EndGame(bool win)
    {

    }

    public void StartGame()
    {
        GameScreen.active = true;
        WaitingScreen.active = false;
    }
        
}
