using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonType
{
    Guard,
    Medical,
    Inmate,
}
public class Person : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IPerson
{
    void Describe();
}

public class PersonBase : IPerson
{
    public virtual void Describe()
    {
        Debug.Log("I am a person");
    }
}

public abstract class PersonDecorator : IPerson
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

public class Doctor : PersonDecorator
{
    public Doctor(IPerson person) : base(person) { }
    public override void Describe()
    {
        base.Describe();
        Debug.Log("I am also a doctor.");
    }
    public void HealPatient()
    {
        Debug.Log("Healed a patient and sent back.");
    }
}

public class Guard : PersonDecorator
{
    public Guard(IPerson person) : base(person) { }
    public override void Describe()
    {
        base.Describe();
        Debug.Log("I am also a guard.");
    }
    public void EscortInmate()
    {
        Debug.Log("Inmate escorted to needed location.");
    }
}
