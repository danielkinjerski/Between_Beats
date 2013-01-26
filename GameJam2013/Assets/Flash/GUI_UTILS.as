package 
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	import flash.events.Event;
	
	
	/**
	 * ...
	 * @author Zane Gholson
	 */
	public class GUI_UTILS
	{
		public static function MakeButton(tempBtn:Button, func:Function)
		{
			tempBtn.addEventListener(MouseEvent.CLICK, func);
			tempBtn.preventAutosizing = true;
			tempBtn.tabChildren = true;
			
		}
		
		public static function SetFocusOnButton(tempBtn:Button)
		{
			tempBtn.selected = true;
			tempBtn.setState("over");
		}
		
		public static function RemoveFocusOnButton(tempBtn:Button)
		{
			tempBtn.selected = false;
			tempBtn.setState("up");
		}
	}
	
}