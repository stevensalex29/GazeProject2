using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Play game
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // How To Play
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    // Main Menu
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Next
    public void Next()
    {
        GameObject.Find("InstructionManager").GetComponent<InstructionManager>().Next();
    }

    // Previous
    public void Previous()
    {
        GameObject.Find("InstructionManager").GetComponent<InstructionManager>().Previous();
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }
}
