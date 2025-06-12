using System.Collections;
using System.Collections.Generic;
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
        costText.text = $"${_option.cost}";
    }

    public void Select()
    {
        Debug.Log(index);

        BuildingManager.Instance.SetSelectedBuildingOption(index);
    }
}
