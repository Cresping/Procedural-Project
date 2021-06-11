using System;
using System.Collections;
using System.Collections.Generic;

public static class ListUtils
{
    public static List<T> UnsortList<T>(List<T> input)
    {
        List<T> arr = input;
        List<T> arrDes = new List<T>();
        Random randNum = new Random();
        while (arr.Count > 0)
        {
            int val = randNum.Next(0, arr.Count - 1);
            arrDes.Add(arr[val]);
            arr.RemoveAt(val);
        }
        return arrDes;
    }
}
