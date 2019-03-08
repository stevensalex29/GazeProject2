using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    //Attribute
    GameManager gm;
    public GameObject select;

    // Start is called before the first frame update
    void Start()
    {
        // Get the game manager for use
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Check for mouse down on jewels
    void OnMouseDown()
    {
        // As long as swapping is not taking place, allow user to select components
        if (!gm.getSwap() && !gm.getMatch()) {
            GameObject first = gm.getFirstSelected();
            GameObject second = gm.getSecondSelected();

            // Check if none selected yet, select first
            if (first == null)
            {
                if (gm.match(gm.checkMatch(), gameObject.GetComponent<GridPosition>().getRow(), gameObject.GetComponent<GridPosition>().getColumn()))
                {
                    return;
                }
                gm.setFirstSelected(gameObject);
                Instantiate(select, gameObject.transform.position, Quaternion.identity);
            }
            else if (first != null && second == null) // If first selected, select second
            {
                if (checkAdjacent(first, gameObject)) // Check if second selected is adjacent to the first in order to swap
                {
                    gm.setSecondSelected(gameObject);
                    Instantiate(select, gameObject.transform.position, Quaternion.identity);
                    gm.swap();
                }
                else if (first == gameObject) // If the same object was selected
                {
                    Destroy(GameObject.FindGameObjectWithTag("select"));
                    gm.setFirstSelected(null);
                }
                else // If not adjacent, select second as first
                {
                    Destroy(GameObject.FindGameObjectWithTag("select"));
                    gm.setFirstSelected(gameObject);
                    Instantiate(select, gameObject.transform.position, Quaternion.identity);
                }
            }
        }
    }

    // check if two Game Objects are adjacent
    public bool checkAdjacent(GameObject first, GameObject second)
    {
        int firstRow = first.GetComponent<GridPosition>().getRow();
        int firstCol = first.GetComponent<GridPosition>().getColumn();
        int secondRow = second.GetComponent<GridPosition>().getRow();
        int secondCol = second.GetComponent<GridPosition>().getColumn();

        // Check if first and second are near each other
        if (firstRow == secondRow)
        {
            if (firstCol - 1 == secondCol || firstCol + 1 == secondCol) return true;
        }else
        {
            if ((firstCol==secondCol) && (firstRow - 1 == secondRow || firstRow + 1 == secondRow)) return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
