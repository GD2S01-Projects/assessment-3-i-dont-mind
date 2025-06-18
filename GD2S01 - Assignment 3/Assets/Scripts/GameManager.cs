using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject personPrefab;
    public Transform spawnLocation;

    public List<Person> people = new List<Person>();
    public List<Person> injuredPeople = new List<Person>();

    public List<Person> inmates = new List<Person>();
    public List<Person> guards = new List<Person>();
    public List<Person> doctors = new List<Person>();
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(StartFightChance());
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
        GameObject inmateObject = Instantiate(personPrefab, spawnLocation.position, Quaternion.identity);

        Person personScript = inmateObject.GetComponent<Person>();
        personScript.personType = PersonType.Inmate;

        personScript.personName = "Inamte " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;
        if (personScript.decoratedPerson is Inmate inmate)
        {
            inmate.SetPrisonSentence(Random.Range(1, 100));
        }

    }
    public void SpawnGuard()
    {
        GameObject guardObject = Instantiate(personPrefab, spawnLocation.position, Quaternion.identity);

        Person personScript = guardObject.GetComponent<Person>();
        personScript.personType = PersonType.Guard;

        personScript.personName = "Guard " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;

    }

    public void SpawnDoctor()
    {
        GameObject doctorObject = Instantiate(personPrefab, spawnLocation.position, Quaternion.identity);

        Person personScript = doctorObject.GetComponent<Person>();
        personScript.personType = PersonType.Medical;

        personScript.personName = "Doctor " + Random.Range(1000, 9999);
        personScript.age = Random.Range(18, 80);
        personScript.health = 100;
        personScript.hunger = 0;

    }

    private void StartInmateFight()
    {
        if (inmates.Count < 2)
        {
            Debug.Log("Not enough inmates to start fight.");
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
        Person patient = injuredPeople[Random.Range(0, injuredPeople.Count)];
        Person doctor = doctors[Random.Range(0, doctors.Count)];
        if (doctor == null)
        {
            Debug.Log("No doctors to heal patiens.");
        }
        if (patient == null)
        {
            Debug.Log("No patients to be healed.");
        }
        else
        {
            if (doctor.decoratedPerson is Doctor doc)
            {
                doc.HealPatient(patient);
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

}
