using UnityEngine;

public class Food : MonoBehaviour
{
    public int value = 5;
    private int maxValue = 15;

    void Start()
    {
        value = Random.Range(value, maxValue);
    }
}
