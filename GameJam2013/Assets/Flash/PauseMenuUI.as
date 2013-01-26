package  {
	
	import flash.display.MovieClip;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	import flash.external.ExternalInterface;
	import flash.events.MouseEvent;
	import flash.events.Event;
	import scaleform.clik.events.ButtonEvent;
	

	
	public class PauseMenuUI extends UIMenu {
		
		
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
			
			buttonList = [resumeGameBtn,mainMenuGameBtn,exitGameBtn];
			UpdateFocussedButton();
		}
		
		
		public function OnMainMenuButtonClick(e:MouseEvent)
		{
			(root as UIManager).MainMenu();
			(root as UIManager).ClosePauseMenu();
			(root as UIManager).OpenMainMenu();
		}
		
		
		public function OnResumeGameButtonClick(e:MouseEvent)
		{
			(root as UIManager).paused = false;	
			ExternalInterface.call("OnResumeGameButtonClick");
			(root as UIManager).ClosePauseMenu();
		}
		
		
		public function OnExitButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).ExitGame();
			(root as UIManager).ClosePauseMenu();
		}
		
		public function HandelConfirm()
		{
			if (resumeGameBtn.state == "over")
			{
				OnResumeGameButtonClick(null);
			}
			else if (mainMenuGameBtn.state == "over")
			{
				OnMainMenuButtonClick(null);
			}
			else if (exitGameBtn.state == "over")
			{
				OnExitButtonClick(null);
			}
		}
	}
	
}
