using UnityEngine;
using UnityEngine.UI;

public class OnScreenDebugger : MonoBehaviour
{
    public static OnScreenDebugger Instance;
    public VerticalLayoutGroup verticalLayoutGroup;
    public int maxMessages;
    public GameObject debugMessagePrefab;

    int messageCount;

    private void Awake()
    {
        Instance = this;
    }

    public static void DebugMessage(string _message)
    {
        GameObject newMessage = Instantiate(Instance.debugMessagePrefab, Instance.verticalLayoutGroup.transform);
        newMessage.GetComponent<Text>().text = $"[{Instance.messageCount}] {_message}";
        Instance.messageCount++;

        if(Instance.verticalLayoutGroup.transform.childCount >= Instance.maxMessages)
        {
            Destroy(Instance.verticalLayoutGroup.transform.GetChild(0).gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DebugMessage($"Testing Testing!");
        }
    }
}
