using UnityEngine;

public class Grapher2 : MonoBehaviour {

	public int resolution = 100;
	
	public Vector2 epiciCenter;
	public float area;

	private int currentResolution;
	private ParticleSystem.Particle[] points;

	void Start () {
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
		if(currentResolution != resolution){
			CreatePoints();
		}
		float t = Time.timeSinceLevelLoad;
		for(int i = 0; i < points.Length; i++){
			Vector3 p = points[i].position;
			p.y = Ripple(p, t);
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
		}
		particleSystem.SetParticles(points, points.Length);
	}

	private float Ripple (Vector3 p, float t){
		float squareRadius = (p.x - epiciCenter.x) * (p.x - epiciCenter.x) + (p.z - epiciCenter.y) * (p.z - epiciCenter.y);
		return 0.5f + Mathf.Sin(15 * Mathf.PI * squareRadius - 2f * t) / (2f + 100f * squareRadius);
	}
}