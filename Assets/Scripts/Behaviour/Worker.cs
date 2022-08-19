using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] private Room _workRoom;
    [SerializeField] private Room _restRoom;
    [SerializeField] private Room _sleepRoom;
    [SerializeField] private Walker _walker;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Clock _clock;
    private int lastTimeCheck = 0;

    private void Start()
    {
        _walker = GetComponentInChildren<Walker>();
        _progressBar = GetComponentInChildren<ProgressBar>();
    }

    private void Update()
    {
        int time = _clock.GetHoursOfDay();
        if (lastTimeCheck != time)
        {
            lastTimeCheck = time;
            DoNextAction();
        }
    }

    private void DoNextAction()
    {
        if (lastTimeCheck == 7) Work();
        else if (lastTimeCheck == 18) Rest();
        else if (lastTimeCheck == 22) Sleep();
    }

    private void Work()
    {
        _walker.Walk(_workRoom);
    }

    private void Rest()
    {
        _walker.Walk(_restRoom);
    }

    private void Sleep()
    {
        _walker.Walk(_sleepRoom);
    }
}
