import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import lejos.nxt.Button;
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
import lejos.nxt.addon.AccelHTSensor;
public class CPSenderV2 {
	public static CPSenderV2 instance;
	static String VERSION = "2.0.0";
	
	BTConnection btc;
	DataInputStream dis;
	DataOutputStream dos;
	
	NXTMotor mot;
	AccelHTSensor a;
	TouchSensor left;
	TouchSensor right;
	SoundSensor sound;
	
	int yVal,zVal;
	byte yByte,zByte;
	byte soundVol;
	byte touch = 0;
	
	public CPSenderV2()
	{
		LCD.drawString("Cyborg Physicals",0,0);
		LCD.drawString("Version "+VERSION, 0, 1);
		LCD.drawString("WILD WIREWORKS",0,2);
		LCD.refresh();
		SetupSensActuators();
		waitForConnection();
		Step();
		try { Thread.sleep(2000); } catch (InterruptedException e) {}
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
    	
    	try {Thread.sleep(1000);} catch (InterruptedException e) {}
    	//data specification:
		//in bytes: length,null,y,z,soundVol,touchSensors (6 bytes)
    	//TODO INCORPORATE MULTI THREADING? (ONE FOR VALUE READING AND ONE FOR BLUETOOTH I/O)
    	while (!Button.ESCAPE.isPressed())
    	{
    		//UPDATE VALUES
    		yVal = a.getYAccel();
    		yByte = (byte) (0.635*(yVal-((yVal>500) ? 1024 : 0))); //cut of to min of -127?
    		zVal = a.getZAccel();
    		zByte = (byte) (0.635*(zVal-((zVal>500) ? 1024 : 0))); //cut of to max of 127?
    		soundVol = (byte) sound.readValue();
    		if (left.isPressed()) touch+=1;
    		if (right.isPressed()) touch+=2;
    		//WRITE VALUES TO SCREEN
    		LCD.clear();
    		LCD.drawInt(yByte, 0, 0);
    		LCD.drawInt(yVal, 5,0);
    		LCD.drawInt(zByte, 0 ,1);
    		LCD.drawInt(zVal, 5,1);
    		LCD.drawInt(soundVol, 0, 2);
    		LCD.drawInt(touch, 0,3);
    		LCD.refresh();
    		try
    		{
    			//READ BT
    			if (dis.available() > 0)
    			{
    				Sound.beep();
    				i=dis.readByte();
    				if (i==1) //FEEDBACK: START MOTOR
    					mot.setPower(100);
    				if (i==2) //FEEDBACK: STOP MOTOR
    					mot.setPower(0);
    				if (i==3) //FEEDBACK: EXIT PROGRAM
    					break;
    				dis.skipBytes(dis.available());
    			}
    			//WRITE BT
    			dos.writeByte(yByte);
    			dos.writeByte(zByte);
    			dos.writeByte(soundVol);
    			dos.writeByte(touch);
    			dos.flush();
    		}
    		catch (IOException e)
    		{
    			LCD.drawString("BT Exception", 0,6);
    			break;
    		}	
    		try {
				Thread.sleep(15);
			} catch (InterruptedException e) {}
    		touch = 0;
    	}
    }
    
    public void Disconnect()
    {
    	btc.close();
    }
	
	public static void main(String[] args)
	{
		instance = new CPSenderV2();
	}
}
