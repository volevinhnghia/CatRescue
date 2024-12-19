using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _money;

    public float GetSpeed() { return _speed; }
    public float GetMoney() { return _money; }
    public void SetSpeed(float speed) { this._speed += speed; }
    public void SetMoney(float money) {  this._money += money; }

}
