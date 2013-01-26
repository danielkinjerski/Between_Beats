package  {
	
	import flash.display.MovieClip;
	import flash.external.ExternalInterface;
	
	public class UIManager extends MovieClip {
		
		private static var _UIManager:UIManager;
		public var _mainMenu:MainMenuUI;
		public var _pauseMenu:PauseMenuUI;
		public var _endGameMenu:EndGameMenuUI;
		public var _hudUI:HUDUI;
		
		public var paused:Boolean = false;
		
		public static function Instance() :UIManager //also known as root.  Also ROOT works better
		{
			if(_UIManager == null)
			{
				_UIManager = new UIManager();
				
			}
			
			return _UIManager;
			
		}
		
		
		
		
		public function UIManager() 
		{
			if(_UIManager != null)
			{
				throw new Error("THE UIMANAGER already exists. stupid");
			}
			Init();
			
		}
		
		public function Init()
		{
			OpenMainMenu();
		}
		
		public function OpenMainMenu()
		{
			if (_mainMenu == null)
			{
				_mainMenu = new MainMenuUI();
				this.addChild(_mainMenu);
			}
			else
			{
				_mainMenu.visible = true;
			}
		}
		public function CloseMainMenu()
		{
			if (_mainMenu != null)
			{
				_mainMenu.visible = false;
			}
		}
		
		public function OpenPauseMenu()
		{
			paused = true;
			if (_pauseMenu == null)
			{
				_pauseMenu = new PauseMenuUI();
				this.addChild(_pauseMenu );
			}
			else {
				_pauseMenu.visible = true;
			}
		}
		public function ClosePauseMenu()
		{
			if (_pauseMenu != null)
			{
				_pauseMenu.visible = false;
			}
		}
		
		
		public function OpenEndGameMenu()
		{
			if (_endGameMenu == null)
			{
				_endGameMenu = new EndGameMenuUI();
				this.addChild(_endGameMenu);
			}
			else
			{
				_endGameMenu.visible = true;
			}
		}
		public function CloseEndGameMenu()
		{
			if (_endGameMenu != null)
			{
				_endGameMenu.visible = false;
			}
		}
		
		public function OpenHUD()
		{
			if (_hudUI == null)
			{
				_hudUI = new HUDUI();
				this.addChild(_hudUI);
			}
			else
			{
				_hudUI.visible = true;
			}
		}
		public function CloseHUD()
		{
			if (_hudUI != null)
			{
				_hudUI.visible = true;
			}
		}
		
		
		public function ExitGame()
		{
			ExternalInterface.call("ExitGame");
		}
		
		public function MainMenu()
		{
			ExternalInterface.call("OnMainMenuClick");
		}
		
		public function PauseGame()
		{
			OpenPauseMenu();
		}
		
	}
}
