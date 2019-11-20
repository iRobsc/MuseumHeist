using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    /* public stuff */
    public Collider2D player_collider;
    public string next_scene;

    /* private stuff */
    bool isActive = false; 

    /*
     * Switch to next scene if level is completed
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider || !isActive)
            return;
        SceneManager.LoadScene(next_scene);
    }

    /*
     * Activate escape
     */
    public void activate() {
        this.isActive = true;
    }
}
