using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGMUSICScript : MonoBehaviour {

    public static BGMUSICScript Instance;
    [Range(0f, 1f)]
    public float BGVolume;

    public AudioClip[] BGMusic;
    
    // Use this for initialization
    void Start () {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
       
    }
    void Shuffle()
    {
        SceneSounds();
    }
   void OnEnable()
    {
        Shuffle();
        
    }
    public void SceneSounds()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
