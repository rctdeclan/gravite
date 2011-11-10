using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour 
{
	CPReceiverV5 receiver;
	bool leftIsPressed=false;
	bool rightIsPressed=false;
	public Direction startDirection;
	public float camHeight = 30;
	public Texture startupScreen;
	
	public enum Direction : int
	{
		North=0,
		East=1,
		South=2,
		West=3
	}
	
	Direction direction;
	
	public Direction Dir
	{
		get {return direction;}
		set 
		{
			iTween.RotateTo(this.gameObject,new Vector3(camHeight,getAngleFromDirection(value),0),3f);
			direction = value;
			Debug.Log("Direction set to "+value.ToString());
		}
	}
	
	void Awake () {
		StartCoroutine(DelayedStart());
		receiver = GameObject.Find("Player").GetComponent("CPReceiverV5") as CPReceiverV5;
	}
	bool drawSplash=true;
	void OnGUI()
	{
		
		if (drawSplash)
		{
			GUI.DrawTexture(new Rect(Screen.width/2-360,0,640,360),startupScreen,ScaleMode.ScaleToFit);
		}
		StartCoroutine(ShowSplash());
	}
	
	IEnumerator ShowSplash()
	{
		yield return new WaitForSeconds(3);
		drawSplash=false;
	}
	
	IEnumerator DelayedStart()
	{
		Debug.Log("Gravite Ver 3.1.4");
		yield return new WaitForSeconds(3);
		Dir = startDirection;
	}
	
	void Update () {
		if (Input.GetKeyDown("a")) TurnLeft();
		else if (Input.GetKeyDown("d")) TurnRight();
		
		if(receiver.left)
		{
			if (!leftIsPressed)
			{	
				leftIsPressed=true;
				TurnLeft();
			}
		}
		else
		{
			leftIsPressed=false;
		}
		
		if(receiver.right)
		{
			if (!rightIsPressed)
			{	
				rightIsPressed=true;
				TurnRight();
			}
		}
		else
		{
			rightIsPressed=false;
		}
	}
	
	void TurnRight()
	{
		switch (Dir)
		{
		case Direction.North: Dir=Direction.West;break;
		case Direction.East: Dir=Direction.North;break;
		case Direction.South: Dir=Direction.East;break;
		case Direction.West: Dir=Direction.South;break;
		}
	}
	
	void TurnLeft()
	{
		switch (Dir)
		{
		case Direction.North: Dir=Direction.East;break;
		case Direction.East: Dir=Direction.South;break;
		case Direction.South: Dir=Direction.West;break;
		case Direction.West: Dir=Direction.North;break;
		}
	}
	
	Direction getDirectionFromAngle(float angle)
	{
		angle = angle % 360;
		int iAngle = ((int) angle / 90);
		switch (iAngle)
		{
			case 0: return Direction.North;
			case 1: return Direction.East;
			case 2: return Direction.South;
			case 3: return Direction.West;
			default: return Direction.North;
		}
	}
	
	float getAngleFromDirection(Direction dir)
	{
		switch(dir)
		{
		case Direction.North: return 0;
		case Direction.East: return 90;
		case Direction.South: return 180;
		case Direction.West: return 270;
		default: return 0;
		}
	}
}