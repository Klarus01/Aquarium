using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    private FishStats fishStats;
    public Aquarium aquarium;
    private Vector3 targetPosition;
    private float stoppingDistance = .1f;
    private float waitForNextPlaceToSwim = 0;
    private float minWaitForNextSwim = 1f;
    private float maxWaitForNextSwim = 5f;

    public enum State
    {
        Idle,
        Swim,
        SwimToFood,
        LookingForTarget,
        LookingForFood
    }

    public State currentState;

    private float RandPos(float min, float max) => Random.Range(min, max);

    private Vector3 GetPlaceToSwim() => new(RandPos(aquarium.xMin, aquarium.xMax), RandPos(aquarium.yMin, aquarium.yMax), RandPos(aquarium.zMin, aquarium.zMax));

    private void Start()
    {
        fishStats = GetComponent<FishStats>();
        aquarium = GameManager.Instance.mainAquarium;
        SetState(State.Idle);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Swim:
                SwimUpdate();
                break;
            case State.SwimToFood:
                SwimToFoodUpdate();
                break;
            default:
                break;
        }
    }

    private void IdleUpdate()
    {
        if (fishStats.IsHungerLessThanHalf())
        {
            SetState(State.LookingForFood);
        }
        else if (waitForNextPlaceToSwim <= 0)
        {
            SetState(State.LookingForTarget);
        }
        else
        {
            waitForNextPlaceToSwim -= Time.deltaTime;
        }
    }

    private void SwimUpdate()
    {
        MoveTowardsTarget();
    }

    private void SwimToFoodUpdate()
    {
        targetPosition = GetClosestFood().position;
        SetState(State.Swim);
    }

    private void SetState(State newState)
    {
        currentState = newState;
        EnterState(newState);
    }

    private void EnterState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                Debug.Log("idle");
                break;
            case State.Swim:
                Debug.Log("swim");
                SwimToPlace();
                break;
            case State.LookingForFood:
                Debug.Log("food");
                if (GetClosestFood())
                {
                    targetPosition = GetClosestFood().position;
                    SetState(State.Swim);
                    break;
                }
                SetState(State.Idle);
                break;
            case State.LookingForTarget:
                Debug.Log("target");
                targetPosition = GetPlaceToSwim();
                SetState(State.Swim);
                break;
        }
    }

    private void SwimToPlace()
    {
        AdjustRotation();

        MoveTowardsTarget();
    }

    private void AdjustRotation()
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90f, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        if (distance > stoppingDistance)
        {
            direction.Normalize();
            transform.Translate(fishStats.movementSpeed * Time.deltaTime * direction, Space.World);
        }
        else
        {
            HandleStateAfterArrival();
        }
    }

    private void HandleStateAfterArrival()
    {
        if (fishStats.IsHungerLessThanHalf())
        {
            SetState(State.LookingForFood);
        }
        else
        {
            waitForNextPlaceToSwim = RandPos(minWaitForNextSwim, maxWaitForNextSwim);
            SetState(State.Idle);
        }
    }

    private Transform GetClosestFood()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        List<Food> foodListCopy = new(GameManager.Instance.foodList);

        foreach (Food food in foodListCopy)
        {
            if (food != null)
            {
                float dist = Vector3.Distance(food.transform.position, currentPos);
                if (dist < minDist)
                {
                    closest = food.transform;
                    minDist = dist;
                }
            }
        }
        return closest;
    }

    public void AquariumChanged(Aquarium aquarium)
    {
        this.aquarium = aquarium;
        SetState(State.LookingForTarget);
    }
}