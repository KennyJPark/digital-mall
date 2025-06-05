using UnityEngine;
using Random = UnityEngine.Random;
using System;
[Serializable]
public struct RangedFloat
{
    public float minValue;
    public float maxValue;
}
public abstract class AudioDelegateSO : ScriptableObject
{
    public abstract void Play(AudioSource source);
}