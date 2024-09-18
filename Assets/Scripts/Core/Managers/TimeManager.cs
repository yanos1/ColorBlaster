using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager
{
    private bool _isPaused;
    private float _elapsedTime;
    private float _currentRunElapsedTime;
    private List<Coroutine> _runningCoroutines;
    private MonoBehaviour _coroutineRunner;

    public TimeManager()
    {
        _coroutineRunner = CoreManager.instance.MonoRunner;
        _runningCoroutines = new List<Coroutine>();
        _isPaused = false;
        _elapsedTime = 0f;
        _currentRunElapsedTime = 0f;

        _coroutineRunner.StartCoroutine(UpdateTime());
        CoreManager.instance.EventManager.AddListener(EventNames.GameOver, ResetRunTime);
   
    }

    public void OnDestroy()
    {
        CoreManager.instance.EventManager.RemoveListener(EventNames.GameOver, ResetRunTime);

    }

    private void ResetRunTime(object obj)
    {
        _currentRunElapsedTime = 0;
    }


    // Run a function X times every Y seconds

    private void ResumeRunTimer(object obj)
    {
        _isPaused = false;
    }

    private void PauseRunTimer(object obj)
    {
        _isPaused = true;
    }

    public IEnumerator RunFunctionXTImes(EventNames eventName, object parameters, int x, float interval)
    {
        int count = 0;
        while (count < x)
        {
        

            CoreManager.instance.EventManager.InvokeEvent(eventName, parameters);
            count++;
            yield return new WaitForSeconds(interval);
        }
    }

    // Run a function infinitely every Y seconds


    public IEnumerator RunFunctionInfinitely(EventNames eventName, object parameters, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            CoreManager.instance.EventManager.InvokeEvent(eventName, parameters);
        }
    }

    // Track the elapsed time
    public float GetGameElapsedTime()
    {
        return _elapsedTime;
    }

    public float GetRunElapsedTime()
    {
        return _currentRunElapsedTime;
    }

    // Update the elapsed time (to be called every frame)
    public IEnumerator UpdateTime()
    {
        while (true)
        {
            _elapsedTime += Time.deltaTime;
            if (CoreManager.instance.GameManager.IsRunActive)
            {
                _currentRunElapsedTime += Time.deltaTime;
            }

            yield return null;
        }
    }


// Pause time
    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
    }

// Resume time
    public void ResumeTime()
    {
        _isPaused = false;
        Time.timeScale = 1;
    }
    
}