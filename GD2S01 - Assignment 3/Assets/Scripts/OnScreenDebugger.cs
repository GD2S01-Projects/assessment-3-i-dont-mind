/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : OnScreenDebugger.cs
Description : Contains the implementation to be able to debug onscreen messages onto the unity screen environment
Author : Joe Rickwood
Mail : joe.rickwood@mds.ac.nz
*/


using UnityEngine;
using UnityEngine.UI;

public class OnScreenDebugger : MonoBehaviour
{
    public static OnScreenDebugger Instance;
    public VerticalLayoutGroup verticalLayoutGroup;
    public int maxMessages;
    public GameObject debugMessagePrefab;

    static int messageCount;

    private void Awake()
    {
        Instance = this;
    }

    //Debugs A Logged Message To The On-Screen Debugger
    public static void DebugMessage(string _message)
    {
        GameObject newMessage = Instantiate(Instance.debugMessagePrefab, Instance.verticalLayoutGroup.transform);
        newMessage.GetComponent<Text>().text = $"[{messageCount}] {_message}";
        messageCount++;

        if(Instance.verticalLayoutGroup.transform.childCount >= Instance.maxMessages)
        {
            Destroy(Instance.verticalLayoutGroup.transform.GetChild(0).gameObject);
        }
    }

    //Debugs A Logged Message To The On-Screen Debugger With A Custom Color 
    public static void DebugMessage(string _message, Color _color)
    {
        GameObject newMessage = Instantiate(Instance.debugMessagePrefab, Instance.verticalLayoutGroup.transform);
        newMessage.GetComponent<Text>().text = $"[{messageCount}] {_message}";
        newMessage.GetComponent<Text>().color = _color;

        messageCount++;

        if (Instance.verticalLayoutGroup.transform.childCount >= Instance.maxMessages)
        {
            Destroy(Instance.verticalLayoutGroup.transform.GetChild(0).gameObject);
        }
    }
}
