using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            collider.SendMessage("Respawn");
        else
        {
            Destroy(collider.gameObject);
            GameObject.Find("GameManager").SendMessage("GameOver");
        }
    }
}
