using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerMenu : MonoBehaviour
{
    void Start()
    {
        GameObject musicGO = GameObject.FindGameObjectWithTag("Music");
        musicGO.GetComponent<MusicPlayer>().PlayMusic(0);
    }
}
