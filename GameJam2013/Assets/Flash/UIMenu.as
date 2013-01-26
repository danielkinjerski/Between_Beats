package 
{
	import flash.display.MovieClip;
	import flash.text.TextField;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	/**
	 * ...
	 * @author Zane Gholson
	 */
	public class UIMenu extends MovieClip
	{
		public var buttonList:Array;
		public var buttonIndex:int = 0;
		
		public function UIMenu()
		{
			
		}
		
		public function UpdateButtonIndex(delta:Number)
		{
			if (buttonList != null)
			{
				buttonIndex += delta;
				if (buttonIndex < 0)
				{
					buttonIndex = buttonList.length - 1;
				}
				else if (buttonIndex > buttonList.length -1)
				{
					buttonIndex = 0;
				}
				UpdateFocussedButton();
			}
			
		}
		
		public function UpdateFocussedButton()
		{
			if (buttonList != null)
			{
				for (var i:int = 0; i < buttonList.length; i++)
				{
					if (i == buttonIndex)
					{
						GUI_UTILS.SetFocusOnButton(buttonList[i]);
					}
					else
					{
						GUI_UTILS.RemoveFocusOnButton(buttonList[i]);
					}
				}
			}
		}
	}
	
}