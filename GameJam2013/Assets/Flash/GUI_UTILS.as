package 
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	
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
		}
	}
	
}