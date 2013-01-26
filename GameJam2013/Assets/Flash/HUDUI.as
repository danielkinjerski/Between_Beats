package  {
	
	import flash.display.MovieClip;
	import flash.text.TextField;
	import scaleform.clik.core.UIComponent;
    import scaleform.clik.events.ButtonEvent;
    import scaleform.clik.constants.InvalidationType;
    import scaleform.clik.controls.Button;
	
	
	public class HUDUI extends MovieClip 
	{
		public var scorePanel:ScoreUI;
		public var progressPanel:ProgressPanel;
		
		public function HUDUI()
		{
			// constructor code
			scorePanel = getChildByName("mc_scorePanel") as ScoreUI;
			progressPanel = getChildByName("mc_progressPanel") as ProgressPanel;
		}
		
		
	}
	
}
