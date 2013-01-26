package  {
	
	import flash.display.MovieClip;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	
	
	public class PauseMenuUI extends MovieClip {
		
		
		public var resumeGameBtn:Button;
		public var mainMenuGameBtn:Button;
		public var exitGameBtn:Button;
		
		public function PauseMenuUI() {
			resumeGameBtn = this.getChildByName("btn_ResumeGame");
			mainMenuGameBtn = this.getChildByName("btn_MainMenu");
			exitGameBtn = this.getChildByName("btn_ExitGame");
			
		}
	}
	
}
