using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;

public class CPReceiverV5 : MonoBehaviour
{
	public int inPort = 6767;
	public int outPort = 3434;
	UdpClient inClient;
	UdpClient outClient;
	IPEndPoint receivePoint;
	public long ipAdress = 192168171;
	byte[] inData;
	
	void Awake()
	{
		try
		{
			inData = new byte[4];
			inClient = new UdpClient(inPort);
			outClient = new UdpClient(outPort);
			receivePoint = new IPEndPoint(ipAdress,inPort);
		}
		catch(System.Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	void Update()
	{
		inData = inClient.Receive(ref receivePoint);
		if (Input.GetKeyDown("s")) 
		{StartMotor();Debug.Log("Start Motor");}	
	}
		
	public int zTilt
    {
        get { return (sbyte) inData[0];}
    }

    public int yTilt
    {
        get { return (sbyte) inData[1];}
    }

    public int soundVol
    {
        get { return inData[2]; }
    }

    public bool left
    {
        get { return inData[3] == 1 || inData[3] == 3; }
    }

    public bool right
    {
        get { return inData[3] == 2 || inData[3] == 3; }
    }
	
	
	public void Exit()
	{
		byte[] b = new byte[1];
		b[0] = 3;
		outClient.Send(b,1,"localhost",outPort);
	}
	public void StartMotor()
	{
		byte[] b = new byte[1];
		b[0] = 1;
		outClient.Send(b,1,"localhost",outPort);
	}
	
	public void StopMotor()
	{
		byte[] b = new byte[1];
		b[0] = 2;
		outClient.Send(b,1,"localhost",outPort);
	}
}
