using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private GameObject _speedText;
    [SerializeField] private GameObject _moneyText;
    [SerializeField] private GameObject _upgradeButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValue();
    }
    private void UpdateValue() 
    {
        _speedText.GetComponent<TextMeshProUGUI>().SetText($"Speed: {_stats.GetSpeed()}");
        _moneyText.GetComponent<TextMeshProUGUI>().SetText($"Money: {_stats.GetMoney()}");
        if (_stats.GetMoney() < 30f)
        {
            _upgradeButton.GetComponent<Button>().enabled = false;
        }
        else
        {
            _upgradeButton.GetComponent<Button>().enabled = true;
        }
        
    }
    public void speedUpgrade() 
    {
        _stats.SetSpeed(3);
        _stats.SetMoney(-30);
    }
    public void gameStart()
    {
        SceneManager.LoadScene("Map1");
        Time.timeScale = 1;
    }
}
