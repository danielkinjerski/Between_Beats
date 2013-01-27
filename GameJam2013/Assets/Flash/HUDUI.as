package  {
	
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.text.TextField;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	
	
	public class HUDUI extends MovieClip 
	{
		public const alphaDelta:Number = .05;
		public var levelTxt:TextField;
		public var fadeUp:Boolean;
		public var fadeDown:Boolean;
		
		public function HUDUI()
		{
			// constructor code
			levelTxt = getChildByName("txt_levelProgress") as TextField;
			levelTxt.alpha = 0;
			fadeDown = false;
			fadeUp = true;
		}
		
		public function ShowLevel(level :int)
		{
			levelTxt.text = "Level: " + level.toString();
			fadeUp = true;
			this.addEventListener(Event.ENTER_FRAME, UpdateAlpha);
		}
		
		public function UpdateAlpha(e:Event)
		{
			if (fadeUp)
			{
				levelTxt.alpha += alphaDelta;
				if (levelTxt.alpha  >= .99)
				{
					fadeUp = false;
					fadeDown = true;
				}
			}
			else if (fadeDown)
			{
				levelTxt.alpha -= alphaDelta;
				if (levelTxt.alpha <= .01)
				{
					fadeUp = false;
					fadeDown = false;					
				}
			}
			else
			{
				this.removeEventListener(Event.ENTER_FRAME, UpdateAlpha);
			}
		}
		
	}
	
}
