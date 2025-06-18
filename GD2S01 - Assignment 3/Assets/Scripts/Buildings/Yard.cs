using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yard : BuildingClass
{
    // Start is called before the first frame update
    void Start()
    {
        buildingName = "Yard";
        buildingCost = 200;
        buildingMaxHousedPeople = 120;
        buildingCurrentHousedPeople = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddInmateRecreation(int _Amount)
    {
        //Add Inmate Recreation amount to inmate
    }
}
