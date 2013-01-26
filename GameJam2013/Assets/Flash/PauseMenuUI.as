package  {
	
	import flash.display.MovieClip;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	import flash.external.ExternalInterface;
	import flash.events.MouseEvent;
	
	public class PauseMenuUI extends MovieClip {
		
		
		public var resumeGameBtn:Button;
		public var mainMenuGameBtn:Button;
		public var exitGameBtn:Button;
		
		public function PauseMenuUI() {
			
			
			resumeGameBtn = this.getChildByName("btn_ResumeGame") as Button;
			mainMenuGameBtn = this.getChildByName("btn_MainMenu")as Button;
			exitGameBtn = this.getChildByName("btn_ExitGame") as Button;
			
			GUI_UTILS.MakeButton(resumeGameBtn, OnResumeGameButtonClick);
			GUI_UTILS.MakeButton(mainMenuGameBtn, OnMainMenuButtonClick);
			GUI_UTILS.MakeButton(exitGameBtn, OnExitButtonClick);
		}
		
		
		public function OnMainMenuButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).MainMenu();
		}
		
		
		public function OnResumeGameButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).paused = false;	
			ExternalInterface.call("OnResumeGameButtonClick");
		}
		
		
		public function OnExitButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).ExitGame();
		}
	}
	
}
