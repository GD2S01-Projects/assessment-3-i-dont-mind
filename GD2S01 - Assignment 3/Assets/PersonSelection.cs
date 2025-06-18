using System;
using System.Collections;
using System.Collections.Generic;
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
        if(!BuildingManager.Instance.GetActiveState())
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, Mathf.Infinity);

                if(hit.transform != null && hit.transform.TryGetComponent<Person>(out var person))
                {
                    visuals.SetActive(true);

                    personNameText.text = person.personName;
                    personAgeText.text = person.age.ToString();
                    personTypeText.text = Enum.GetName(typeof(PersonType), person.personType);
                    personHealthText.text = person.health.ToString();
                }else
                {
                    visuals.SetActive(false);
                }
            }
        }else
        {
            visuals.SetActive(false);
        }
    }
}
