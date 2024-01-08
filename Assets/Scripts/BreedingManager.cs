using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingManager : MonoBehaviour
{
    [SerializeField] private List<Breeding> breedings = new();
    [SerializeField] private FishStats fishPrefab;
    [SerializeField] private Aquarium mainAquarium;
    [SerializeField] private Aquarium[] aquariumsTransform;

    [System.Serializable]
    public class Breeding
    {
        public List<FishStats> fishList = new();
        public float breedingPercent = 0f;
        public bool IsBreedingComplete => breedingPercent >= 100f;
    }

    public void AddToBreeding(FishStats fishToBreed)
    {
        if (breedings.Count == 0 || breedings[^1].fishList.Count == 2)
        {
            breedings.Add(new Breeding());
        }

        var currentBreeding = breedings[^1];
        currentBreeding.fishList.Add(fishToBreed);
        fishToBreed.transform.position = aquariumsTransform[breedings.Count - 1].transform.position;
        fishToBreed.GetComponent<FishBehaviour>().AquariumChanged(aquariumsTransform[breedings.Count - 1]);
        fishToBreed.isBreeding = true;

        if (currentBreeding.fishList.Count == 2)
        {
            StartCoroutine(IncreaseBreedingPercentOverTime(currentBreeding));
        }
    }

    private void CreateNewBornFish(FishStats momFish, FishStats dadFish)
    {
        FishStats fish = Instantiate(fishPrefab);
        fish.isFishBorn = true;
        fish.momFish = momFish;
        fish.dadFish = dadFish;

        momFish.GetComponent<FishBehaviour>().aquarium = mainAquarium;
        momFish.isBreeding = false;
        dadFish.GetComponent<FishBehaviour>().aquarium = mainAquarium;
        dadFish.isBreeding = false;
    }

    private IEnumerator IncreaseBreedingPercentOverTime(Breeding breeding)
    {
        const float duration = 5f;
        float startTime = Time.time;
        float startPercent = breeding.breedingPercent;

        while (breeding.breedingPercent < 100f)
        {
            float time = (Time.time - startTime) / duration;
            breeding.breedingPercent = Mathf.Lerp(startPercent, 100f, time);
            yield return null;
        }

        breeding.breedingPercent = 100f;
        CreateNewBornFish(breeding.fishList[0], breeding.fishList[1]);
        breedings.Remove(breeding);
    }
}