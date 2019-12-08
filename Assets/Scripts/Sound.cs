using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public AudioSource source;
    public AudioClip alarm_clip;

    private bool alarm_playing = false;

    public void play_alarm_sound() {
        if (alarm_playing)
            return;
        alarm_playing = true;
        source.clip = alarm_clip;
        source.Play();
    }

}
