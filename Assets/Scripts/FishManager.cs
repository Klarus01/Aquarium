using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private FishStats fishPrefab;

    public FishStats GenerateFish()
    {
        return Instantiate(fishPrefab);
    }
}