using UnityEngine;
using System.Collections;

public class SetLevel : MonoBehaviour {

    UILabel label;

    void Start()
    {
        label = GetComponent<UILabel>();
    }
    void OnEnable()
    {
        if (label != null)
        {
            label.text = "Level " + (GameManager.levelsCompleted + 1);
        }

    }
}
