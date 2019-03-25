using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    //Attribute 
    private string weakness;
    private int maxDamage;
    private int minDamage;
    private int[] weaknessSpell;
    private float regularPlayerDamage;
    private string name;

    private void Awake()
    {
        weakness = "warlockery";
        maxDamage = 20;
        minDamage = 15;
        weaknessSpell = new int[] { 1, 0, 3, 0, 2, 0 };
        regularPlayerDamage = 0.03f;
        name = "Demon";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public string getWeakness() { return weakness; }
    public int getMaxDamage() { return maxDamage; }
    public int getMinDamage() { return minDamage; }
    public int[] getWeaknessSpell() { return weaknessSpell; }
    public float getPlayerDamage() { return regularPlayerDamage; }
    public string getName() { return name; }

    // Update is called once per frame
    void Update()
    {

    }
}
