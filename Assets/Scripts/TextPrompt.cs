using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrompt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // is needed to remove stutter for some reason.
        GetComponent<Text>().enabled = false;
    }

}
