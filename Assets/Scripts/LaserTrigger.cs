using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{

    /* public stuff */
    public GameObject alarm_light_group;
    public Guard[] guards;

    /* private stuff */

    void OnTriggerEnter2D(Collider2D collider)
    {
        // call enable() for all alarm lights in the alarm lights group
        foreach (Transform child in alarm_light_group.transform) {
            child.gameObject.GetComponent<AlarmLight>().enable();
        }
        
        foreach (Guard guard in guards) {
            guard.detected = true;
        }
    }
    
}
