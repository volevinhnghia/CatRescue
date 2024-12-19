using System;
using UnityEngine;

public class TsunamiBehaviour : MonoBehaviour
{
    [SerializeField] private int _tsunamiSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CharacterMovement.OnScaleTsunami += ScaleTsunami;
    }
    void OnDestroy()
    {
        CharacterMovement.OnScaleTsunami -= ScaleTsunami;
    }
    // Update is called once per frame
    void Update()
    {
        TsunamiMove(_tsunamiSpeed);
    }
    private void TsunamiMove(int speed)
    {
        Vector3 dir = new Vector3(1, 0, 0);
        this.transform.Translate(dir * speed * Time.deltaTime);
    }
    private void ScaleTsunami()
    {
        _tsunamiSpeed += 30;
    }

    //Change stage to GameOver when Tsunami hit player
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            StageManager.Instance.UpdateGameStage(GameStage.GameOver);
        }
        else
        {
            return;
        }
    }
    
}
