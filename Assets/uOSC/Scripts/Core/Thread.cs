﻿using UnityEngine;
using System;
using System.Threading;

namespace uOSC
{

public class Thread
{
    private const int IntervalMillisec = 1;

    System.Threading.Thread thread_;
    bool isRunning_ = false;
    System.Action loopFunc_ = null;

    public void Start(System.Action loopFunc)
    {
        if (isRunning_) return;

        isRunning_ = true;
        loopFunc_ = loopFunc;

        thread_ = new System.Threading.Thread(() => 
        {
            try
            {
                while (isRunning_)
                {
                    if (loopFunc_ != null) 
                    {
                        loopFunc_();
                    }
                    System.Threading.Thread.Sleep(IntervalMillisec);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
        });
        thread_.Start();
    }

    public void Stop(int timeoutMilliseconds = 3000)
    {
        if (!isRunning_) return;

        isRunning_ = false;

        if (thread_.IsAlive)
        {
            thread_.Join(timeoutMilliseconds);
            if (thread_.IsAlive)
            {
                thread_.Abort();
            }
        }
    }
}

}