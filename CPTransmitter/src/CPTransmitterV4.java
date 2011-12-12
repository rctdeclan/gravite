import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.*;


import lejos.pc.comm.*;
public class CPTransmitterV4 {
	static NXTConnector connection;
	static DataInputStream dis;
	static DataOutputStream dos;
	static int outPort = 6767;
	static int inPort = 3434;
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
		
		(new Thread(new IPSenderV4(localhost))).start();
		(new Thread(new IPReceiverV4(localhost))).start();
		
		
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

class IPSenderV4 implements Runnable
{
	InetAddress localhost;
	DatagramPacket packet;
	DatagramSocket socket;
	public IPSenderV4(InetAddress localhost)
	{
		this.localhost = localhost;
		try {
			socket = new DatagramSocket(CPTransmitterV4.outPort);
		} catch (SocketException e) {
			System.out.println("OutPort Socket Exception");
		}
	}
	
	public void run()
	{
		try
		{
			while(true)
			{
				if (!(CPTransmitterV4.buffer==null))
				{					
					packet = new DatagramPacket(CPTransmitterV4.buffer,CPTransmitterV4.buffer.length,localhost, CPTransmitterV4.outPort);
					socket.send(packet);
					System.out.println("Packet send to port " + packet.getPort());
					Thread.sleep(33); 
					
				}
			}
		}
		catch (IOException e) {System.out.println("Send exception");}
		catch (InterruptedException e) {e.printStackTrace();
		}
	}
}

class IPReceiverV4 implements Runnable
{
	InetAddress localhost;
	DatagramPacket packet;
	DatagramSocket socket;
	public IPReceiverV4(InetAddress localhost)
	{
		this.localhost = localhost;
		try {
			socket = new DatagramSocket(CPTransmitterV4.inPort);
		} catch (SocketException e) {
			System.out.println("InPort Socket Exception");
		}
	}

	public void run()
	{
		try
		{
			while(true)
			{
				packet = new DatagramPacket(CPTransmitterV4.buffer,CPTransmitterV4.buffer.length,localhost,CPTransmitterV4.inPort);
				socket.receive(packet);
				byte[] data = packet.getData();
				System.out.println("Packet received from port "+packet.getPort()+" on IP "+packet.getAddress());
				CPTransmitter.SendToNXT(data[0]);
				Thread.sleep(33); 
			}
		}
		catch (IOException e) {System.out.println("Receive exception");}
		catch (InterruptedException e) {e.printStackTrace();
		}
	}
}