using UnityEngine;
using System.Collections;

public class LifeTrigger : MonoBehaviour {
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			(GameObject.Find("Player").GetComponent("Player") as Player).lives++;
			Destroy(gameObject);
			
		}
	}
}
