using UnityEngine;

public class WorkerTyper : MonoBehaviour
{
    public Enums.WorkerType resourceType;
    void Start()
    {
        if (resourceType == Enums.WorkerType.Food)
        {
            ResourceHandler.Instance.AddFoodWorker();
        }
        else if (resourceType == Enums.WorkerType.Gold)
        {
            ResourceHandler.Instance.AddGoldWorker();
        }
        else if (resourceType == Enums.WorkerType.Wood)
        {
            ResourceHandler.Instance.AddWoodWorker();
        }
        else if (resourceType == Enums.WorkerType.Sword)
        {
            ResourceHandler.Instance.AddSwordWorker();
        }
    }
}
