using UnityEngine;
using System.Collections;
using Scaleform;

public class ScaleformGamePad : SFGamepad
{
	public GameManager gManager;
	private const double joystickVertThreshold = .1;
	private const double joystickVertThresholdDefault = .5;
	private bool downKeyReleased;
	private bool upKeyReleased;
	
	public ScaleformGamePad() { }

    public ScaleformGamePad(SFManager sfMgr)
    {
        SFMgr = sfMgr;
    }
	
	public void Update()
	{
		for (int a = 0; a < Gamepads.Length; a++)
        {
			for (int b = 0; b < Gamepads[a].Joysticks.Length;b++ )
	        {
				if(gManager.menuOpen)
				{
					Gamepads[a].Joysticks[b].VerticalAxisThresholdD = joystickVertThreshold;
				}
				else
				{
					Gamepads[a].Joysticks[b].VerticalAxisThresholdD = joystickVertThresholdDefault;
				}
				
				if(/*Gamepads[a].Joysticks[b].DownKeyPressed*/Input.GetAxis(Gamepads[a].Joysticks[b].VerticalAxisName) < 0)
				{
					if(downKeyReleased)
					{
						downKeyReleased = false;
						gManager.scaleFormCamera.hud.HandelScrollPress(GameManager.gameState.ToString(), 1);
					}
				}
				else
				{
					downKeyReleased = true;
				}
				
				if(/*Gamepads[a].Joysticks[b].UpKeyPressed*/Input.GetAxis(Gamepads[a].Joysticks[b].VerticalAxisName) > 0)
				{
					if(upKeyReleased)
					{
						upKeyReleased = false;
						gManager.scaleFormCamera.hud.HandelScrollPress(GameManager.gameState.ToString(), -1);
					}
				}
				else
				{
					upKeyReleased = true;
				}
			}
			
		}
		base.Update();
		
	}

}

