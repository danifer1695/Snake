using UnityEngine;
using UnityEngine.UI;

public class VolumeToggleButton : MonoBehaviour
{
    public Image soundOn;
    public Image soundOff;
    public AudioSource source;

    public void ToggleIcons()
    {
        soundOn.enabled = !soundOn.enabled;
        soundOff.enabled = !soundOff.enabled;
        if(source) source.mute = !source.mute;
    }
}
