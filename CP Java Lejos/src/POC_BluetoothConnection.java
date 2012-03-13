import lejos.nxt.*;
import lejos.nxt.comm.*;
import java.io.*;
public class POC_BluetoothConnection {
	public static POC_BluetoothConnection instance;
	String connected = "Connected";
    String waiting = "Waiting...";
    String closing = "Closing...";

	DataInputStream dis;
	DataOutputStream dos;
	BTConnection btc;
	
	public void sendString(String message) throws IOException
	{
		dos.writeBytes("DECLAN BULLOCK RULES");
		dos.flush();		
	}
	
	public String receiveString() throws IOException
	{
		byte[] b = new byte[dis.available()];
		dis.read(b);
		return new String(b);
	}
    
    public void waitForConnection()
    {
    	LCD.drawString(waiting,0,0);
		LCD.refresh();

        btc = Bluetooth.waitForConnection();
        dis = btc.openDataInputStream();
    	dos = btc.openDataOutputStream();

		LCD.clear();
		LCD.drawString(connected,0,0);
		LCD.refresh();	
    }
    
    public void close()
    {
		LCD.drawString(closing,0,0);
		LCD.refresh();
		btc.close();
		LCD.clear();    	
    }
    
	public static void main(String [] args)  throws Exception 
	{
		String str;
		instance = new POC_BluetoothConnection();
		instance.waitForConnection();
		instance.sendString("Test Number One");
		while (true)
		{
			str=instance.receiveString();
			if (str!="")
			{
				LCD.drawString(str,0,0);
				instance.sendString("RECEIVED!" + str);
				break;
			}
			Thread.sleep(30);
		}
		instance.close();
	}
}
