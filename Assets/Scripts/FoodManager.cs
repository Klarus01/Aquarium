using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food foodPrefab;

    public List<Food> GenerateFood(int amount)
    {
        List<Food> foodList = new List<Food>();

        for (int i = 0; i < amount; i++)
        {
            Food food = Instantiate(foodPrefab, GameManager.Instance.mainAquarium.RandomPointInBounds(), Quaternion.identity);
            foodList.Add(food);
        }

        return foodList;
    }
}