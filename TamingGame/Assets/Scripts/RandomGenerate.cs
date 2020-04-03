using System.Collections;
using System.Collections.Generic;
using System;

public static class RandomGenerate
{
    public static int GetRandomInt()
    {
        Random rand = new Random();
        return rand.Next();
    }

    public static float GetRandomFloat(float _min, float _max)
    {
        float result = 0;
        Random rand = new Random();
        result = (float)rand.NextDouble();
        result += _min + GetRandomInt() % (_max - _min);

        return result;

    }
}
