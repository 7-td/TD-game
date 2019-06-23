using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DelayFunc 
{
    public static IEnumerator Invoke(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
