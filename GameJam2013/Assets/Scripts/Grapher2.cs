using UnityEngine;
using System.Collections.Generic;

public class Grapher2 : MonoBehaviour {

	public int resolution = 100;
	
	public Vector2 epiciCenter;
	public float area;

	private int currentResolution;
	private ParticleSystem.Particle[] points;
	
	public class shockWave
	{
		public Vector3 origin;
		public float creationTime;
		public float intensity;
		public float speed;
		public float peakDistance;
		public float lifeTime;
	}
	List<shockWave> waves;
	
	
	void Start () {
		waves = new List<shockWave>();
		CreatePoints();
	}

	private void CreatePoints () {
		if(resolution < 2){
			resolution = 2;
		}
		else if(resolution > 100){
			resolution = 100;
		}
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution * resolution];
		float increment = area / (resolution - 1);
		int i = 0;
		for(int x = 0; x < resolution; x++){
			for(int z = 0; z < resolution; z++){
				Vector3 p = new Vector3(x * increment, 0f, z * increment);
				points[i].position = p;
				points[i].color = new Color(p.x, 0f, p.z);
				points[i++].size = 0.1f;
			}
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.X))
		{
			createShockwave(epiciCenter, .25f,1, 5);
		}
		if((Time.time % 5) >= 4.8)
		{
			createShockwave(epiciCenter, .25f,1, 5);
		}
		if(currentResolution != resolution){
			CreatePoints();
		}
		for(int j = (waves.Count-1); j >= 0 ; j--)
		{
			shockWave aShockWave = waves [j];
			float pD = (Time.time - waves[j].creationTime) / aShockWave.speed ;
			aShockWave.peakDistance = pD;
			
			float t = Time.timeSinceLevelLoad;
			for(int i = 0; i < points.Length; i++){
				if(i == 1 )
				{					
					//print(">>>>>>>> " + aShockWave.peakDistance);
				}
				Vector3 p = points[i].position;
				//p.y = Ripple(p, t);
				p.y = shockWaveUpdate(aShockWave, p);
				points[i].position = p;
				Color c = points[i].color;
				c.g = p.y;
				points[i].color = c;
			}
			particleSystem.SetParticles(points, points.Length);
			if((Time.time-aShockWave.creationTime) > aShockWave.speed+.25f)
			{
				waves.Remove(waves[j]);
				print ("=========== " + waves.Count);
			}
		}
	}

	private float Ripple (Vector3 p, float t){
		float squareRadius = (p.x - epiciCenter.x) * (p.x - epiciCenter.x) + (p.z - epiciCenter.y) * (p.z - epiciCenter.y);
		return 0.5f + Mathf.Sin(15 * Mathf.PI * squareRadius - 2f * t) / (2f + 100f * squareRadius);
	}
	
	void createShockwave(Vector3 origin, float intensity, float speed, float lifeTime)
	{
		shockWave wave = new shockWave();
		wave.origin = origin;
		wave.intensity = intensity;
		wave.speed = speed;
		wave.creationTime = Time.time;
		wave.peakDistance = 0;
		wave.lifeTime = lifeTime;
		waves.Add(wave);
	}
	
	private float shockWaveUpdate(shockWave s, Vector3 p)
	{		
		float squareRadius = (p.x - s.origin.x) * (p.x - s.origin.x) + (p.z - s.origin.y) * (p.z - s.origin.y);
		if( squareRadius < .05)
		{
			return 0;
		}
		float distanceFromPeak = Mathf.Abs(s.peakDistance - squareRadius);
		float dt = Time.time - s.creationTime;
		return Mathf.Clamp( (s.intensity - (dt/s.lifeTime)) - distanceFromPeak,0,1);
	}
}