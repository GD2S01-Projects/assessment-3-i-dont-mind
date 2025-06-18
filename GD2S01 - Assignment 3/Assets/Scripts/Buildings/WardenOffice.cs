using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WardenOffice : BuildingClass
{
    // Start is called before the first frame update
    void Start()
    {
        buildingName = "Warden Office";
        buildingCost = 100;
        buildingMaxHousedPeople = 8;
        buildingCurrentHousedPeople = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MonitorPrison()
    {
        //monitor the stats of the prison
    }
}
