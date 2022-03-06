using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour
{
    public float maxHealthAdj = 10.0f;
    public float curMaxHealth;

    public ABC_StateManager abcStateManager;

    // Start is called before the first frame update
    void Awake()
    {
        abcStateManager = transform.GetComponent<ABC_StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayMaxHealth();
    }

    void DisplayMaxHealth()
    {
        curMaxHealth = abcStateManager.currentMaxHealth;
    }
}
