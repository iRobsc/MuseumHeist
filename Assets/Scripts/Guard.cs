using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard : MonoBehaviour
{
    public Collider2D player_collider;
    private GameObject playerObject;
    public string next_scene;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = player_collider.gameObject;
    }

    /*
     * Switch to next scene if level is completed
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider)
            return;
        SceneManager.LoadScene(next_scene);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaY = transform.position.y - playerObject.transform.position.y;
        float deltaX = transform.position.x - playerObject.transform.position.x;

        float posX = transform.position.x - 0.01f * (float)Math.Cos(Math.Atan2(deltaY, deltaX));
        float posY = transform.position.y - 0.01f * (float)Math.Sin(Math.Atan2(deltaY, deltaX));
        transform.position = new Vector3(posX, posY, 0);
    }
}
