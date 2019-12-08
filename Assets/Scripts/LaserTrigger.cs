using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{

    /* public stuff */
    public GameObject alarm_light_group;
    public Transform guards;

    /* private stuff */

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        GameObject.Find("background_music").GetComponent<Sound>().play_alarm_sound();

        // call enable() for all alarm lights in the alarm lights group
        foreach (Transform child in alarm_light_group.transform) {
            child.gameObject.GetComponent<AlarmLight>().enable();
        }
        
        foreach (Transform guard in guards) {
            guard.gameObject.GetComponent<Guard>().detected = true;
        }
    }
    
}
