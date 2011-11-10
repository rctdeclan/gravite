using UnityEngine;
using System.Collections;

public class StageTrigger : MonoBehaviour {
	GameObject playerGO;
	bool wasHere = false;
	void Start()
	{
		playerGO = GameObject.Find("Player");
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !wasHere)
		{
			Player player = playerGO.GetComponent("Player") as Player;
			wasHere=true;
			player.currentStageNumber++;
			if ((Parent.GetComponent("Stage") as Stage).isFinish)
			{
				player.lives=0;
			}
		}
	}
	
		GameObject Parent
	{
		get{return transform.parent.gameObject;}
	}
}
