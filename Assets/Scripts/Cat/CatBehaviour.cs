using System;
using System.Collections;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float destroyDelay = 1.5f;

    private bool isCoroutineRun = false;
    private Coroutine countdownCoroutine;

    public static event Action<Transform> OnPickUpCat;
    void Start()
    {
    }
    private void OnDestroy()
    {
    }

    //Start coroutine and pick up after delay time
    public void FieldOfView_OnEnterFOV(Transform obj)
    {
        if (!isCoroutineRun)
        {
            countdownCoroutine = StartCoroutine(DestroyAfterSeconds(obj));
            isCoroutineRun = true;
        }
    }
    //when object get out Player's FOV -> stop coroutine
    public void FieldOfView_OnExitFOV(Transform obj)
    {
        if (isCoroutineRun)
        {
            StopCoroutine(countdownCoroutine);
            isCoroutineRun = false;
        }
    }
    private IEnumerator DestroyAfterSeconds(Transform obj)
    {
        yield return new WaitForSeconds(destroyDelay);
        OnPickUpCat?.Invoke(obj);
        Destroy(obj.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
