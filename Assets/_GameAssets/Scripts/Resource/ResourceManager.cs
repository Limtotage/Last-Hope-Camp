using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public float GoldAmount=0f;
    public float WoodAmount=0f;
    public float FoodAmount=0f;
    public float SwordAmount=0f;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddGold(float amount)
    {
        GoldAmount += amount;
    }
    public void AddFood(float amount)
    {
        FoodAmount += amount;
    }
    public void AddWood(float amount)
    {
        WoodAmount += amount;
    }
    public void AddSword(float amount)
    {
        SwordAmount += amount;
    }
    public bool SpendGold(float amount)
    {
        if(GoldAmount >= amount)
        {
            GoldAmount -= amount;
            return true;
        }
        return false;
    }    
    public bool SpendFood(float amount)
    {
        if(FoodAmount >= amount)
        {
            FoodAmount -= amount;
            return true;
        }
        return false;
    }
    public bool SpendWood(float amount)
    {
        if(WoodAmount >= amount)
        {
            WoodAmount -= amount;
            return true;
        }
        return false;
    }
    public bool SpendSword(float amount)
    {
        if(SwordAmount >= amount)
        {
            SwordAmount -= amount;
            return true;
        }
        return false;
    }
}
