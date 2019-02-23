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
    GameObject firstSelected;
    GameObject secondSelected;


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate jewel list
        jewels = new List<GameObject>() { blueJewel, orangeJewel, yellowJewel, redJewel, purpleJewel, whiteJewel, greenJewel };
        // Instantiate selected
        firstSelected = null;
        secondSelected = null;
        // Instantiate component grid
        componentGrid = new GameObject[6][];
        componentGrid[0] = new GameObject[6];
        componentGrid[1] = new GameObject[6];
        componentGrid[2] = new GameObject[6];
        componentGrid[3] = new GameObject[6];
        componentGrid[4] = new GameObject[6];
        componentGrid[5] = new GameObject[6];
        // Create the starting grid for the level
        createGrid();
    }

    // Create initial level with prefabs
    void createGrid()
    {
        // Create grid with random jewels and display on screen
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                componentGrid[i][k] = getRandomJewel();
                componentGrid[i][k].GetComponent<GridPosition>().setColumn(k);
                componentGrid[i][k].GetComponent<GridPosition>().setRow(i);
                int row = i * -1;
                Instantiate(componentGrid[i][k], new Vector2(k-8,row+1), Quaternion.identity);
            }
        }
    }
    
    // Get the component grid
    public GameObject[][] getComponentGrid()
    {
        return componentGrid;
    }


    // Swap first selected and second selected components
    public void swap()
    {
        // If not null, swap components in grid and on screen
        if (firstSelected != null && secondSelected != null)
        {
            int firstRow = firstSelected.GetComponent<GridPosition>().getRow();
            int firstCol = firstSelected.GetComponent<GridPosition>().getColumn();
            int secondRow = secondSelected.GetComponent<GridPosition>().getRow();
            int secondCol = secondSelected.GetComponent<GridPosition>().getColumn();

            // Swap grid positions
            componentGrid[firstRow][firstCol] = componentGrid[secondRow][secondCol];
            secondSelected.GetComponent<GridPosition>().setRow(firstRow);
            secondSelected.GetComponent<GridPosition>().setColumn(firstCol);
            componentGrid[secondRow][secondCol] = firstSelected;
            firstSelected.GetComponent<GridPosition>().setRow(secondRow);
            firstSelected.GetComponent<GridPosition>().setColumn(secondCol);

            // Swap screen positions
            Vector2 fp = firstSelected.transform.position;
            Vector2 temp = fp;
            Vector2 sp = secondSelected.transform.position;
            firstSelected.transform.SetPositionAndRotation(sp, Quaternion.identity);
            secondSelected.transform.SetPositionAndRotation(temp, Quaternion.identity);

            // Delete selects after swap
            GameObject[] selects = GameObject.FindGameObjectsWithTag("select");
            Destroy(selects[0]);
            Destroy(selects[1]);

            //TODO THIS FUNCTION IS BROKE
            // Check for matches and match
            //match(checkMatch(), firstSelected.GetComponent<GridPosition>().getRow(), firstSelected.GetComponent<GridPosition>().getColumn());

            // Set selected as null
            setFirstSelected(null);
            setSecondSelected(null);
        }
    }

    // Finds match containing current row and column position
    public List<GameObject> findCurrMatch(List<List<GameObject>> matches,int r, int c)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            List<GameObject> m = matches[i];
            GameObject m0 = m[0];
            GameObject m1 = m[1];
            GameObject m2 = m[2];
            if (m0.GetComponent<GridPosition>().getRow() == r && m0.GetComponent<GridPosition>().getColumn() == c) return m;
            if (m1.GetComponent<GridPosition>().getRow() == r && m1.GetComponent<GridPosition>().getColumn() == c) return m;
            if (m2.GetComponent<GridPosition>().getRow() == r && m2.GetComponent<GridPosition>().getColumn() == c) return m;
        }
        return null;
    }

    // Invoke found matches
    public void match(List<List<GameObject>> matches,int currR, int currC)
    {
        // Get current matched
        List<GameObject> match = findCurrMatch(matches,currR,currC);
        if (match == null) return;

        if (match[0].GetComponent<GridPosition>().getColumn() == match[1].GetComponent<GridPosition>().getColumn())
        { // If in same column, match vertically
            int topRow = match[0].GetComponent<GridPosition>().getRow();
            int topCol = match[0].GetComponent<GridPosition>().getColumn();
            Vector2 matchP1 = match[0].transform.position;
            Vector2 matchP2 = match[1].transform.position;
            Vector2 matchP3 = match[2].transform.position;
            // Remove match from grid
            componentGrid[topRow][topCol] = null;
            componentGrid[topRow + 1][topCol] = null;
            componentGrid[topRow + 2][topCol] = null;
            // Remove game objects
            Destroy(match[0]);
            Destroy(match[1]);
            Destroy(match[2]);
            // Move other objects down
            int bottomRow = topRow - 1;
            int p = 2;
            for (int i = bottomRow; i > -1; i--)
            {
                componentGrid[i + 3][topCol] = componentGrid[i][topCol];
                componentGrid[i][topCol] = null;
                if (p == 0) componentGrid[i + 3][topCol].transform.SetPositionAndRotation(matchP1, Quaternion.identity);
                if (p == 1) componentGrid[i + 3][topCol].transform.SetPositionAndRotation(matchP2, Quaternion.identity);
                if (p == 2) componentGrid[i + 3][topCol].transform.SetPositionAndRotation(matchP3, Quaternion.identity);
                p--;
            }
            // Replace top three with randoms
            GameObject n1 = getRandomJewel();
            GameObject n2 = getRandomJewel();
            GameObject n3 = getRandomJewel();
            while (n1.tag == n2.tag && n1.tag == n3.tag) // Keep jewels random
            {
                n1 = getRandomJewel();
                n2 = getRandomJewel();
                n3 = getRandomJewel();
            }
            n3.GetComponent<GridPosition>().setRowColumn(2, topCol);
            componentGrid[2][topCol] = n3;
            Vector2 pos1 = new Vector2(topCol - 8, 3);
            Instantiate(componentGrid[2][topCol], pos1, Quaternion.identity);
            n2.GetComponent<GridPosition>().setRowColumn(1, topCol);
            componentGrid[1][topCol] = n2;
            Vector2 pos2 = new Vector2(topCol - 8, 2);
            Instantiate(componentGrid[1][topCol], pos2, Quaternion.identity);
            n1.GetComponent<GridPosition>().setRowColumn(0, topCol);
            componentGrid[0][topCol] = n1;
            Vector2 pos3 = new Vector2(topCol - 8, 1);
            Instantiate(componentGrid[0][topCol], pos3, Quaternion.identity);
        }
        else // Otherwise match horizontally
        {

        }
    }

    // Check for Matches
    public List<List<GameObject>> checkMatch()
    {
        List<List<GameObject>> matches = new List<List<GameObject>>();
        // Check for horizontal matches
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 6; k++)
            {
                string tag = componentGrid[i][k].tag;
                if (k + 1 < 6 && k + 2 < 6) // Make sure check is in bounds
                {   // Check for matches of 3 at each position of grid, horizontally
                    if (tag == componentGrid[i][k + 1].tag && tag == componentGrid[i][k + 2].tag)
                    {
                        // Add match to list of matches
                        List<GameObject> match = new List<GameObject>();
                        match.Add(componentGrid[i][k]);
                        match.Add(componentGrid[i][k+1]);
                        match.Add(componentGrid[i][k+2]);
                        matches.Add(match);
                    }
                }

            }
        }

        // Check for vertical matches
        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < 6; i++)
            {
                string tag = componentGrid[i][k].tag;
                if (i + 1 < 6 && i + 2 < 6) // Make sure check is in bounds
                {   // Check for matches of 3 at each position of grid, horizontally
                    if (tag == componentGrid[i + 1][k].tag && tag == componentGrid[i + 2][k].tag)
                    {
                        // Add match to list of matches
                        List<GameObject> match = new List<GameObject>();
                        match.Add(componentGrid[i][k]);
                        match.Add(componentGrid[i + 1][k]);
                        match.Add(componentGrid[i + 2][k]);
                        matches.Add(match);
                    }
                }
            }
        }
        return matches;
    }

    // Gets a random jewel from the jewel list
    public GameObject getRandomJewel()
    {
        return jewels[Random.Range(0, jewels.Count)];
    }

    // Set First Selected
    public void setFirstSelected(GameObject g)
    {
        firstSelected = g;
    }

    // Get first Selected
    public GameObject getFirstSelected()
    {
        return firstSelected;
    }

    // Set second Selected
    public void setSecondSelected(GameObject g)
    {
        secondSelected = g;
    }

    // Get second Selected
    public GameObject getSecondSelected()
    {
        return secondSelected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
