using System.Collections;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public static ResourceHandler Instance;
    public int FoodWorkerCount = 0;
    public int GoldWorkerCount = 0;
    public int WoodWorkerCount = 0;
    public int SwordWorkerCount = 0;

    public float FoodproductionRate = 2f;
    public float GoldproductionRate = 2f;
    public float WoodproductionRate = 2f;
    public float SwordproductionRate = 2f;
    public AudioSource MinerSource;
    public AudioSource BusherSource;
    public AudioSource LumberSource;
    public AudioSource blackSmithSource;
    bool isPlayGold=false;
    bool isPlayFood=false;
    bool isPlayWood=false;
    bool isPlaySword=false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        float Goldamount = GoldWorkerCount * GoldproductionRate * Time.deltaTime;
        float Foodamount = FoodWorkerCount * FoodproductionRate * Time.deltaTime;
        float Woodamount = WoodWorkerCount * WoodproductionRate * Time.deltaTime;
        float Swordamount = SwordWorkerCount * SwordproductionRate * Time.deltaTime;
        ResourceManager.Instance.AddFood(Foodamount);
        ResourceManager.Instance.AddGold(Goldamount);
        ResourceManager.Instance.AddWood(Woodamount);
        ResourceManager.Instance.AddSword(Swordamount);
        if (GoldWorkerCount > 0&& !isPlayGold)
        {
            PlayMinerSFX();
            isPlayGold = true;
        }
        if (FoodWorkerCount > 0&& !isPlayFood)
        {
            PlayBusherSFX();
            isPlayFood = true;
        }
        if (WoodWorkerCount > 0&& !isPlayWood)
        {
            PlayLumberSFX();
            isPlayWood = true;
        }
        if (SwordWorkerCount > 0&& !isPlaySword)
        {
            PlayBlackSmithSFX();
            isPlaySword = true;
        }
    }
    public void AddFoodWorker()
    {
        FoodWorkerCount++;
    }
    public void AddGoldWorker()
    {
        GoldWorkerCount++;
    }
    public void AddWoodWorker()
    {
        WoodWorkerCount++;
    }
    public void AddSwordWorker()
    {
        SwordWorkerCount++;
    }
    public void PlayMinerSFX()
    {
        MinerSource.Play();
    }
    public void PlayBusherSFX()
    {
        BusherSource.Play();
    }
    public void PlayLumberSFX()
    {
        LumberSource.Play();
    }
    public void PlayBlackSmithSFX()
    {
        blackSmithSource.Play();
    }
}
