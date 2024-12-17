using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float money;
    [SerializeField]
    private int level;

    public float getSpeed() { return speed; }
    public float getMoney() { return money; }
    public int getLevel() { return level; }
    public void setSpeed(float speed) { this.speed += speed; }
    public void setMoney(float money) {  this.money += money; }

    public void setLevel() { this.level++; }
}
