using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	
	public CP.CPReceiverV4Mono receiver;

	// Use this for initialization
	void Start () {
		receiver = new CP.CPReceiverV4Mono();
		try{
			receiver.Connect("COM3");
		}
		catch (System.IO.IOException) 
		{
			Application.Quit();
			
			return;
		}
		System.Threading.Thread.Sleep(2000);
		receiver.Start();
	}
	
	// Update is called once per frame
	void Update () {
		receiver.PollData();
		Debug.Log("y:" + receiver.yTilt.ToString() + ", z:" + receiver.zTilt.ToString() + ", snd:" + receiver.soundVol.ToString() + 
		          ", left:" + receiver.left.ToString() + ", right:" + receiver.right.ToString());
	}
}
