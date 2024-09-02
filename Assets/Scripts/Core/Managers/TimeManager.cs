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
        CoreManager.instance.EventManager.AddListener(EventNames.RestartGame, CancelAllCoroutines);
    }

    public void OnDestroy()
    {
        CoreManager.instance.EventManager.RemoveListener(EventNames.RestartGame, CancelAllCoroutines);
    }

    // Run a function X times every Y seconds
    public void RunFunctionXTImes(EventNames eventName, object parameters, int repeatitions, float interval)
    {
        Coroutine coroutine =
            _coroutineRunner.StartCoroutine(RunXTImesCoroutine(eventName, parameters, repeatitions, interval));
        _runningCoroutines.Add(coroutine);
    }

    private IEnumerator RunXTImesCoroutine(EventNames eventName, object parameters, int x, float interval)
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
    public void RunFunctionInfinitely(EventNames eventName, object parameters, float interval)
    {
        Coroutine coroutine = _coroutineRunner.StartCoroutine(RunInfinitelyCoroutine(eventName, parameters, interval));
        _runningCoroutines.Add(coroutine);
    }

    private IEnumerator RunInfinitelyCoroutine(EventNames eventName, object parameters, float interval)
    {
        while (true)
        {
            while (_isPaused)
            {
                yield return null; // Wait until unpaused
            }

            CoreManager.instance.EventManager.InvokeEvent(eventName, parameters);
            yield return new WaitForSeconds(interval);
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

// Cancel all running coroutines
    public void CancelAllCoroutines(object obj)
    {
        foreach (var coroutine in _runningCoroutines)
        {
            _coroutineRunner.StopCoroutine(coroutine);
        }

        _runningCoroutines.Clear();
    }
}