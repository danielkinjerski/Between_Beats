using UnityEngine;
using System.Collections;

public class Player : CharacterBasics {
	
    #region Mono Inherit Functions
	
	public void Update()
	{
        if (GameManager.gameState != GameState.PlayGame)
            return;


        BaseMovement(InputMovement(), InputMovement().magnitude );
		if (Input.GetKeyDown(KeyCode.Space)  || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			base.Launch();
		}
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            manager.SendMessage("Switch");
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
