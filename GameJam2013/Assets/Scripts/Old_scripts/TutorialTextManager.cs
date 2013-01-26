using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialTextManager : MonoBehaviour {

    bool isFinished = false;
    float timeCache = 0;
    public int index = 0;
    public List<GameObject> msg = new List<GameObject>();

	// Use this for initialization
	void OnEnable () {
        if (isFinished)
            return;

        for (int i = 0; i < msg.Count; i++)
        {
            if(i!=index)
            msg[i].active = false;
        }
        timeCache = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
        if (isFinished)
            return ;

        for (int i = 0; i < msg.Count; i++)
        {
            if (index > msg.Count || (i == 3 && GameManager.currentPlayMode == CurrentPlayMode.Grey))
            {
                isFinished = true;
            }
            if (index == i)
            {
                if (msg[i].active && ((Time.timeSinceLevelLoad - timeCache) % 60) > 14)
                {
                    msg[i].active = false;
                    timeCache = Time.timeSinceLevelLoad;
                    index++;
                }
                else if (!msg[i].active && ((Time.timeSinceLevelLoad - timeCache) % 60) > 1)
                {
                    msg[i].active = true;
                    timeCache = Time.timeSinceLevelLoad;
                }
                return;
            }

        }	
    }
}
