using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{

    public Collider2D player_collider;
    public GameObject group_to_affect;
    public bool action_enable;
    private bool player_near = false;

    void Start() {
        group_to_affect.SetActive(!action_enable);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider)
            return;
        player_near = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != player_collider)
            return;
        player_near = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (player_near && Input.GetKey(KeyCode.F)) {
            group_to_affect.SetActive(action_enable);
        }
    }
}
