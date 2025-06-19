/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : Person.cs
Description : Person class using the decorator pattern
Author : Reece Smith
Mail : reece.smtih@mds.ac.nz
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonType // enums for what type of person it is
{
    Guard,
    Medical,
    Inmate,
}
public class Person : MonoBehaviour
{

    public PersonType personType;

    public IPerson decoratedPerson;

    public string personName = "Alex";
    public int age = 30;
    public int health = 100;
    public int hunger = 0;
    // Start is called before the first frame update
    void Start() // sets up the base person and then sets the decorator depending on the enum
    {
        PersonBase basePerson = new PersonBase()
        {
            name = personName,
            age = age,
            health = health,
            hunger = hunger
        };

        switch (personType)
        {
            case PersonType.Guard:
                decoratedPerson = new Guard(basePerson);
                break;
            case PersonType.Medical:
                decoratedPerson = new Doctor(basePerson);
                break;
            case PersonType.Inmate:
                decoratedPerson = new Inmate(basePerson);
                break;
            default:
                decoratedPerson = basePerson;
                break;
        }

        decoratedPerson.Describe();
    }


    void Update()
    {
        
    }
}

public interface IPerson // decorator with a describe function
{
    void Describe();
}

public class PersonBase : IPerson // base class with the defined describe function and variable check functions
{
    public int health { get; set; }
    public string name { get; set; }

    public int hunger { get; set; }

    public int age { get; set; }


    public virtual void Describe()
    {
        OnScreenDebugger.DebugMessage("I am a person");
    }
    public void HealthCheck()
    {
        OnScreenDebugger.DebugMessage(health.ToString());
    }
    public void NameCheck()
    {
        OnScreenDebugger.DebugMessage(name);
    }
}

public abstract class PersonDecorator : IPerson // person decorator based on interface
{
    protected IPerson person;
    public PersonDecorator(IPerson _person)
    {
        person = _person;
    }

    public virtual void Describe()
    {
        person.Describe();
    }
}

public class Doctor : PersonDecorator // doctor decorator with heal patient function
{
    public Doctor(IPerson person) : base(person) { }
    public override void Describe()
    {
        base.Describe();
        OnScreenDebugger.DebugMessage("I am also a doctor.");
    }
    public void HealPatient(Person _person)
    {
        _person.health = 100;
        OnScreenDebugger.DebugMessage("Healed a patient and sent back.");
    }
}

public class Guard : PersonDecorator // guard decorator with check contraband function
{
    public Guard(IPerson person) : base(person) { }
    public override void Describe()
    {
        base.Describe();
        OnScreenDebugger.DebugMessage("I am also a guard.");
    }
    public void CheckContraband(Person _person)
    {
        if (_person.decoratedPerson is Inmate inmate)
        {
            if (inmate.GetHasContraband())
            {

                OnScreenDebugger.DebugMessage("Inmate has contraband");
                if (inmate.GetContrabandSeverity() < 33)
                {
                    inmate.IncreaceSentence(5);
                    OnScreenDebugger.DebugMessage("Inmate increased sentence by 5 years.");
                }
                if (inmate.GetContrabandSeverity() >= 33 &&  inmate.GetContrabandSeverity() <= 66)
                {
                    inmate.IncreaceSentence(20);
                    OnScreenDebugger.DebugMessage("Inmate increased sentence by 20 years.");
                }
                if (inmate.GetContrabandSeverity() > 66)
                {
                    inmate.IncreaceSentence(80);
                    OnScreenDebugger.DebugMessage("Inmate increased sentence by 80 years.");
                }
                inmate.contribandSeverity = 0;
                inmate.hasContraband = false;
            }
            else
            {
                return;
            }
        }
        
    }

}

public class Inmate : PersonDecorator // inmate decorator with inmate specific variable and functions for setting, getting, increaseing and decreasing its variables around sentence and contraband
{
    public int prisonSentece;

    public bool hasContraband;
    public int contribandSeverity;
    public Inmate(IPerson person) : base(person) { }

    public override void Describe()
    {
        base.Describe();
        OnScreenDebugger.DebugMessage("I am also an inmate");
    }
    public void ReduceSentence(int _sentenceAmount)
    {
        prisonSentece -= _sentenceAmount;
        OnScreenDebugger.DebugMessage("Inmate sentence reduced.");
    }
    public void IncreaceSentence(int _sentenceAmount)
    {
        prisonSentece += _sentenceAmount;
        OnScreenDebugger.DebugMessage("Inmate sentence increased.");
    }
    public void FightInmate(Person _person)
    {
        if (_person.decoratedPerson is Inmate inmate)
        {
            _person.health -= 20;
        }
    }
    public int GetPrisonSentece()
    {
        return prisonSentece;
    }
    public void SetPrisonSentence(int _sentenceAmount)
    {
        prisonSentece = _sentenceAmount;
    }

    public bool GetHasContraband()
    {
        return hasContraband;
    }
    public void SetHasContraband()
    {
        hasContraband = Random.Range(0f, 100f) < 10f;
    }

    public int GetContrabandSeverity()
    {
        return contribandSeverity;
    }

    public void SetContrabandSeverity()
    {
        if (hasContraband) contribandSeverity = Random.Range(1, 100);
        else contribandSeverity = 0;
    }
}
