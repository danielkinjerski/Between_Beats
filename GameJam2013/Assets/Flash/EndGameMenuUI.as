﻿package  {
	
	import flash.display.MovieClip;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	import flash.events.MouseEvent;
	
	
	public class EndGameMenuUI extends UIMenu {
		
		public var mainMenuBtn:Button;
		public var exitGameBtn:Button;
		
		public function EndGameMenuUI() {
			
			mainMenuBtn = getChildByName("btn_MainMenu") as Button;
			exitGameBtn = getChildByName("btn_ExitGame") as Button;
			
			GUI_UTILS.MakeButton(mainMenuBtn, OnMainMenuButtonClick);
			GUI_UTILS.MakeButton(exitGameBtn, OnExitButtonClick);
			
			buttonList = [mainMenuBtn, exitGameBtn];
			UpdateFocussedButton();
		}
		
		
		public function OnMainMenuButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).MainMenu();
			(root as UIManager).CloseEndGameMenu();
			(root as UIManager).OpenMainMenu();
		}
		
		
		
		
		public function OnExitButtonClick(e:MouseEvent)
		{
			//
			(root as UIManager).ExitGame();
			(root as UIManager).CloseEndGameMenu();
		}
		
		public function HandelConfirm()
		{
			if (mainMenuBtn.state == "over")
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
