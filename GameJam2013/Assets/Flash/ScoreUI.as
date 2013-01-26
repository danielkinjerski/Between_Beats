package  {
	
	import flash.display.MovieClip;
	import flash.text.TextField;
	
	
	public class ScoreUI extends MovieClip {
		
		public var txt_Score:TextField;
		
		public function ScoreUI() {
			// constructor code
			txt_Score = getChildByName("txt_Score") as TextField;
		}
		
		public function UpdateScore(newScore:Number)
		{	
			txt_Score.text = newScore.toString();
		}
	}
	
}
