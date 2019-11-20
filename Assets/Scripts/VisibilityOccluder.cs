using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;

public class VisibilityOccluder : MonoBehaviour
{

    
    public float obstacle_multiplicative_alpha;
    public float obstacle_additive_alpha;

    // Start is called before the first frame update
    void Start()
    {
        this.obstacle_multiplicative_alpha = GetComponent<LightObstacleGenerator>().MultiplicativeColor.a;
        this.obstacle_additive_alpha = GetComponent<LightObstacleGenerator>().AdditiveColor.a;
    }

}
