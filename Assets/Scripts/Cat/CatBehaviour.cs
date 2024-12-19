using System;
using System.Collections;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float _destroyDelay = 1.5f;

    private bool _isCoroutineRun = false;
    private Coroutine _countdownCoroutine;

    public static event Action<Transform> OnPickUpCat;

    //Start coroutine and pick up after delay time
    public void FieldOfView_OnEnterFOV(Transform obj)
    {
        if (!_isCoroutineRun)
        {
            _countdownCoroutine = StartCoroutine(DestroyAfterSeconds(obj));
            _isCoroutineRun = true;
        }
    }
    //when object get out Player's FOV -> stop coroutine
    public void FieldOfView_OnExitFOV(Transform obj)
    {
        if (_isCoroutineRun)
        {
            StopCoroutine(_countdownCoroutine);
            _isCoroutineRun = false;
        }
    }
    private IEnumerator DestroyAfterSeconds(Transform obj)
    {
        yield return new WaitForSeconds(_destroyDelay);
        OnPickUpCat?.Invoke(obj);
        Destroy(obj.gameObject);
    }
}
