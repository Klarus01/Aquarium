using UnityEngine;

public class FishMutationChance : MonoBehaviour
{
    private float chanceOnBought = 20f;

    private bool GetParentToMutate() => Random.Range(0, 2) == 0;

    private bool IsStatMutate(float chanceToMutate) => Random.Range(0, 100) <= chanceToMutate;

    private float MutateStat(float baseValue, float minValue, float maxValue) => Mathf.Round(Random.Range(baseValue + minValue, baseValue + maxValue));

    public void CheckMutationOnBought(ref float maxHunger, ref float maxSize, ref float maxPrice, ref float movementSpeed, ref Color color)
    {
        if (IsStatMutate(chanceOnBought))
        {
            maxHunger = MutateStat(maxHunger, -10, 50);
            maxSize = MutateStat(maxSize, -10, 50);
            maxPrice = MutateStat(maxPrice, -50, 500);
            movementSpeed = MutateStat(movementSpeed, -0.1f, 0.3f);
        }
    }

    public void CheckMutationOnBreed(ref float maxHunger, ref float maxSize, ref float maxPrice, ref float movementSpeed, ref Color color, FishStats momFish, FishStats dadFish)
    {
        maxHunger = GetParentToMutate() ? MutateStat(momFish.maxHunger, -10, 50) : MutateStat(dadFish.maxHunger, -10, 50);
        maxSize = GetParentToMutate() ? MutateStat(momFish.maxSize, -10, 50) : MutateStat(dadFish.maxSize, -10, 50);
        maxPrice = GetParentToMutate() ? MutateStat(momFish.maxPrice, -50, 500) : MutateStat(dadFish.maxPrice, -50, 500);
        movementSpeed = GetParentToMutate() ? MutateStat(momFish.movementSpeed, -0.1f, 0.3f) : MutateStat(dadFish.movementSpeed, -0.1f, 0.3f);
    }
}
