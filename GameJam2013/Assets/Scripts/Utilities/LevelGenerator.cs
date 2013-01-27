using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : ScriptableObject {
	
	LevelGenerator _instance;
	public LevelGenerator Instance
	{
		get 
		{ 
		 if (_instance == null)
         {
            _instance = new LevelGenerator();
         }
         return _instance;
		}
	}
	
	float distancePerLevel = 30.0f;
	
	void placeBlock( List<GameObject> pieces ,int index )
	{
		//get a location within the play area
		Vector2 dir = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
		dir.Normalize();
		Vector3 pos = new Vector3(dir.x * (Random.Range(5, distancePerLevel + 30)), 0 , dir.y * (Random.Range(5, distancePerLevel+30)));
		
		
		//place the block and check if its colliding with anything
		pieces[index].transform.position = pos;
		for(int i = 0; i < pieces.Count ;i++)
		{
			if(i != index)
			{
				if(	pieces[index].collider.bounds.Intersects(pieces[i].collider.bounds))
				{
					placeBlock(pieces, index);
				}
			}
		}
	}
	
	public void GenerateLevel(int level, List<GameObject> pieces, GameObject finishNode)
	{
		foreach (GameObject piece in pieces) {
			piece.transform.position = Vector3.one * 55555;
		}
		
		for(int i = 0; i < pieces.Count; i++)
		{
			placeBlock(pieces, i);
		}
		
		bool finishNotPlaced = true;
		while(finishNotPlaced)
		{
			Vector2 dir = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
			dir.Normalize();
			Vector3 pos = new Vector3(dir.x * distancePerLevel , 0 , dir.y * distancePerLevel);
			
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast(new Vector3(pos.x, 20, pos.z), Vector3.down, out hit, 40))
			{
				if(hit.collider.tag != "block")
				{
					finishNode.transform.position = pos;
					finishNotPlaced = false;
				}
			}
		}
	}
}
