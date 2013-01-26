using UnityEngine;
using System.Collections;

public class Enemy : CharacterBasics {
	
	#region Fields
	/// <summary>
	/// The hitbox of the character
	/// </summary>
	public Collider hitbox;
    /// <summary>
    /// The attackbox of the character
    /// </summary>
    public Collider attackbox; 
	public GameObject target;
		
	/// <summary>
    /// Behavior collision check smoothing variables
    /// </summary>
	public float minDistance = 2, scopeDistance = 10, attackDistance = 2;
	
	   /// <summary>
    /// This contains a set of states that dicate whether a character is a player, enemy, etc
    /// </summary>
    public enum CharacterType { enemy, netural }
    public CharacterType charType = CharacterType.netural;
	#endregion

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(GameManager.gameState != GameState.PlayGame) return;
		
		if (charType == CharacterType.enemy)
		{
            this.gameObject.tag = "Enemy";
			if (CheckTargetDistance() <= scopeDistance)
				Seek(target.transform.position);
		}
		
		if (charType == CharacterType.netural)
            this.gameObject.tag = "Neutral";
	}
	
	/// <summary>
    /// Returns the distance this object is away from the target
    /// </summary>
    public virtual float CheckTargetDistance()
    {
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        return dist;
    }
	
	/// <summary>
    /// Check if target is inside this character's hitbox
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool HitBoxCheck(Collider t)
    {
        if (hitbox.collider.bounds.Intersects(t.bounds))
        {
           return true;
        }
        return false;
    }

    /// <summary>
    /// Check if target is inside this character's attackbox
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool AttackBoxCheck(Collider t)
    {
        if (attackbox.collider.bounds.Intersects(t.bounds))
        {
            //print("Something's in my hitbox");
            return true;
        }
        return false;
    }
	
	/// <summary>
    ///  Seek Behavior
    /// </summary>
    /// <param name="targetPos">target position</param>
    /// <returns>ing or not</returns>
	public bool Seek(Vector3 targetPos)
    {
        var tempDir = (targetPos - transform.position);
		//Debug.Log(tempDir.magnitude);
        Vector2 direction = new Vector3(tempDir.x, tempDir.z);
		
        //The distance between me and the target is greater than the minimum distance allowed.
        if ((tempDir.magnitude > minDistance))   
        {
			
            //Check for potential pitfalls
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.forward, Vector3.down, out hit, 3))
            {
                base.BaseMovement(direction,maxSpeed);
            }
            else //If there is a pit fall ahead, stop moving
            { base.BaseMovement(direction, 0); }
            return false;
        }
        ////Point towards the target.
        //transform.forward = new Vector3(tempDir.x, 0, tempDir.z);
        //speed = 0;
		return true;
	} 
}
