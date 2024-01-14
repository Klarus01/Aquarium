using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float money = 1000f;
    public float Money { get { return money; } set { UpdateMoneyText(); } }

    public float FoodCost { get; private set; } = 1f;
    public float FishCost { get; private set; } = 50f;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Button buyFoodButton;
    [SerializeField] private Button buyFishButton;
    public Aquarium mainAquarium;

    public FishMutationChance mutationChance;
    private FoodManager foodManager;
    private FishManager fishManager;

    public List<Food> foodList = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        foodManager = GetComponent<FoodManager>();
        fishManager = GetComponent<FishManager>();
    }

    private void Start()
    {
        buyFoodButton.onClick.AddListener(BuyFood);
        buyFishButton.onClick.AddListener(BuyFish);
        UpdateMoneyText();
    }

    public bool DecreaseMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
            return true;
        }
        return false;
    }

    private void UpdateMoneyText()
    {
        moneyText.SetText(Money + "$");
    }

    private void BuyFood()
    {
        if (DecreaseMoney(FoodCost))
        {
            int amount = Random.Range(1, 5);
            List<Food> food = foodManager.GenerateFood(amount);

            foodList.AddRange(food);
        }
    }

    private void BuyFish()
    {
        if (DecreaseMoney(FishCost))
        {
            FishStats fish = fishManager.GenerateFish();
        }
    }
}