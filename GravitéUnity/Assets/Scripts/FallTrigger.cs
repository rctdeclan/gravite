using UnityEngine;
using System.Collections;

public class FallTrigger : MonoBehaviour {

	GameObject playerGO;
	void Start()
	{
		playerGO = GameObject.Find("Player");
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			(playerGO.GetComponent("Player") as Player).Respawn();
		}
		else{
			Destroy(other.gameObject);
		}
	}
}
