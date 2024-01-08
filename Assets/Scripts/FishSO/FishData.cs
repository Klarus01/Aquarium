using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Custom/Fish Data")]
public class FishData : ScriptableObject
{
    public string id = "ID000";
    public float maxHunger = 100f;
    public float maxSize = 100f;
    public float maxPrice = 100f;
    public float movementSpeed = 2f;
    public Color color = Color.white;

    public float hungerRate = 1f;
    public float sizeRate = 1f;
}
