using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text sizeText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button breedButton;
    [SerializeField] private LayerMask fishLayer;
    [SerializeField] BreedingManager breedingManager;

    public FishStats fish;

    private void Start()
    {
        sellButton.onClick.AddListener(SellButton);
        breedButton.onClick.AddListener(BreedButton);
    }

    private void Update()
    {
        if (fish != null)
        {
            OnUiChange();
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryHitWithRaycast();
        }
    }

    private void TryHitWithRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, fishLayer))
        {
            fish = hit.transform.GetComponent<FishStats>();
            if (fish.Scale >= .1f)
            {
                breedButton.gameObject.SetActive(true);
            }
            else
            {
                breedButton.gameObject.SetActive(false);
            }
            panel.SetActive(true);
        }
        else
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    private void SellButton()
    {
        GameManager.Instance.Money += fish.Price;
        panel.SetActive(false);
        Destroy(fish.gameObject);
    }

    private void BreedButton()
    {
        breedingManager.AddToBreeding(fish);
        panel.SetActive(false);
    }

    public void OnUiChange()
    {
        foodText.SetText(fish.hunger + " / " + fish.maxHunger);
        sizeText.SetText(fish.size + " / " + fish.maxSize);
        priceText.SetText(fish.Price + "$");
    }
}
