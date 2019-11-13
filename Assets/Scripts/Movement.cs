﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class Movement : MonoBehaviour
{

    /* public stuff */
    // movement
    public float movement_acceleration_coefficient;
    public float movement_drag_coefficient;
    public float movement_max_velocity;
    public float movement_shift_multiplier;

    // noise and visibility
    public float visibility_step_volume;
    public float visibility_step_sneak_multiplier;
    public float visibility_illumination_multiplier;
    public float visibility_illumination_offset;
    public GameObject visibility_static_lights;

    /* private stuff */
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    public NoiseAndVisibility playerNoiseAndVisibility = new NoiseAndVisibility();

    /* types */
    public struct NoiseAndVisibility {
        public float noise;
        public float visibility;
    }

    private void UpdateMovement() {
        // apply drag to velocity
        this.velocity.x *= 1 - movement_drag_coefficient;
        this.velocity.y *= 1 - movement_drag_coefficient;

        // apply velocity to position
        transform.position += this.velocity;
        
        // translate input to player movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        float dv_x = input.x * Time.deltaTime * this.movement_acceleration_coefficient;
        float dv_y = input.y * Time.deltaTime * this.movement_acceleration_coefficient;

        // apply change to velocity
        bool shift_down = Input.GetKey(KeyCode.LeftShift);
        dv_x *= shift_down ? movement_shift_multiplier : 1.0f;
        dv_y *= shift_down ? movement_shift_multiplier : 1.0f;
        this.velocity.x += (Mathf.Abs(dv_x + this.velocity.x) > movement_max_velocity) ? 0 : dv_x;
        this.velocity.y += (Mathf.Abs(dv_y + this.velocity.y) > movement_max_velocity) ? 0 : dv_y;
    }

    private void UpdateVisibility() {
        // Noise
        bool shift_down = Input.GetKey(KeyCode.LeftShift);
        playerNoiseAndVisibility.noise = this.visibility_step_volume 
                * (shift_down ? visibility_step_sneak_multiplier : 1.0f);
        playerNoiseAndVisibility.noise *= ((float)(velocity.magnitude) > 0.001f) ? 1.0f : 0.0f;
        
        // visibility
        float lightsum = 0.0f;
        foreach (Transform child in visibility_static_lights.transform){
            // calculate illumination from light
            Vector4 color = child.gameObject.GetComponent<LightSprite>().Color;
            float luminance = 0.3f * color.x + 0.59f * color.y + 0.11f * color.z; // red, green, blue components
            luminance *= color.w; // alpha component
            float dx = child.transform.position.x - transform.position.x;
            float dy = child.transform.position.y - transform.position.y;
            float distance = Mathf.Sqrt(dx*dx + dy*dy);

            // raycast to see if player is occluded
            CircleCollider2D player_collider = GetComponent<CircleCollider2D>();
            Vector2 dir = transform.position - child.position;
            float player_radius = player_collider.radius;
            float radius_offset = player_radius + 0.01f; // offset player radius
            RaycastHit2D hit = Physics2D.Raycast(child.position, dir, distance - (radius_offset)); 
            if (hit == null) {
                lightsum += luminance / (distance*distance); // inverse square fall-off
            } else {
                VisibilityOccluder visibility_occluder = hit.collider.gameObject.GetComponent<VisibilityOccluder>();
                bool hit_visibility_occluder = visibility_occluder != null;
                if (!hit_visibility_occluder) {
                    lightsum += luminance / (distance*distance); // inverse square fall-off
                } else {
                    float occlusion_multiplier = 1.0f - visibility_occluder.obstacle_multiplicative_alpha;
                    lightsum += (luminance / (distance*distance)) * occlusion_multiplier;
                }
            }
        }

        // TODO illumination from guard flashlights. Take flashlight cone shape into account.

        // add together.
        lightsum += visibility_illumination_offset;
        playerNoiseAndVisibility.visibility =
                visibility_illumination_offset +
                visibility_illumination_multiplier * lightsum;
        print(playerNoiseAndVisibility.noise);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateVisibility();
        // apply drag to velocity
    }
}
