using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
    #region Mono Inherit Functions
	
	public void Update()
	{
        if (GameManager.gameState != GameState.PlayGame)
            return;


        base.BaseMovement(InputMovement(), InputMovement().magnitude );
		if (Input.GetButtonDown("Jump"))
		{
			base.Launch();
		}
        if (Input.GetButtonDown("Rush"))
        {
            Debug.Log("RUSH");
            base.Rush(trans.forward, 10);
        }
	}

    #endregion

    #region Utilities

    /// <summary>
    /// Recieves the input from the axes
    /// </summary>
    /// <returns>
    /// The movement.
    /// </returns>
    private Vector2 InputMovement()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    #endregion


}
