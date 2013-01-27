package  {
	
	import flash.display.Loader;
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.text.TextField;
	import flash.external.ExternalInterface;
	import flash.net.XMLSocket;
	
	
	public class TutorialMenu extends MovieClip {
		
		
		public var sourceText:String;
		public var tutorialText:TextField;
		public const timeBetweenLetters:Number = 5/*miliiseconds*/;
		public const timeBetweenFrames:Number = 1000/*miliiseconds*/;
		private var prevTime:Date;
		private var newDate:Date;
		private var sourceIndex:int = 0;
		private var textIndex:int = 0;
		private var timeDelta:Number;
		private var tempString:String;
		private var sourceComplete:Boolean = false;
		private var tutorialComeplete:Boolean = false;
		
		private var tutorialXML:XML;
		private var xmlLoader:URLLoader = new URLLoader();
		
		
		public function TutorialMenu() {
			// constructor code
			this.addEventListener(Event.ENTER_FRAME, Update);
			tutorialText = getChildByName("txt_tutorial") as TextField;
			tutorialText.text = "";
			xmlLoader.load(new URLRequest("Tutorial.xml"));
			xmlLoader.addEventListener(Event.COMPLETE, ProcessXML);
			prevTime = new Date();
		}
		
		public function ProcessXML(e:Event):void
		{
			tutorialXML = new XML(e.target.data);
			sourceText = tutorialXML.TutorialText[sourceIndex];
			trace("THIS IS THE XML", sourceText);
		}
		
		public function Update(e:Event)
		{
			if (tutorialXML != null)
			{
				if (!tutorialComeplete)
				{
					newDate = new Date();
					timeDelta =newDate.getTime() - prevTime.getTime();
					
					 if (!sourceComplete)
					 {
						if (timeDelta >= timeBetweenLetters)
						{
							if ((sourceText.length-1) > textIndex)
							{
								tempString = sourceText.substring(textIndex, textIndex+1);	
								tutorialText.appendText(tempString);
								textIndex++;
							}
							else
							{
								sourceComplete = true;
							}
							prevTime = newDate;
						}
					 }
					 else
					 {
						 if (timeDelta >= timeBetweenFrames)
						 {
							 NextStep();
							 prevTime = newDate;
						 }
					 }
				}
				else
				{
					this.removeEventListener(Event.ENTER_FRAME, Update);
				}
			}
			
		}
		
		public function AddLetterToTutorialText()
		{
			
		}
		
		public function NextStep()
		{
			tutorialText.text = "";
			sourceIndex ++;
			textIndex = 0;
			sourceComplete = false;
			sourceText= tutorialXML.TutorialText[sourceIndex];
			if (sourceText == null)
			{
				tutorialComeplete = true;
				TutorialComplete();
			}
		}
		
		public function TutorialComplete()
		{
			this.visible = false;
			ExternalInterface.call("TutorialComplete");
		}
	}
	
}
