using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBlock : BuildingClass
{
    // Start is called before the first frame update
    void Start()
    {
        buildingName = "Cell Block";
        buildingCost = 100;
        buildingMaxHousedPeople = 80;
        buildingCurrentHousedPeople = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LivingIn()
    {
        //for inamtes living in
    }
}
