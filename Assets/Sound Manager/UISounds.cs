using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public void PlayUISound(string soundName)
    {
        GameObject.Find("AudioManager").SendMessage("play", soundName);
        Debug.Log(soundName);
    }
}
