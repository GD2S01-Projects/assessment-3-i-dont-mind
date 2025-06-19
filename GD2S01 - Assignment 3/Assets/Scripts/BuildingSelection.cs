/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : BuildingSelection.cs
Description : Contains the implementation to be able to seelect a building type to be able to create
Author : Joe Rickwood
Mail : joe.rickwood@mds.ac.nz
*/

using UnityEngine;
using UnityEngine.UI;

public class BuildingSelection : MonoBehaviour
{
    private int index;
    private BuildingOption buildingOption;

    public Image iconImage;
    public Text nameText;
    public Text costText;


    public void Initialize(BuildingOption _option, int _index)
    {
        index = _index;
        buildingOption = _option;


        iconImage.sprite = _option.icon;
        nameText.text = _option.name;
        costText.text = $"${_option.prefab.GetComponent<BuildingClass>().buildingCost}";
    }

    public void Select()
    {
        BuildingManager.Instance.SetSelectedBuildingOption(index);
    }
}
