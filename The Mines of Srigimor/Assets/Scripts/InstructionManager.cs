using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    // Attributes
    public List<GameObject> instructions;
    int currentIndex;
    GameObject currentInstruction;

    // Start is called before the first frame update
    void Start()
    {
        // Start at beginning of list
        currentIndex = 0;

        // Set starting instruction
        if (instructions.Count > 0)
        {
            currentInstruction = instructions[0];
            for(int i = 1; i < instructions.Count; i++) // Set all instructions other than current as not active
            {
                instructions[i].SetActive(false);
            }
        }
    }

    // Display next instructions in list
    public void Next()
    {
        // Return if nothing in list or no more instructions
        if (instructions.Count == 0) return;
        // Return if no more instructions
        if (currentIndex + 1 == instructions.Count) return;

        // Get and show current instructions, don't show previous instructions
        currentIndex++;
        currentInstruction.SetActive(false);
        currentInstruction = instructions[currentIndex];
        currentInstruction.SetActive(true);
    }

    // Display previous instructions in list
    public void Previous()
    {
        // Return if nothing in list or no more instructions
        if (instructions.Count == 0) return;
        // Return if no more instructions
        if (currentIndex -1 == -1) return;

        // Get and show current instructions, don't show last instructions
        currentIndex--;
        currentInstruction.SetActive(false);
        currentInstruction = instructions[currentIndex];
        currentInstruction.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
