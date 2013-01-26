package  {
	
	import flash.display.DisplayObject;
	import flash.display.MovieClip;
	
	
	public class ProgressPanel extends MovieClip {
		
		public var indicator:DisplayObject;
		
		public function ProgressPanel() 
		{
			indicator = getChildByName("mc_positionIndicator");
		}
		
		public function UpdateIndicatorPos(percentComplete:Number)
		{
			indicator.x = width * percentComplete;
		}
	}
	
}
