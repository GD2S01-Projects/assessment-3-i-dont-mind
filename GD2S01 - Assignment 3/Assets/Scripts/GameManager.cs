/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : GameManager.cs
Description : Game manager that manags the players as well as their functions
Author : Reece Smith
Mail : reece.smtih@mds.ac.nz
*/


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject personPrefab;
    public Vector3 spawnLocation;

    public int money;

    float incomeTimer;

    public List<Person> people = new List<Person>();
    public List<Person> injuredPeople = new List<Person>();
    public List<GameObject> inmatePrefabInstances = new List<GameObject>();

    public List<Person> inmates = new List<Person>();
    public List<Person> guards = new List<Person>();
    public List<Person> doctors = new List<Person>();
    public BoxCollider spawnSquare;
    public int maxPeople = 20;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartFightChance());
        StartCoroutine(SpawnPersonChance());
        StartCoroutine(SearchInmateChance());
        StartCoroutine(SetContrabandChance());
        StartCoroutine(HealPatienChance());
        StartCoroutine(ReduceInmateSentenceCoRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        incomeTimer -= Time.deltaTime;
        if(incomeTimer <= 0f)
        {
            int incomeTally = inmates.Count * 25;

            OnScreenDebugger.DebugMessage($"Giving Player ${incomeTally} For Prisoner Income");

            AddIncome(incomeTally);

            incomeTimer = 30f;
        }
    }

    public void RegisterPerson(Person _person)
    {
        people.Add(_person);

        if (_person.decoratedPerson is Inmate) { inmates.Add(_person); }
        if (_person.decoratedPerson is Guard) { guards.Add(_person); }
        if (_person.decoratedPerson is Doctor) { doctors.Add(_person); }
    }
    
    //Returns false if its possible to spend that money
    public bool SpendMoney(int _amount)
    {
        if(money - _amount < 0)
        {
            return false;
        }

        OnScreenDebugger.DebugMessage($"Spent ${_amount}");

        money -= _amount;
        return true;
    }

    //Adds Income To The Player Money
    public void AddIncome(int _amount)
    {
        money += _amount;
    }

    public void SpawnInmate()
    {

        Bounds spawnBoxBounds = spawnSquare.bounds;

        float x = Random.Range(spawnBoxBounds.min.x, spawnBoxBounds.max.x);
        float z = Random.Range(spawnBoxBounds.min.z, spawnBoxBounds.max.z);
        float y = spawnBoxBounds.max.y;
        spawnLocation = new Vector3(x, y, z);
        GameObject inmateObject = Instantiate(personPrefab, spawnLocation, Quaternion.identity);

        Person personScript = inmateObject.GetComponent<Person>();
        personScript.personType = PersonType.Inmate;

        personScript.personName = "Inmate " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;
        if (personScript.decoratedPerson is Inmate inmate)
        {
            inmate.SetPrisonSentence(Random.Range(20, 100));
            inmate.SetHasContraband();
            inmate.SetContrabandSeverity();
        }
        inmatePrefabInstances.Add(inmateObject);
    }
    
    public void SpawnGuard() // spawning a guard and setting its vallues randomly
    {
        Bounds spawnBoxBounds = spawnSquare.bounds;

        float x = Random.Range(spawnBoxBounds.min.x, spawnBoxBounds.max.x);
        float z = Random.Range(spawnBoxBounds.min.z, spawnBoxBounds.max.z);
        float y = spawnBoxBounds.max.y;
        spawnLocation = new Vector3(x, y, z);
        GameObject guardObject = Instantiate(personPrefab, spawnLocation, Quaternion.identity);

        Person personScript = guardObject.GetComponent<Person>();
        personScript.personType = PersonType.Guard;

        personScript.personName = "Guard " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;

    }

    public void SpawnDoctor() // spawning a doctor and setting its vallues randomly
    {
        Bounds spawnBoxBounds = spawnSquare.bounds;

        float x = Random.Range(spawnBoxBounds.min.x, spawnBoxBounds.max.x);
        float z = Random.Range(spawnBoxBounds.min.z, spawnBoxBounds.max.z);
        float y = spawnBoxBounds.max.y;
        spawnLocation = new Vector3(x, y, z);
        GameObject doctorObject = Instantiate(personPrefab, spawnLocation, Quaternion.identity);

        Person personScript = doctorObject.GetComponent<Person>();
        personScript.personType = PersonType.Medical;

        personScript.personName = "Doctor " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;

    }

    public void SpawnPerson() // chooses what type of person to spawn
    {
        int personTypeIndex = Random.Range(0, 100);
        if (personTypeIndex < 33)
        {
            SpawnDoctor();
        }
        if (personTypeIndex > 33 && personTypeIndex < 66)
        {
            SpawnGuard();
        }
        if (personTypeIndex > 66)
        {
            SpawnInmate();
        }
    }

    private void StartInmateFight() // try start a fight between 2 inmates, if there are not enough inmates to start a fight then send debug message
    {
        if (inmates.Count < 2)
        {
            OnScreenDebugger.DebugMessage("Not enough inmates to start fight.");
                return;
        }

        int attackingInmateIndex = Random.Range(0, inmates.Count);
        Person attackingInmate = inmates[attackingInmateIndex];

        Person targetInmate;

        do
        {
            targetInmate = inmates[Random.Range(0, inmates.Count)];
        } while (targetInmate == attackingInmate);

        if (attackingInmate.decoratedPerson is Inmate inmateDecorator)
        {
            inmateDecorator.FightInmate(targetInmate);
            if (targetInmate.health <= 0)
            {
                injuredPeople.Add(targetInmate);
            }
        }
        
    }

    public void CheckInjured() // checks if there are injured people, and hela them
    {
        if (injuredPeople.Count == 0)
        {
            OnScreenDebugger.DebugMessage("No injured people.");
            return;
        }
        Person patient = injuredPeople[Random.Range(0, injuredPeople.Count)];
        if (doctors.Count == 0)
        {
            OnScreenDebugger.DebugMessage("No doctors.");
            return;
        }
        Person doctor = doctors[Random.Range(0, doctors.Count)];
        if (doctor == null)
        {
            OnScreenDebugger.DebugMessage("No doctors to heal patiens.");
        }
        if (patient == null)
        {
            OnScreenDebugger.DebugMessage("No patients to be healed.");
        }
        else
        {
            if (doctor.decoratedPerson is Doctor doc)
            {
                doc.HealPatient(patient);
            }
        }
    }

    public void CheckForContraband() // checks for inmates and searches them for contraband
    {
        if (inmates.Count == 0)
        {
            OnScreenDebugger.DebugMessage("No prisoners.");
            return;
        }
        Person inmate = inmates[Random.Range(0, inmates.Count)];
        if (guards.Count == 0)
        {
            OnScreenDebugger.DebugMessage("No Guards.");
            return;
        }

        Person guard = guards[Random.Range(0, guards.Count)];
        if (guard == null)
        {
            OnScreenDebugger.DebugMessage("No guards to check for contraband.");
            return;
        }
        if (inmate == null)
        {
            OnScreenDebugger.DebugMessage("No inmates to have contraband.");
            return;
        }
        else
        {
            if (guard.decoratedPerson is Guard gaurd)
            {
                gaurd.CheckContraband(inmate);
            }
        }
    }

    public void SetContraband() // chooses and inmate and sets the contraband for them
    {
        if (inmates.Count == 0)
        {
            OnScreenDebugger.DebugMessage("No inmates.");
            return;
        }
        Person inmate = inmates[Random.Range(0, inmates.Count)];
        if (inmate == null)
        {
            OnScreenDebugger.DebugMessage("No inmates to set contraband.");
            return;
        }
        if (inmate.decoratedPerson is Inmate prisoner)
        {
            if (prisoner.hasContraband)
            {
                OnScreenDebugger.DebugMessage("Inmate already has contraband.");
                return;
            }
            else
            {
                prisoner.SetHasContraband();
                prisoner.SetContrabandSeverity();
            }
        }
    }

    public void InmateReduceSentence() // reduces all inmates sentence by 1, if the sentece is finished, removes from list and destroy prefab instance
    {
        if (inmates.Count > 0)
        {
            for (int i = 0; i < inmates.Count; i++)
            {
                if (inmates[i].decoratedPerson is Inmate inmate)
                {
                    inmate.ReduceSentence(1);
                    if (inmate.GetPrisonSentece() <= 0)
                    {
                        GameObject.Destroy(inmatePrefabInstances[i]);
                        inmates.Remove(inmates[i]);
                        people.Remove(people[i]);
                        Destroy(people[i].gameObject);
                    }
                }
            }
        }
    }

    public IEnumerator StartFightChance() // chance every second to call fight function
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            float chance = Random.value;

            if (chance <= 0.05f)
            {
                StartInmateFight();
            }
        }
    }

    public IEnumerator SpawnPersonChance() // chance every second to call spawn person function
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float chance = Random.value;
            if (chance <= 0.1f && people.Count < maxPeople)
            {
                SpawnPerson();
            }
        }
    }

    public IEnumerator SearchInmateChance() // chance every second to call check for contraband function
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float chance = Random.value;
            if (chance <= 0.4f)
            {
                CheckForContraband();
            }
        }
    }

    public IEnumerator SetContrabandChance() // chance every second to call set contraband function
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float chance = Random.value;
            if (chance <= 0.5)
            {
                SetContraband();
            }
        }
    }

    public IEnumerator HealPatienChance() // chance every second to call checked on injured function
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float chance = Random.value;
            if (chance <= 0.5)
            {
                CheckInjured();
            }
        }
    }

    public IEnumerator ReduceInmateSentenceCoRoutine() // while running runs the inmate reduce sentence every second.
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            InmateReduceSentence();
        }
    }
}
