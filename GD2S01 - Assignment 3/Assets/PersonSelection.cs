/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : PersonSelection.cs
Description : Contains the implementation to be able to select People within the prison and view thier information
Author : Joe Rickwood
Mail : joe.rickwood@mds.ac.nz
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class PersonSelection : MonoBehaviour
{
    [Header("UI")]
    public GameObject visuals;
    public Text personNameText;
    public Text personAgeText;
    public Text personTypeText;
    public Text personHealthText;

    private void Update()
    {
        if(!BuildingManager.Instance.GetActiveState()) //Only Allow Selection When The Building State Is Disabled
        {
            if(Input.GetMouseButtonDown(0)) //Only Spawn When Mouse Has Been Pressed
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, Mathf.Infinity); //Store All Raycast Data In A RaycastHit Variable

                if(hit.transform != null && hit.transform.TryGetComponent<Person>(out var person)) //If Raycast Hits Someone And Is Valid 
                {
                    visuals.SetActive(true); //Enable Visuals


                    //Update UI Visuals
                    personNameText.text = person.personName;
                    personAgeText.text = person.age.ToString();
                    personTypeText.text = Enum.GetName(typeof(PersonType), person.personType);
                    personHealthText.text = person.health.ToString();
                }else
                {
                    visuals.SetActive(false); //Disable Visuals
                }
            }
        }else
        {
            //Disable Visuals If Not In Selecting State
            visuals.SetActive(false);
        }
    }
}
