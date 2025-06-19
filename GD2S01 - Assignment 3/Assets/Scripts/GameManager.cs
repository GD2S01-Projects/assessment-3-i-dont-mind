using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject personPrefab;
    public Vector3 spawnLocation;

    public List<Person> people = new List<Person>();
    public List<Person> injuredPeople = new List<Person>();

    public List<Person> inmates = new List<Person>();
    public List<Person> guards = new List<Person>();
    public List<Person> doctors = new List<Person>();
    public BoxCollider spawnSquare;
    public int maxPeople = 20;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(StartFightChance());
        StartCoroutine(SpawnPersonChance());
        StartCoroutine(SearchInmateChance());
        StartCoroutine(SetContrabandChance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterPerson(Person _person)
    {
        people.Add(_person);

        if (_person.decoratedPerson is Inmate) { inmates.Add(_person); }
        if (_person.decoratedPerson is Guard) { guards.Add(_person); }
        if (_person.decoratedPerson is Doctor) { doctors.Add(_person); }
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

        personScript.personName = "Inamte " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;
        if (personScript.decoratedPerson is Inmate inmate)
        {
            inmate.SetPrisonSentence(Random.Range(1, 100));
            inmate.SetHasContraband();
            inmate.SetContrabandSeverity();
        }

    }
    public void SpawnGuard()
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

    public void SpawnDoctor()
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

    public void SpawnPerson()
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

    private void StartInmateFight()
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

    public void CheckInjured()
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

    public void CheckForContraband()
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

    public void SetContraband()
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

    public IEnumerator StartFightChance()
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

    public IEnumerator SpawnPersonChance()
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

    public IEnumerator SearchInmateChance()
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

    public IEnumerator SetContrabandChance()
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

}
