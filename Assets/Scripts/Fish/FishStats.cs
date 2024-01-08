using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishStats : MonoBehaviour
{
    [SerializeField] private FishData fishData;
    [SerializeField] private FishBehaviour fishBehaviour;
    [SerializeField] private RawImage hungerIcon;
    [SerializeField] private GameObject mutationObject;
    private Renderer renderer;

    [Header("---Parents---")]
    public bool isFishBorn = false;
    public FishStats momFish;
    public FishStats dadFish;

    [Header("---Hunger Stats---")]
    public float maxHunger;
    public float hunger;
    private float Hunger => Mathf.Round(maxHunger * Random.Range(0.6f, 0.8f));
    private float hungerRate;

    [Header("---Size Stats---")]
    public float maxSize;
    public float size;
    private float Size => Random.Range(5, 20);
    private float sizeRate;
    public float Scale => size / maxSize;

    [Header("---Price Stats---")]
    public float maxPrice;
    public float Price => Mathf.Round(maxPrice * Scale);

    [Header("---Rest---")]
    public Material material;
    public float movementSpeed;
    public bool isBreeding = false;

    private void FishScale() => transform.localScale = new Vector3(Scale, Scale, Scale);

    public bool IsHungerMoreThanHalf() => hunger > maxHunger / 2f;

    public bool IsHungerLessThanHalf() => hunger < maxHunger / 2f;

    public bool IsHungerMax() => hunger >= maxHunger;

    private void ToggleHungerIcon(bool state) => hungerIcon.gameObject.SetActive(state);

    private void Start()
    {
        Initialization();
        FishScale();
        StartCoroutine(IncreaseHungerOverTime());
        StartCoroutine(GrowthOverTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        TryEatFood(other.GetComponent<Food>());
    }

    private void Initialization()
    {
        renderer = GetComponent<Renderer>();

        maxHunger = fishData.maxHunger;
        hungerRate = fishData.hungerRate;

        maxSize = fishData.maxSize;
        sizeRate = fishData.sizeRate;

        maxPrice = fishData.maxPrice;

        movementSpeed = fishData.movementSpeed;

        material = fishData.material;

        if (isFishBorn)
        {
            GameManager.Instance.mutationChance.CheckMutationOnBreed(ref maxHunger, ref maxSize, ref maxPrice, ref movementSpeed, ref material, momFish, dadFish);
        }
        else GameManager.Instance.mutationChance.CheckMutationOnBought(ref maxHunger, ref maxSize, ref maxPrice, ref movementSpeed, ref material);

        hunger = Hunger;
        size = Size;

        renderer.material = material;
    }

    private void TryEatFood(Food food)
    {
        if (food != null && !IsHungerMax())
        {
            Eat(food.value);
            GameManager.Instance.foodList.Remove(food);
            Destroy(food.gameObject);
        }
    }

    private void Eat(float foodAmount)
    {
        hunger += foodAmount;
        if (IsHungerMax())
        {
            hunger = maxHunger;
        }
    }

    private IEnumerator IncreaseHungerOverTime()
    {
        while (hunger > 0)
        {
            if (!isBreeding)
            {
                hunger -= hungerRate;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator GrowthOverTime()
    {
        while (true)
        {
            ToggleHungerIcon(IsHungerLessThanHalf());
            while (IsHungerMoreThanHalf() && !isBreeding)
            {
                size += sizeRate;
                FishScale();
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}