using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class SpriteAnimation : MonoBehaviour
{
    [System.Serializable]
    public class SpriteAnim
    {
        public int begin;
        public int end;
        public string name;
        public bool playing = false;
    }
    public List<SpriteAnim> anims;
    public int columns = 2;
    public int rows = 2;
    public float framesPerSecond = 10f;


    //the current frame to display
    private int index = 0;

    void Start()
    {
        //StartCoroutine(updateTiling());

        //set the tile size of the texture (in UV units), based on the rows and columns
        Vector2 size = new Vector2(1f / columns, 1f / rows);
        renderer.sharedMaterial.SetTextureScale("_MainTex", size);
    }
    public void PlayAnim(string name)
    {
        for (int i = 0; i < anims.Count; i++ )
        {
            if (string.Compare(name, anims[i].name) == 0)
            {
                if (anims[i].playing)
                    return;
                StartCoroutine(updateTiling(i));                    
            }
        }


    }
    private IEnumerator updateTiling(int anim)
    {
        index = anims[anim].begin;
        while (index < anims[anim].end)
        {
            anims[anim].playing = true;
            //split into x and y indexes
            Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
                                          (index / columns) / (float)rows);          //y index

            renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);

            //move to the next index
            index++;

            yield return new WaitForSeconds(1f / framesPerSecond);
        }
        anims[anim].playing = false;


        yield return null;

    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            PlayAnim("Left");
        }
        foreach (SpriteAnim anim in anims)
        {
            if (GUILayout.Button(anim.name))
                PlayAnim(anim.name);
        }
    }
}