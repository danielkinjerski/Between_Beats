package  {
	
	import flash.display.MovieClip;
	import flash.external.ExternalInterface;
	
	public class UIManager extends MovieClip {
		
		private static var _UIManager:UIManager;
		public var _mainMenu:MainMenuUI;
		public var _pauseMenu:PauseMenuUI;
		public var _endGameMenu:EndGameMenuUI;
		public var _hudUI:HUDUI;
		
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
			_mainMenu = new MainMenuUI();
			this.addChild(_mainMenu);
		}
		public function CloseMainMenu()
		{
			this.removeChild(_mainMenu);
			_mainMenu = null;
		}
		
		public function OpenPauseMenu()
		{
			_pauseMenu = new PauseMenuUI();
			this.addChild(_pauseMenu );
		}
		public function ClosePauseMenu()
		{
			this.removeChild(_pauseMenu );
			_pauseMenu  = null;
		}
		
		
		public function OpenEndGameMenu()
		{
			_endGameMenu = new EndGameMenuUI();
			this.addChild(_endGameMenu);
		}
		public function CloseEndGameMenu()
		{
			this.removeChild(_endGameMenu);
			_endGameMenu = null;
		}
		
		public function OpenHUD()
		{
			_hudUI = new HUDUI();
			this.addChild(_hudUI);
		}
		public function CloseHUD()
		{
			this.removeChild(_hudUI);
			_hudUI = null;
		}
		
		
		public function ExitGame()
		{
			ExternalInterface.call("ExitGame");
		}
		
	}
	
}
