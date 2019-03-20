using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject tipPanel;

    // Awake
    private void Awake()
    {
        GameObject t = GameObject.Find("TipPanel");
        if (t != null) tipPanel = t;
    }

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

    // Retry the last level
    public void Retry()
    {
        int level = PlayerPrefs.GetInt("currentLevel") + 1;
        string scene = "Level" + level;
        SceneManager.LoadScene(scene);

    }

    // Open Tips menu
    public void OpenTips()
    {
        tipPanel.SetActive(true);
        GameObject.Find("GameManager").GetComponent<GameManager>().setPaused(true);
    }

    // Close Tips menu
    public void CloseTips()
    {
        GameObject.Find("TipsPanel").SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().setPaused(false);
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }
}
