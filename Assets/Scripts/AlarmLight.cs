using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{

    /* public stuff */
    public float rotate_speed;

    /* Turn on the lights */
    public void enable() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 180.0f))); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, rotate_speed));
    }
}
