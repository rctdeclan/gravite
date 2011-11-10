using UnityEngine;
using System.Collections;

public class BlowerTrigger : MonoBehaviour {
	GameObject model;
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			model = GameObject.Find("model");
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			model = GameObject.Find("model");
			float force = Vector3.Distance(model.transform.position,gameObject.transform.position)*30;
			model.rigidbody.AddForce(0,force,0);
		}
	}
}
