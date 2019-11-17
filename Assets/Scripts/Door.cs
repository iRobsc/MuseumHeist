using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    /* public stuff */
    public bool is_locked = false;
    public Collider2D player_collider;
    public Text textPrompt;
    public string prompt_text;
    public Collider2D prompt_collider;
    public Collider2D block_collider;
    public GameObject guards;
    
    /* private stuff */
    private float delta_time = 0.0f;
    private bool player_near = false;
    private int guards_near = 0;
    private bool is_open = false;
    private List<Collider2D> guard_colliders;

    void Start() {
        guard_colliders = new List<Collider2D>();
        foreach (Transform child in guards.transform) {
            guard_colliders.Add(child.GetComponent<BoxCollider2D>());
        }
    }

    private bool guard_near() {
        return guards_near > 0;
    }

    public bool is_blocked() {
        return transform.GetChild(0).GetComponent<DoorBlocker>().is_blocking;
    }

    public void try_open_door() {
        if (!is_blocked()) {
            is_open = true;
            transform.GetChild(0).RotateAround(transform.position, Vector3.forward, -80.0f);
            textPrompt.GetComponent<Text>().enabled = false;
        } else {
            textPrompt.GetComponent<Text>().text = "Door is blocked";
        }
    }

    public void close_door() {
        if(is_open) {
            is_open = false;
            transform.GetChild(0).RotateAround(transform.position, Vector3.forward, 80.0f);
        }
    }

    private int c = 0;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (is_open)
            return;

        if (collider == player_collider) {
            textPrompt.GetComponent<Text>().text = prompt_text;
            textPrompt.GetComponent<Text>().enabled = true;
            player_near = true;       
        }

        foreach (Collider2D guard_collider in guard_colliders) {
            if (collider == guard_collider) {
                guards_near++;
                break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == player_collider) {
            player_near = false;
            textPrompt.GetComponent<Text>().enabled = false;
        }
        
        foreach (Collider2D guard_collider in guard_colliders) {
            if (collider == guard_collider) {
                guards_near--;
                break;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(block_collider)

        if (player_near && Input.GetKey(KeyCode.F) && !is_open) {
            try_open_door();
        }

        if(guard_near() && !is_open) {
            try_open_door();
        }

        if (!guard_near() && !player_near && is_open) {
            close_door();
        }
    }
}
