using System.Collections;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public int currentTaskIndex;
    public float taskDelay;
    private string[] tasks =
    {
        "Find the key to the locked room.",
        "the whispers won’t stop… turn off the radio.",
        "Where’s your phone? Find it and call for help.",
        "Answer the phone… someone is calling.",
        "the ritual is incomplete… Place the cursed objects.",
        "Find the key to the main door to escape.",
        "Get out… before it’s too late.",
    };

    public void CompleteTask()
    {
        if (currentTaskIndex < tasks.Length)
        {
            StartCoroutine(TaskDelay());
        }
    }

    public IEnumerator TaskDelay()
    {
        yield return new WaitForSeconds(taskDelay);
        currentTaskIndex++;
        Debug.Log($"Current task is {tasks[currentTaskIndex]} with index {currentTaskIndex}");
    }
}
