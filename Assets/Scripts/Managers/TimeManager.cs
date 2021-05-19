using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float TimeElapsed { get; private set; }

    private void Update()
    {
        TimeElapsed += Time.deltaTime;
    }
}