using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = a + b;
    }
}


public class TestJobSystem : MonoBehaviour
{
    private MyJob job;

    private void Start()
    {
        NativeArray<float> res = new NativeArray<float>(1, Allocator.TempJob);
        job = new MyJob {a = 10, b = 5, result = res};

        var handler = job.Schedule();
        handler.Complete();

        float eheh = res[0];
        
        print(eheh);

        res.Dispose();
        
    }
}