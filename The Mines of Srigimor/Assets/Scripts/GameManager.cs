using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    //Attributes
    public GameObject blueJewel;
    public GameObject orangeJewel;
    public GameObject yellowJewel;
    public GameObject redJewel;
    public GameObject purpleJewel;
    public GameObject greenJewel;
    public GameObject enemy;
	private float heroHealth;
	private float enemyHealth;
    private int currentLevel;
	private int enemyAttack;
    private string enemyWeakness;
    private int[] enemyWeaknessSpell;
    private int enemyMaxDamage;
    private int enemyMinDamage;
    private float regularPlayerDamage;
    private string enemyName;
	[SerializeField] private healthBar healthBar;
	[SerializeField] private healthBar healthBarHero;
    GameObject[][] componentGrid;
    List<string> levels;


    List<GameObject> jewels;
    GameObject firstSelected;
    GameObject secondSelected;

    // Keep track of spell matches
    int blueMatches;
    int orangeMatches;
    int yellowMatches;
    int redMatches;
    int purpleMatches;
    int greenMatches;


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate levels
        levels = new List<string> {"Level1"};
        Scene currentScene = SceneManager.GetActiveScene(); //reset playerPrefs if at starting level
        string sceneName = currentScene.name;
        if (sceneName == "Level1")
        {
            PlayerPrefs.DeleteAll();
        }

        // Set enemy data
        string level = sceneName.Replace("Level", "");
        int lev = 0;
        Int32.TryParse(level,out lev);
        if(lev!=0)setEnemy(lev);

        if (PlayerPrefs.HasKey("currentLevel"))
            currentLevel = PlayerPrefs.GetInt("currentLevel"); //setup current level in playerprefs
        else
            PlayerPrefs.SetInt("currentLevel", 0);
        // Instantiate jewel list
        jewels = new List<GameObject>() { blueJewel, orangeJewel, yellowJewel, redJewel, purpleJewel, greenJewel };
        // Instantiate matches
        blueMatches = 0;
        orangeMatches = 0;
        yellowMatches = 0;
        redMatches = 0;
        purpleMatches = 0;
        greenMatches = 0;
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
		heroHealth = 1.0f;
		enemyHealth = 1.0f;
		enemyAttack = 5;
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
                GameObject g = Instantiate(getRandomJewel(), new Vector2(0,0), Quaternion.identity);
                g.GetComponent<GridPosition>().setColumn(k);
                g.GetComponent<GridPosition>().setRow(i);
                int row = i * -1;
                g.transform.SetPositionAndRotation(new Vector2(k - 7, row + 1), Quaternion.identity);
                componentGrid[i][k] = g;
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

			enemyAttack--;
            GameObject.Find("EnemyAttackCountText").GetComponent<Text>().text = "Enemy will attack in " + enemyAttack + " moves";

            if (enemyAttack == 0) {
				enemyAttack = 5;
                int damage = UnityEngine.Random.Range(enemyMinDamage, enemyMaxDamage); // does damage based on enemy script
				heroHealth -= (float)damage/100;
				healthBarHero.SetSize(heroHealth);
                GameObject.Find("EnemyAttackCountText").GetComponent<Text>().text = "Enemy attacked for " + damage + " damage";
            }
				
            // Check for matches and match
            match(checkMatch(), firstSelected.GetComponent<GridPosition>().getRow(), firstSelected.GetComponent<GridPosition>().getColumn());

            // Set selected as null
            setFirstSelected(null);
            setSecondSelected(null);
        }
    }

    // Finds match containing current row and column position
    public string[] findCurrMatch(List<string[]> matches,int r, int c)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            string[] m = matches[i];
            int m0Row, m0Col, m1Row, m1Col, m2Row, m2Col;
            m0Row = getStringRow(m[0]);
            m1Row = getStringRow(m[1]);
            m2Row = getStringRow(m[2]);
            m0Col = getStringCol(m[0]);
            m1Col = getStringCol(m[1]);
            m2Col = getStringCol(m[2]);
            if (m0Row == r && m0Col == c) return m;
            if (m1Row == r && m1Col == c) return m;
            if (m2Row == r && m2Col == c) return m;
        }
        return null;
    }

    // Gets the row number from the string 
    public int getStringRow(string s)
    {
        string[] pos = s.Split(',');
        return int.Parse(pos[0]);
    }

    // Gets the column number from the string 
    public int getStringCol(string s)
    {
        string[] pos = s.Split(',');
        return int.Parse(pos[1]);
    }

    // Update number of spell matches
    public void updateMatches(string type)
    {
        switch (type)
        {
            case "blue":
                blueMatches++;
                GameObject.Find("BlueMatches").GetComponent<Text>().text = "Blue Spells: " + blueMatches;
                break;
            case "yellow":
                yellowMatches++;
                GameObject.Find("YellowMatches").GetComponent<Text>().text = "Yellow Spells: " + yellowMatches;
                break;
            case "purple":
                purpleMatches++;
                GameObject.Find("PurpleMatches").GetComponent<Text>().text = "Purple Spells: " + purpleMatches;
                break;
            case "orange":
                orangeMatches++;
                GameObject.Find("OrangeMatches").GetComponent<Text>().text = "Orange Spells: " + orangeMatches;
                break;
            case "green":
                greenMatches++;
                GameObject.Find("GreenMatches").GetComponent<Text>().text = "Green Spells: " + greenMatches;
                break;
            case "red":
                redMatches++;
                GameObject.Find("RedMatches").GetComponent<Text>().text = "Red Spells: " + redMatches;
                break;
        }
    }

    // Invoke found matches
    public bool match(List<string[]> matches,int currR, int currC)
    {
        // Get current matched
        string[] match = findCurrMatch(matches,currR,currC);
        if (match==null) return false;

        // Update amount of matches
        updateMatches(componentGrid[getStringRow(match[0])][getStringCol(match[0])].tag);

        string type = "";

        if (getStringCol(match[0]) == getStringCol(match[1]))
        { // If in same column, match vertically
            int topRow = getStringRow(match[0]);
            int topCol = getStringCol(match[0]);
            type = componentGrid[topRow][topCol].tag;
            Vector2 matchP1 = componentGrid[topRow][topCol].transform.position;
            Vector2 matchP2 = componentGrid[topRow + 1][topCol].transform.position;
            Vector2 matchP3 = componentGrid[topRow + 2][topCol].transform.position;
            // Remove match from grid
            GameObject match1 = componentGrid[topRow][topCol];
            GameObject match2 = componentGrid[topRow+1][topCol];
            GameObject match3 = componentGrid[topRow+2][topCol];
            componentGrid[topRow][topCol] = null;
            componentGrid[topRow + 1][topCol] = null;
            componentGrid[topRow + 2][topCol] = null;
            // Destroy matched on screen
            Destroy(match1);
            Destroy(match2);
            Destroy(match3);
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
                componentGrid[i + 3][topCol].GetComponent<GridPosition>().setRowColumn(i + 3, topCol);
                p--;
            }
            // Replace top three with randoms
            GameObject n1 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            GameObject n2 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            GameObject n3 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            while (n1.tag == n2.tag && n1.tag == n3.tag) // Keep jewels random
            {
                Destroy(n1);
                Destroy(n2);
                Destroy(n3);
                n1 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
                n2 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
                n3 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            }
            n3.GetComponent<GridPosition>().setRowColumn(2, topCol);
            n3.transform.SetPositionAndRotation(new Vector2(topCol - 7, -1), Quaternion.identity);
            componentGrid[2][topCol] = n3;
            n2.GetComponent<GridPosition>().setRowColumn(1, topCol);
            n2.transform.SetPositionAndRotation(new Vector2(topCol - 7, 0), Quaternion.identity);
            componentGrid[1][topCol] = n2;
            n1.GetComponent<GridPosition>().setRowColumn(0, topCol);
            n1.transform.SetPositionAndRotation(new Vector2(topCol - 7, 1), Quaternion.identity);
            componentGrid[0][topCol] = n1;
        }
        else // Otherwise match horizontally
        {
            int frontCol= getStringCol(match[0]);
            int topRow = getStringRow(match[0]);
            // Remove match from grid
            type = componentGrid[topRow][frontCol].tag;
            GameObject match1 = componentGrid[topRow][frontCol];
            GameObject match2 = componentGrid[topRow][frontCol+1];
            GameObject match3 = componentGrid[topRow][frontCol+2];
            componentGrid[topRow][frontCol] = null;
            componentGrid[topRow][frontCol+1] = null;
            componentGrid[topRow][frontCol+2] = null;
            // Destroy matched on screen
            Destroy(match1);
            Destroy(match2);
            Destroy(match3);
            // Move other objects down
            int bottomRow = topRow - 1;
            for (int i = bottomRow; i > -1; i--)
            {
                componentGrid[i + 1][frontCol] = componentGrid[i][frontCol];
                componentGrid[i + 1][frontCol+1] = componentGrid[i][frontCol+1];
                componentGrid[i + 1][frontCol + 2] = componentGrid[i][frontCol + 2];
                componentGrid[i][frontCol] = null;
                componentGrid[i][frontCol+1] = null;
                componentGrid[i][frontCol+2] = null;


                componentGrid[i + 1][frontCol].transform.SetPositionAndRotation(new Vector2(frontCol - 7,-i), Quaternion.identity);
                componentGrid[i + 1][frontCol + 1].transform.SetPositionAndRotation(new Vector2(frontCol - 6, -i), Quaternion.identity);
                componentGrid[i + 1][frontCol + 2].transform.SetPositionAndRotation(new Vector2(frontCol - 5, -i), Quaternion.identity);

                componentGrid[i + 1][frontCol].GetComponent<GridPosition>().setRowColumn(i + 1, frontCol);
                componentGrid[i + 1][frontCol + 1].GetComponent<GridPosition>().setRowColumn(i + 1, frontCol + 1);
                componentGrid[i + 1][frontCol + 2].GetComponent<GridPosition>().setRowColumn(i + 1, frontCol + 2);
            }
            // Replace top three with randoms
            GameObject n1 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            GameObject n2 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            GameObject n3 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            while (n1.tag == n2.tag && n1.tag == n3.tag) // Keep jewels random
            {
                Destroy(n1);
                Destroy(n2);
                Destroy(n3);
                n1 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
                n2 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
                n3 = Instantiate(getRandomJewel(), new Vector2(0, 0), Quaternion.identity);
            }
            n3.GetComponent<GridPosition>().setRowColumn(0, frontCol + 2);
            n3.transform.SetPositionAndRotation(new Vector2(frontCol - 5, 1), Quaternion.identity);
            componentGrid[0][frontCol + 2] = n3;
            n2.GetComponent<GridPosition>().setRowColumn(0, frontCol + 1);
            n2.transform.SetPositionAndRotation(new Vector2(frontCol - 6, 1), Quaternion.identity);
            componentGrid[0][frontCol + 1] = n2;
            n1.GetComponent<GridPosition>().setRowColumn(0, frontCol);
            n1.transform.SetPositionAndRotation(new Vector2(frontCol - 7, 1), Quaternion.identity);
            componentGrid[0][frontCol] = n1;
        }

        // Do player damage
		if (enemyHealth > 0) {
            // Do more damage if cast spell of enemy weakness, otherwise do regular damage
            float damage = 0.0f;
            bool crit = false;
            if (checkDamageWeakness(type) &&!checkCritical()) // If weakness spell cast and critical not met, do weakness damage
            {
                damage = regularPlayerDamage + 0.05f;
                enemyHealth -= damage;
            }else if(!checkDamageWeakness(type) &&!checkCritical()) // If weakness spell not cast and critical not met, do regular damage
            {
                damage = regularPlayerDamage;
                enemyHealth -= damage;
            }
            else // Otherwise do critical damage
            {
                crit = true;
                reduceInventory(); // Reduce inventory on screen and in code
                damage = 0.40f; // Do critical damage
                enemyHealth -= damage;
                // (Zach)Do health bar size change

            }
            damage = damage * 100;
            int d = (int)damage;
            displayAttack(d, type,crit); // Display the attack on screen
            if (enemyHealth < 0.0f) enemyHealth = 0.0f;
			healthBar.SetSize (enemyHealth);
		}
        return true;
    }

    // Check for Matches
    public List<string[]> checkMatch()
    {
        List<string[]> matches = new List<string[]>();
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
                        string[] match = new string[3];
                        match[0] = i + "," + k;
                        match[1] = i + "," + (k + 1);
                        match[2] = i + "," + (k + 2);
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
                        // Add match to list of matches
                        string[] match = new string[3];
                        match[0] = i + "," + k;
                        match[1] = (i + 1) + "," + k;
                        match[2] = (i + 2) + "," + k;
                        matches.Add(match);
                    }
                }
            }
        }
        return matches;
    }

    // Checks if the player damage was enemy weakness
    public bool checkDamageWeakness(string matchType)
    {
        string type = "";
        switch (matchType)
        {
            case "red":
                type = "demonology";
                break;
            case "yellow":
                type = "warlockery";
                break;
            case "purple":
                type = "witchcraft";
                break;
            case "blue":
                type = "wizardry";
                break;
            case "green":
                type = "sorcery";
                break;
            case "orange":
                type = "theurgy";
                break;
        }
        return enemyWeakness == type;
    }

    // Gets the enemy data from enemy script
    public void setEnemy(int level)
    {
        GameObject.Instantiate(enemy, new Vector2(0, 0), Quaternion.identity);
        GameObject enem = GameObject.FindGameObjectWithTag("Enemy");
        enem.AddComponent<Enemy1>();
        switch (level)
        {
            case 1:
                enemyMaxDamage = enem.GetComponent<Enemy1>().getMaxDamage();
                enemyMinDamage = enem.GetComponent<Enemy1>().getMinDamage();
                enemyWeakness = enem.GetComponent<Enemy1>().getWeakness();
                enemyWeaknessSpell = enem.GetComponent<Enemy1>().getWeaknessSpell();
                regularPlayerDamage = enem.GetComponent<Enemy1>().getPlayerDamage();
                enemyName = enem.GetComponent<Enemy1>().getName();
                break;
        }
        GameObject.Find("EnemyName").GetComponent<Text>().text = "Enemy: " + enemyName;
        GameObject.Find("EnemyWeakness").GetComponent<Text>().text = "(Weak against " + enemyWeakness + " spells)";
        string weaknessSpell = "";
        for(int i = 0; i <enemyWeaknessSpell.Length; i++)
        {
            if (i == 0 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Blue: " + enemyWeaknessSpell[i] + " ";
            if (i == 1 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Orange: " + enemyWeaknessSpell[i] + " ";
            if (i == 2 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Yellow: " + enemyWeaknessSpell[i] + " ";
            if (i == 3 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Red: " + enemyWeaknessSpell[i] + " ";
            if (i == 4 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Purple: " + enemyWeaknessSpell[i] + " ";
            if (i == 5 && enemyWeaknessSpell[i] != 0) weaknessSpell += " Green: " + enemyWeaknessSpell[i] + " ";
        }
        GameObject.Find("CriticalSpell").GetComponent<Text>().text += " " + weaknessSpell;
    }

    // Displays the attack on screen
    public void displayAttack(int damage, string type,bool crit)
    {
        string t = "";
      
        switch (type)
        {
            case "red":
                t = "demonology";
                break;
            case "yellow":
                t = "warlockery";
                break;
            case "purple":
                t = "witchcraft";
                break;
            case "blue":
                t = "wizardry";
                break;
            case "green":
                t = "sorcery";
                break;
            case "orange":
                t = "theurgy";
                break;
        }
        if (crit)
        {
            GameObject.Find("PlayerAttack").GetComponent<Text>().text = "critical spell: + " + damage + " damage";
        }
        else
        {
            if (enemyWeakness == t)
            {
                GameObject.Find("PlayerAttack").GetComponent<Text>().text = "weakness spell: + " + damage + " damage";
            }else
            {
                GameObject.Find("PlayerAttack").GetComponent<Text>().text = t + " spell: + " + damage + " damage";
            }
        }
        
    }

    // Gets a random jewel from the jewel list
    public GameObject getRandomJewel()
    {
        return jewels[UnityEngine.Random.Range(0, jewels.Count)];
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

    // Check critical spell activated
    public bool checkCritical()
    {
        // Check that met spell criteria, true if so, false otherwise
        if (blueMatches >= enemyWeaknessSpell[0])
            if (orangeMatches >= enemyWeaknessSpell[1])
                if (yellowMatches >= enemyWeaknessSpell[2])
                    if (redMatches >= enemyWeaknessSpell[3])
                        if (purpleMatches >= enemyWeaknessSpell[4])
                            if (greenMatches >= enemyWeaknessSpell[5])
                                return true;
        return false;
    }

    // Reduce inventory after critical spell
    public void reduceInventory()
    {
        // Reduce spells in inventory
        blueMatches -= enemyWeaknessSpell[0];
        orangeMatches -= enemyWeaknessSpell[1];
        yellowMatches -= enemyWeaknessSpell[2];
        redMatches -= enemyWeaknessSpell[3];
        purpleMatches -= enemyWeaknessSpell[4];
        greenMatches -= enemyWeaknessSpell[5];
        // Display reduced spells
        updateInventoryDisplay();

    }

    // Updates the display of the inventory for the user
    public void updateInventoryDisplay()
    {
        GameObject.Find("BlueMatches").GetComponent<Text>().text = "Blue Spells: " + blueMatches;
        GameObject.Find("YellowMatches").GetComponent<Text>().text = "Yellow Spells: " + yellowMatches;
        GameObject.Find("PurpleMatches").GetComponent<Text>().text = "Purple Spells: " + purpleMatches;
        GameObject.Find("OrangeMatches").GetComponent<Text>().text = "Orange Spells: " + orangeMatches;
        GameObject.Find("GreenMatches").GetComponent<Text>().text = "Green Spells: " + greenMatches;
        GameObject.Find("RedMatches").GetComponent<Text>().text = "Red Spells: " + redMatches;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for next level when enemy dies
        if (enemyHealth == 0.0f) nextLevel();
    }

    // Move to the next level
    public void nextLevel()
    {
        currentLevel++;

        if (currentLevel == levels.Count)
        {
            PlayerPrefs.SetInt("currentLevel", 0);
            SceneManager.LoadScene("EndGame");
        }
        else
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            SceneManager.LoadScene(levels[currentLevel]);
        }
    }
}
