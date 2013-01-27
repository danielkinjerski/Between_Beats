using UnityEngine;
using System.Collections;

public class LevelGenerator : ScriptableObject {
	
	LevelGenerator _instance;
	public LevelGenerator Instance
	{
		get 
		{ 
		 if (_instance == null)
         {
            _instance = new Singleton();
         }
         return _instance;
		}
	}
	
	public int currentLevel;
	public float distancePerLevel;
	
	public GameObject blockPrefab;
	public GameObject finishNode;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GenerateLevel(int level)
	{
		currentLevel = level;
		
		int blocksToGenerate = Random.Range(0, currentLevel * 5);
		
		for(int i = 0; i < blocksToGenerate; i++)
		{
			Vector2 dir = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
			dir.Normalize();
			Vector3 pos = new Vector3(dir.x * (Random.Range(5, distancePerLevel + 30)), 0 , dir.y * (Random.Range(5, distancePerLevel+30)));
			
			RaycastHit hit;
			if(Physics.Raycast(new Vector3(pos.x, 20, pos.z), hit, 40))
			{
				if(hit.collider.tag != "block")
				{
					Instantiate(blockPrefab, pos, blockPrefab.transform.rotation); 
				}
			}
		}
		
		bool finishNotPlaced = true;
		while(finishNotPlaced)
		{
			dir = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
			dir.Normalize();
			pos = new Vector3(dir.x * distancePerLevel , 0 , dir.y * distancePerLevel);
			
			RaycastHit hit;
			if(Physics.Raycast(new Vector3(pos.x, 20, pos.z), hit, 40))
			{
				if(hit.collider.tag != "block")
				{
					Instantiate(finishNode, pos, finishNode.transform.rotation);
					finisheNotPlaced = false;
				}
			}
		}
	}
}
