using UnityEngine;
using System.Collections;

public class TransformerTrigger : MonoBehaviour {
	
	public string becomesBallType;
	public GameObject becomesModel;
	Player player;
	RuntimeBall ball;
	GameObject model;
	bool pullBall = false;
	bool pushBall = false;
	GameObject playerGO;
	CPReceiverV5 receiver;
	
	void Start()
	{
		playerGO = GameObject.Find("Player");
		receiver = GameObject.Find("Player").GetComponent("CPReceiverV5") as CPReceiverV5;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			player = playerGO.GetComponent("Player") as Player;
			model = GameObject.Find("model");
			ball = model.GetComponent("RuntimeBall") as RuntimeBall;
			if(!(ball.ballType==becomesBallType))
			{
			StartCoroutine(doBallTransform());
			}
		}
	}
	
	/*void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			player = playerGO.GetComponent("Player") as Player;
			model = GameObject.Find("model");
			ball = model.GetComponent("RuntimeBall") as RuntimeBall;
			if((ball.ballType==becomesBallType))
			{
			pushBall=false;
			}
		}
	}*/
		
	Vector3 force;
	void Update()
	{
		if (pullBall && !(ball==null))
		{
			force=(gameObject.transform.position-model.transform.position)*600+new Vector3(0,200,0);
			model.rigidbody.AddForce(force);
		}
		if (pushBall && !(ball==null))
		{
			model.rigidbody.AddForce(new Vector3(0,model.rigidbody.mass*20,0));
		}
	}
	
	IEnumerator doBallTransform()
	{
		pullBall=true;
		yield return new WaitForSeconds(1);
		if (receiver) {StartMotor();}
		Parent.animation.Play("Close");
		yield return new WaitForSeconds(.5f);
		player.ChangeBall(becomesModel);
		yield return new WaitForSeconds(.5f);
		Parent.animation.Play("Open");
		yield return new WaitForSeconds(.5f);
		if (receiver) {StopMotor();}
		pullBall=false;
		pushBall=true;
		yield return new WaitForSeconds(1);
		pushBall=false;
	}
	
	void StartMotor()
	{
		receiver.StartMotor();
	}
	
	void StopMotor()
	{
		receiver.StopMotor();
	}
	
	GameObject Parent
	{
		get{return transform.parent.gameObject;}
	}
}