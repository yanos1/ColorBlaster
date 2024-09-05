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
    private List<Coroutine> _runningCoroutines;
    private MonoBehaviour _coroutineRunner;

    public TimeManager()
    {
        _coroutineRunner = CoreManager.instance.MonoRunner;
        _runningCoroutines = new List<Coroutine>();
        _isPaused = false;
        _elapsedTime = 0f;
        _coroutineRunner.StopCoroutine(UpdateTime());
    }



    // Run a function X times every Y seconds

    public IEnumerator RunFunctionXTImes(EventNames eventName, object parameters, int x, float interval)
    {
        int count = 0;
        while (count < x)
        {
            while (_isPaused)
            {
                yield return null; // Wait until unpaused
            }

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
            while (_isPaused)
            {
                yield return null; // Wait until unpaused
            }
            yield return new WaitForSeconds(interval);

            CoreManager.instance.EventManager.InvokeEvent(eventName, parameters);
        }
    }

    // Track the elapsed time
    public float GetElapsedTime()
    {
        return _elapsedTime;
    }

    // Update the elapsed time (to be called every frame)
    public IEnumerator UpdateTime()
    {
        while (true)
        {
            _elapsedTime += Time.deltaTime;
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

// Reset time
    public void ResetTime()
    {
        _elapsedTime = 0;
    }

 
}