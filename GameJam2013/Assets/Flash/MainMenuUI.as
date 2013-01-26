package  {
	
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	import flash.external.ExternalInterface;
	
	
	public class MainMenuUI extends MovieClip {
		
		//exit and start
		public var exitButton:Button;
		public var startButton:Button;
		
		public function MainMenuUI() 
		{
			// constructor code
			exitButton = this.getChildByName("btn_Exit") as Button;
			startButton = this.getChildByName("btn_Start") as Button;
			GUI_UTILS.MakeButton(exitButton, OnExitButtonClick);
			GUI_UTILS.MakeButton(startButton, OnStartButtonClick);
		}
		
		public function OnStartButtonClick(e:MouseEvent)
		{
			//
			ExternalInterface.call("OnStartButtonClick");
		}
		
		public function OnExitButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).ExitGame();
		}
	}
	
}
