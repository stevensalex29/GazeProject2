using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Attributes
    public GameObject blueJewel;
    public GameObject orangeJewel;
    public GameObject yellowJewel;
    public GameObject redJewel;
    public GameObject purpleJewel;
    public GameObject whiteJewel;
    public GameObject greenJewel;
    GameObject[][] componentGrid;
    List<GameObject> jewels;


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate jewel list
        jewels = new List<GameObject>() { blueJewel, orangeJewel, yellowJewel, redJewel, purpleJewel, whiteJewel, greenJewel };
        // Instantiate component grid
        componentGrid = new GameObject[8][];
        componentGrid[0] = new GameObject[8];
        componentGrid[1] = new GameObject[8];
        componentGrid[2] = new GameObject[8];
        componentGrid[3] = new GameObject[8];
        componentGrid[4] = new GameObject[8];
        componentGrid[5] = new GameObject[8];
        componentGrid[6] = new GameObject[8];
        componentGrid[7] = new GameObject[8];
        // Create the starting grid for the level
        createGrid();
    }

    // Create initial level with prefabs
    void createGrid()
    {
        // Create grid with random jewels and display on screen
        for(int i = 0; i < 8; i++)
        {
            for(int k = 0; k < 8; k++)
            {
                GameObject j = getRandomJewel();
                componentGrid[i][k] = j;
            }
        }
        display();
    }
    
    // Gets a random jewel from the jewel list
    public GameObject getRandomJewel()
    {
        return jewels[Random.Range(0, jewels.Count)];
    }

    // Display component grid on screen
    void display()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                GameObject j = componentGrid[i][k];
                Instantiate(j, new Vector2(i-5, k-5), Quaternion.identity);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
