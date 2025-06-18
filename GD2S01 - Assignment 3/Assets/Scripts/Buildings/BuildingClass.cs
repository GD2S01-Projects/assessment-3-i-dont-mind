using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    //Building values
    public string buildingName;
    public int buildingCost;
    public int buildingMaxHousedPeople;
    public int buildingCurrentHousedPeople;


    public void Init()
    {
        OnScreenDebugger.DebugMessage($"Initialised {buildingName}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
