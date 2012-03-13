import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.*;


import lejos.pc.comm.*;
public class CPTransmitterV2 {
	static NXTConnector connection;
	static DataInputStream dis;
	static DataOutputStream dos;
	static DatagramSocket udpSocket;
	static int port = 23175;
	static InetAddress localhost;
	public static byte[] buffer;
	
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
			udpSocket = new DatagramSocket();
		}
		catch(SocketException e)
		{
			System.out.println("No socket!");
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
		

		(new Thread(new IPSender(udpSocket, localhost, port))).start();
		(new Thread(new IPReceiver(udpSocket,localhost,port))).start();
		
		buffer = new byte[4];
		
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
	
	public static void SendToNXT(byte data)
	{
		try {
			dos.write(data);
			dos.flush();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}

class IPSender implements Runnable
{
	DatagramSocket udpSocket;
	InetAddress localhost;
	int port;
	public IPSender(DatagramSocket udpSocket,InetAddress localhost, int port)
	{
		this.udpSocket = udpSocket;
		this.localhost = localhost;
		this.port = port;
	}
	
	public void run()
	{
		DatagramPacket packet;
		
		try
		{
			while(true)
			{
				if (!(CPTransmitterV2.buffer==null))
				{					
					packet = new DatagramPacket(CPTransmitterV2.buffer,CPTransmitterV2.buffer.length,localhost, port);
					udpSocket.send(packet);
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

class IPReceiver implements Runnable
{
	DatagramSocket udpSocket;
	InetAddress localhost;
	int port;
	public IPReceiver(DatagramSocket udpSocket,InetAddress localhost, int port)
	{
		this.udpSocket = udpSocket;
		this.localhost = localhost;
		this.port = port;
	}

	public void run()
	{
		DatagramPacket packet;
		try
		{
			while(true)
			{
				packet = new DatagramPacket(CPTransmitterV2.buffer,CPTransmitterV2.buffer.length,localhost, port);
				udpSocket.receive(packet);
				byte data = packet.getData()[0];
				System.out.println("Packet received: "+data);
				CPTransmitter.SendToNXT(data);
				Thread.sleep(33); 
			}
		}
		catch (IOException e) {System.out.println("Receive exception");}
		catch (InterruptedException e) {e.printStackTrace();
		}
	}
}