using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafeteria : BuildingClass
{
    // Start is called before the first frame update
    void Start()
    {
        buildingName = "Cafeteria";
        buildingCost = 150;
        buildingMaxHousedPeople = 120;
        buildingCurrentHousedPeople = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FeedInmates()
    {
        //feed inmates
    }
}
