using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private UnityEngine.UI.Button thisBtn;
    public int level;
    public string scene;
    void Start()
    {
        thisBtn = gameObject.GetComponent<UnityEngine.UI.Button>();
        if (level < GameController.controller.levelsUnlocked)
        {
            thisBtn.interactable = false;
        }
    }

    public void loadScene()
    {
        SceneManager.LoadScene(scene);
        // GameController.controller.levelsUnlocked += 1; this should be added to completing a level
    }
    
    void Update()
    {
        if (level <= GameController.controller.levelsUnlocked)
        {
            thisBtn.interactable = true;
        } 
        else
        {
            thisBtn.interactable = false;
        }
    }
}
