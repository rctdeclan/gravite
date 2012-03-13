import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import lejos.nxt.LCD;
import lejos.nxt.Motor;
import lejos.nxt.MotorPort;
import lejos.nxt.NXTMotor;
import lejos.nxt.SensorPort;
import lejos.nxt.Sound;
import lejos.nxt.SoundSensor;
import lejos.nxt.TouchSensor;
import lejos.nxt.comm.BTConnection;
import lejos.nxt.comm.Bluetooth;
import lejos.nxt.addon.*;
public class CPSenderV1 {
	public static CPSenderV1 instance;
	static String VERSION = "1.0.0";
	
	BTConnection btc;
	DataInputStream dis;
	DataOutputStream dos;
	
	NXTMotor mot;
	AccelHTSensor a;
	TouchSensor left;
	TouchSensor right;
	SoundSensor sound;
	
	int yTilt,zTilt;
	int soundVol;
	byte touch = 0;
	
	public CPSenderV1()
	{
		LCD.drawString("Cyborg Physicals",0,0);
		LCD.drawString("Version "+VERSION, 0, 1);
		LCD.drawString("WILD WIREWORKS",0,2);
		LCD.refresh();
		SetupSensActuators();
		waitForConnection();
		Step();
		Disconnect();
	}

	public void SetupSensActuators()
	{
		mot = new NXTMotor(MotorPort.A);
		a = new AccelHTSensor(SensorPort.S1);
		left = new TouchSensor(SensorPort.S2);
		right = new TouchSensor(SensorPort.S3);
		sound = new SoundSensor(SensorPort.S4);
	}
	
    public void waitForConnection()
    {
    	LCD.drawString("Waiting for connection",0,4);
    	Sound.beepSequence();
		LCD.refresh();

        btc = Bluetooth.waitForConnection();
        dis = btc.openDataInputStream();
    	dos = btc.openDataOutputStream();
		LCD.clear();
		LCD.drawString("Connected to PC",0,4);
		Sound.twoBeeps();
		LCD.refresh();	
    }
    Byte i;
    
    public void Step()
    {
    	//data specification:
		//in bytes: length,null,x,y,soundVol,touchSensors (7 bytes)
    	//TODO INCORPORATE MULTI THREADING? (ONE FOR VALUE READING AND ONE FOR BLUETOOTH I/O)
    	while (true)
    	{
    		//UPDATE ACCEL VALUES
    		yTilt = a.getYTilt();
    		zTilt = a.getZTilt();
    		soundVol = sound.readValue();
    		if (left.isPressed()) touch+=1;
    		if (right.isPressed()) touch+=2;
    		LCD.drawInt(yTilt, 0, 0);
    		LCD.drawInt(zTilt, 0 ,1);
    		LCD.refresh();
    		try
    		{
    			//READ BT
    			if (dis.available() > 0)
    			{
    				i=dis.readByte();
    				if (i==1) //FEEDBACK: TURN MOTOR.
    					Motor.A.rotate(1000);
    				if (i==2) //FEEDBACK: EXIT PROGRAM
    					break;
    				dis.skipBytes(dis.available());
    			}
    			//WRITE BT
    			dos.writeByte(yTilt);
    			dos.writeByte(zTilt);
    			dos.writeByte(soundVol);
    			dos.writeByte(touch);
    			dos.flush();
    		}
    		catch (IOException e)
    		{
    			LCD.drawString("BT Exception", 0,6);
    			break;
    		}	
    		touch = 0;
    		try {
				Thread.sleep(30);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				LCD.drawString("Thread Exception", 0,6);
				break;
			}
    	}
    }
    
    public void Disconnect()
    {
    	btc.close();
    }
	
	public static void main(String[] args)
	{
		instance = new CPSenderV1();
	}
}
