using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public Collider2D player_collider;
    public Text textPrompt;
    public Camera camera;
    public string prompt_text;
    public GameObject escapes;

    /* private stuff */
    private bool player_near = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player_near && Input.GetKey(KeyCode.F)) {
            textPrompt.GetComponent<Text>().enabled = false;
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(this);

            foreach (Transform child in escapes.transform)
                child.gameObject.GetComponent<Escape>().activate();
        }
    }
}
