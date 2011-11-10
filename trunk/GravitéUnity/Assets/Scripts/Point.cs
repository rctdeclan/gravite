using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour {
	public int inStage;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			(GameObject.Find("Player").GetComponent("Player") as Player).points++;
			Destroy(gameObject);
			
		}
	}
	
	
}
