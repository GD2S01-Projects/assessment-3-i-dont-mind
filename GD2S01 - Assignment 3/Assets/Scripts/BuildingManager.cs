/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : BuildingManager.cs
Description : Building Manager Handles All Logic To Be Able To Create Buildings Within The Game Environment
Author : Joe Rickwood
Mail : joe.rickwood@mds.ac.nz
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct BuildingOption //Building Option Tells The Factory And Building Manager About The Building To Create
{
    public string name;
    public Sprite icon;
    public int cost;

    public GameObject prefab;
}


//Building Factory Is Used To Create The Buildings In The Game
public static class BuildingFactory
{
    public static GameObject CreateBuilding(BuildingOption _buildingOption, Vector3 position)
    {
        GameObject buildingInstance = GameObject.Instantiate(_buildingOption.prefab, position, Quaternion.identity);

        if(buildingInstance.TryGetComponent<BuildingClass>(out var building))
        {
            building.Init();
        }

        return buildingInstance;
    }
}



public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public BuildingOption[] buildings;
    public int selectedBuilding;

    bool active;

    [Header("UI")]
    public GameObject buildingSelectionPrefab;
    public Transform buildingSelectionTransform;

    //Sets Static Instance So Other Objects Can use The Building Manager
    private void Awake() 
    {
        Instance = this;
    }

    //Creates The UI On the First Frame Of PLay
    private void Start() 
    {
        CreateUI();
    }

    //Sets Weather or Not Building Is Enabled
    public void SetActiveState(bool _state)
    {
        active = _state;
    }

    //Gets Weather Or Not Building Is Enabled
    public bool GetActiveState()
    {
        return active;
    }

    private void Update()
    {
        //Only Place Buildings If The Mouse Pointer is Not Over UI Elements
        if(Input.GetMouseButtonDown(0) && !IsPointerOverUIElement() && active)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            if(hit.transform != null)
            {
                PlaceBuilding(buildings[selectedBuilding], hit.point);
            }
        }
    }

    //Placed Building At A Position And Debugs To The Screen
    public void PlaceBuilding(BuildingOption _building, Vector3 _position)
    {
        GameObject cur = BuildingFactory.CreateBuilding(_building, _position);

        OnScreenDebugger.DebugMessage($"Placed New Building At Position {_position}");
    }

    //Placed Building At A Position Using A String As Type Input And Debugs To The Screen
    public void PlaceBuilding(string _buildingString, Vector3 _position)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].name == _buildingString)
            {
                GameObject cur = BuildingFactory.CreateBuilding(buildings[i], _position);

                OnScreenDebugger.DebugMessage($"Placed New Building At Position {_position}");

                return;
            }
        }
    }

    //Used To Select Which Building To Place Down
    public void SetSelectedBuildingOption(int _option)
    {
        selectedBuilding = _option;
    }

    //Creates The Building Manager User Interface
    public void CreateUI()
    {
        //Clear Previous UI
        for (int i = 0; i < buildingSelectionTransform.childCount; i++)
        {
            Destroy(buildingSelectionTransform.GetChild(i).gameObject);
        }

        //Then Create The New UI
        for (int i = 0; i < buildings.Length; i++)
        {
            GameObject cur = Instantiate(buildingSelectionPrefab, buildingSelectionTransform);

            cur.GetComponent<BuildingSelection>()?.Initialize(buildings[i], i);
        }
    }

    //Checks If The Users Cursor Is Over Any User Interface, 
    //Helpful in order to check if the building manager should build something there or not
    public bool IsPointerOverUIElement()
    {
        List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();

        for (int index = 0; index < eventSystemRaysastResults.Count; ++index)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];

            //5 is the UI Layer
            if (curRaysastResult.gameObject.layer == 5)
            {
                return true;
            }
        }

        return false;
    }

    //Gets The Current List Of Raycast results From The Mouse Cursor Onto UI Objects
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
