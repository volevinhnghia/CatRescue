using UnityEngine;

[CreateAssetMenu(fileName = "UpgradePrice", menuName = "Scriptable Objects/UpgradePrice")]
public class UpgradePrice : ScriptableObject
{
    [SerializeField] private float _speedUpgrade;
}
