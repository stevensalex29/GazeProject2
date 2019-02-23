using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    //Attributes
    public int row;
    public int column;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Get row position
    public int getRow()
    {
        return row;
    }

    // Set row position
    public void setRow(int r)
    {
        row = r;
    }

    // Get column position
    public int getColumn()
    {
        return column;
    }

    // Set row position
    public void setColumn(int c)
    {
        column = c;
    }

    // Set both row and column
    public void setRowColumn(int r, int c)
    {
        row = r;
        column = c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
