using UnityEngine;
using Watermelon;

public class CharacterCatCollect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Inventory _inventory;
    [SerializeField] private CharacterStats _stats;
    private void Awake()
    {
        _inventory = new Inventory();
        _inventory.resetInventory();
    }
    void Start()
    {
        CatBehaviour.OnPickUpCat += CatBehaviour_OnPickUpCat;
        StageManager.OnStageChange += OnGameOver;
    }
    private void OnDestroy()
    {
        CatBehaviour.OnPickUpCat -= CatBehaviour_OnPickUpCat;
        StageManager.OnStageChange -= OnGameOver;
    }

    //Counting how many cat player rescue
    private void CatBehaviour_OnPickUpCat(Transform obj)
    {
        _inventory.addCat(obj.gameObject);
        
    }

    //Calculate Money when GameOver
    private void OnGameOver(GameStage stage) 
    {
        if ((int)stage != (int)GameStage.GameOver)
        {
            return;
        }
        _stats.SetMoney(_inventory.getCat() * 10);
    }
}
