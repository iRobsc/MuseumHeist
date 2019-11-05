using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    
    // -------------------------------------------------
    // TODO generalize to do more stuff than disable
    // -------------------------------------------------

    /* public stuff */
    public Collider2D player_collider;
    public GameObject group_to_affect;
    public Text textPrompt;
    public Camera camera;
    public string prompt_text;

    /* private stuff */
    private bool player_near = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider || !group_to_affect.activeSelf)
            return;
        textPrompt.GetComponent<Text>().text = prompt_text;
        textPrompt.GetComponent<Text>().enabled = true;
        player_near = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != player_collider)
            return;
        player_near = false;
        textPrompt.GetComponent<Text>().enabled = false;
    }

    void Update() {
        if (player_near && Input.GetKey(KeyCode.F)) {
            textPrompt.GetComponent<Text>().enabled = false;
            // TODO generalize to do more stuff than disable
            group_to_affect.SetActive(false);
        }
    }

}
