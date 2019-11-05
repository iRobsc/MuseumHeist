using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pixelate : MonoBehaviour
{

    /* public stuff */
    public Material effect;

    void OnRenderImage(RenderTexture src, RenderTexture dst) 
    {
        Graphics.Blit(src, dst, effect);
    }

}
