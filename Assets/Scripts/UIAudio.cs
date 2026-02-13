using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public GameManager gm;
    public AudioSource source;

    [Header("Audio Clips")]
    public AudioClip clip1;
    public AudioClip clip2;

    public void PlayClip1() 
    {
        if (!clip1) return;

        //if there is no game manager in the scene then just play the sound
        if(!gm) source.PlayOneShot(clip1);
        //if there is a game manager, check if the sound setting is on
        else if(gm.soundOn) source.PlayOneShot(clip1);
    }

    public void PlayClip2() 
    {
        if (!clip2) return;

        //if there is no game manager in the scene then just play the sound
        if (!gm) source.PlayOneShot(clip2);
        //if there is a game manager, check if the sound setting is on
        else if (gm.soundOn) source.PlayOneShot(clip2);
    }
}
