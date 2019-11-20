using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throwable : MonoBehaviour
{

    /* public stuff */
    public Collider2D player_collider;
    public GameObject guards;
    public Camera camera;
    public Text textPrompt;
    public float throw_force = 1.0f;
    public int collider_layer = 11;

    /* private stuff */
    private bool on_ground = true;
    private bool player_near = false;
    private bool dead = false;
    private const float rad_to_deg = 57.2957795f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider || !on_ground)
            return;
        textPrompt.GetComponent<Text>().text = "press F to pick up";
        textPrompt.GetComponent<Text>().enabled = true;
        player_near = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != player_collider || !on_ground)
            return;
        player_near = false;
        textPrompt.GetComponent<Text>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != collider_layer)
            return;
        dead = true;
        Vector2 pos = new Vector2(
            collision.transform.position.x,
            collision.transform.position.y
        );

        // notify guards
        foreach (Transform child in guards.transform) {
            child.gameObject.GetComponent<Guard>().notify_of_noise(pos, 1.0f);
        }

        // destroy object
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.gameObject);
    }

    // Update is called once per frame
    private int delay_counter;

    void Update()
    {

        if (dead)
            return;

        if (player_near && on_ground && Input.GetKey(KeyCode.F)) {
            on_ground = false;
            textPrompt.GetComponent<Text>().text = "press F to throw";
            transform.GetChild(0).gameObject.SetActive(false);
            delay_counter = 0;
            return;
        }

        if (delay_counter < 30) { 
            delay_counter++; 
            return;
        }

        if (!on_ground && Input.GetKey(KeyCode.F)) {
            textPrompt.GetComponent<Text>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            Vector3 player_pos = camera.WorldToScreenPoint(player_collider.transform.position);
            Vector3 mouse_pos = Input.mousePosition;
            float d_x = mouse_pos.x - player_pos.x;
            float d_y = mouse_pos.y - player_pos.y;
            float angle = (Mathf.Atan2(d_y, d_x) * rad_to_deg + 360) % 360;

            // move object to player
            Vector3 throw_pos = new Vector3(
                player_collider.transform.position.x + 0.5f*Mathf.Cos(angle / rad_to_deg),
                player_collider.transform.position.y + 0.5f*Mathf.Sin(angle / rad_to_deg),
                player_collider.transform.position.z
            );
            transform.position = throw_pos;
            transform.GetChild(0).position = throw_pos;
            GetComponent<Rigidbody2D>().AddForce((mouse_pos - player_pos).normalized * throw_force);

            print("throw " + angle);
            on_ground = true;
            dead = true;
        }
    }
}
