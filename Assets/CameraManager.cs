using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public List<Transform> Points;
    public int targetPoint = 0;
    public float time = 0.3f;
    private Vector3 velocity = Vector3.zero;
    //0 Home Screen
    //1 Charactrer Selection
    //2 Team Selection
    //3 Game Screen

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Ready
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Points[targetPoint].transform.position,ref velocity, time);
        transform.rotation = Quaternion.Slerp(transform.rotation,Points[targetPoint].rotation,time);
    }

    //Ready
    public void ChangePosition(int number)
    {
        targetPoint = number;
    }
}
