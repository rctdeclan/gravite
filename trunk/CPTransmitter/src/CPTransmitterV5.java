import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.*;


import lejos.pc.comm.*;
public class CPTransmitterV5 {
	static NXTConnector connection;
	static DataInputStream dis;
	static DataOutputStream dos;
	static int outPort = 6767;
	static int inPort = 3434;
	static InetAddress localhost;
	public static byte[] buffer;
	public static byte toNxt;
	
	//4: Start
	//3: Exit
	//2: Stop motor
	//1: Start motor
	public static void main(String [] args)
	{
		connection = new NXTConnector();
		
		boolean connected = connection.connectTo("btspp://0016530A0CC4");
		if (!connected) {
			System.err.println("Not Connected.");
			System.exit(1);
		}
		
		dos = connection.getDataOut();
		dis = connection.getDataIn();
		
		
		try
		{
			localhost = InetAddress.getLocalHost();
		}
		catch(UnknownHostException e)
		{
			System.out.println("No localhost!");
		}
		
		try {
			Thread.sleep(2000);
			dos.writeByte(4);
			dos.flush();
			System.out.println("Started Sending");
			Thread.sleep(2000);
		} catch (IOException e1) {
			e1.printStackTrace();
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		

		buffer = new byte[4];
		
		(new Thread(new IPSenderV5(localhost))).start();
		(new Thread(new IPReceiverV5(localhost))).start();
		
		
		while(true)
		{
			try {
				length=dis.available();
				dis.read(buffer,0,length);
				for(int i=0;i<4;i++)
				{
					System.out.println(buffer[i]);
				}
			}
			catch (IOException e)
			{
				System.out.println("Read exception");
				break;
			} 
		}
	}
	static int length;
	
	public static void SendToNXT(byte b)
	{
		try {
			dos.write(b);
			dos.flush();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}

class IPSenderV5 implements Runnable
{
	InetAddress localhost;

	UDPOutputStream uos;
	public IPSenderV5(InetAddress localhost)
	{
		this.localhost = localhost;
		try {
			uos = new UDPOutputStream();
			uos.open(localhost,CPTransmitterV5.outPort);
		} catch (SocketException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	public void run()
	{
		try
		{
			while(true)
			{
				if (!(CPTransmitterV5.buffer==null))
				{					
					uos.write(CPTransmitterV5.buffer);
					uos.flush();
					System.out.println("Packet send");
					Thread.sleep(33);
				}
			}
		}
		catch (IOException e) {System.out.println("Send exception");}
		catch (InterruptedException e) {e.printStackTrace();
		}
	}
}

class IPReceiverV5 implements Runnable
{
	InetAddress localhost;
	UDPInputStream uis;
	public IPReceiverV5(InetAddress localhost)
	{
		this.localhost = localhost;
		try {
			uis = new UDPInputStream();
			uis.open("127.0.0.1", CPTransmitterV5.inPort);
		} catch (UnknownHostException | SocketException e) {
			e.printStackTrace();
		}
	}

	public void run()
	{
		try
		{
			while(true)
			{
				byte[] data = new byte[1];
				uis.read(data);
				System.out.println("Packet received");
				CPTransmitterV5.SendToNXT(data[0]);
				Thread.sleep(33); 
			}
		}
		catch (IOException e) {System.out.println("Receive exception");}
		catch (InterruptedException e) {e.printStackTrace();
		}
	}
}