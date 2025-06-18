using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoom : BuildingClass
{
    // Start is called before the first frame update
    void Start()
    {
        buildingName = "Staff Room";
        buildingCost = 200;
        buildingMaxHousedPeople = 20;
        buildingCurrentHousedPeople = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddStaffRecreation()
    {
        //Add staff recreation
    }
}
