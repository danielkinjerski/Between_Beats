using UnityEngine;
using System.Collections;

public class PreceptionSwitch : MonoBehaviour {
	
	public GameObject [] blackPieces;
	public GameObject[] whitePieces;
	
	public GameObject blackLayer;
	public GameObject whiteLayer;
	
	public bool showWhite = true;
	public bool showBlack = false;

	// Use this for initialization
	void Start () {
		HideWhitePieces();
		ShowBlackPieces();
	}
	
	// Update is called once per frame
	void HideWhitePieces () 
	{
		for (var i=0;i<whiteLayer.transform.childCount;i++)
    	{
			whiteLayer.transform.GetChild(i).gameObject.SetActiveRecursively(false);
			showWhite = false;	
		}
	}
	
	void ShowWhitePieces () 
	{
		for (var i=0;i<whiteLayer.transform.childCount;i++)
    	{
			whiteLayer.transform.GetChild(i).gameObject.SetActiveRecursively(true);
			showWhite = true;
		}
	}
	
		// Update is called once per frame
	void HideBlackPieces () 
	{
		for (var i=0;i<blackLayer.transform.childCount;i++)
    	{
			blackLayer.transform.GetChild(i).gameObject.SetActiveRecursively(false);
			showBlack = false;	
		}
	}
	
	void ShowBlackPieces () 
	{
		for (var i=0;i<blackLayer.transform.childCount;i++)
    	{
			blackLayer.transform.GetChild(i).gameObject.SetActiveRecursively(true);
			showBlack = true;
		}
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			if (showWhite)
			{
				HideWhitePieces();
				ShowBlackPieces();
			}
			else
			{
				ShowWhitePieces();
				HideBlackPieces();
			}
		}
	}
}
