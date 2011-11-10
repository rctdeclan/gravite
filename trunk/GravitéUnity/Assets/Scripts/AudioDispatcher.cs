using UnityEngine;
using System.Collections;

public class AudioDispatcher : MonoBehaviour {
public AudioClip[] sounds;
public float waitTime = 10;
	void Start()
	{
		InvokeRepeating("DoSound",1,waitTime);
	}
	
	
	void DoSound()
	{
		gameObject.audio.clip = sounds[(int) (Random.value*sounds.Length) % sounds.Length];
		gameObject.audio.Play();
		Debug.Log("Audio played");
	
	}
}
