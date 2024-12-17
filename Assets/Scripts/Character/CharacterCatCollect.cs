using UnityEngine;
using Watermelon;

public class CharacterCatCollect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Inventory inventory;
    [SerializeField] private CharacterStats _stats;
    private void Awake()
    {
        inventory = new Inventory();
        inventory.resetInventory();
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
        inventory.addCat(obj.gameObject);
        
    }

    //Calculate Money when GameOver
    private void OnGameOver(GameStage stage) 
    {
        if ((int)stage != (int)GameStage.GameOver)
        {
            return;
        }
        _stats.setMoney(inventory.getCat() * 10);
    }
}
