using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	[HideInInspector]
	public GameObject model;
	float hSpeed;
	float vSpeed;
	Vector3 relativeSpeed2;
	FollowCam followCam;
	CPReceiverV5 receiver;
	public GameObject startingBall;
	
	[HideInInspector]
	public float points;
	public float lives = 3;
	
	//[HideInInspector]
	public int currentStageNumber = -1;
	
	Rect scorePosition;
	Rect livesPosition;
	
	string currentLevel;
	
	// Use this for initialization
	void Start () {
	model = GameObject.Find("model");
	followCam = GameObject.Find("FollowCam").GetComponent("FollowCam") as FollowCam;
	receiver = gameObject.GetComponent("CPReceiverV5") as CPReceiverV5;
	ChangeBall(startingBall);
	scorePosition = new Rect(10,10,100,20);
		livesPosition = new Rect(10,30,100,20);
		currentLevel = Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update () 
	{
		hSpeed = Input.GetAxis("Horizontal") *50 + receiver.yTilt;
		vSpeed = Input.GetAxis("Vertical") *50 - receiver.zTilt;
		switch(followCam.Dir)
		{
			case FollowCam.Direction.North: relativeSpeed2 = new Vector3(hSpeed,0,vSpeed); break;
			case FollowCam.Direction.East: relativeSpeed2 = new Vector3(vSpeed,0,-hSpeed); break;
			case FollowCam.Direction.South: relativeSpeed2 = new Vector3(-hSpeed,0,-vSpeed); break;
			case FollowCam.Direction.West:relativeSpeed2 = new Vector3(-vSpeed,0,hSpeed); break;
		}
		model.constantForce.force = relativeSpeed2;
		followCam.transform.position = model.transform.position;
		if (Input.GetKeyDown("q"))
			GoToStage(currentStageNumber-1);
		if (Input.GetKeyDown("e"))
			GoToStage(currentStageNumber+1);
	}
	
	public void ChangeBall(GameObject BallModel)
	{
		Destroy(model);
		model = Instantiate(BallModel,model.transform.position,model.transform.rotation) as GameObject;
		model.name="model";
		Debug.Log("Change ball");
	}
	
	
	public void OnGUI()
	{
		GUI.Label(livesPosition,new GUIContent("Lives: "+lives.ToString()));
		GUI.Label(scorePosition,new GUIContent("Score: "+points.ToString()));
	}
	
	public void Respawn()
	{
		if (lives==0)
		{
			Application.LoadLevel(currentLevel);
			return;
		}
		lives--;
		Debug.Log("Respawn");
		GoToStage(currentStageNumber);
	}
	
	public void GoToStage(int stageNumber)
	{
		currentStageNumber=stageNumber;
		GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
		for(int i=0;i<checkPoints.Length;i++)
		{
			if ((checkPoints[i].GetComponent("Stage") as Stage).stageNumber == currentStageNumber)
			{
				model.transform.position = checkPoints[i].transform.position+new Vector3(0,1,0);
				break;
			}
		}
	}
}
