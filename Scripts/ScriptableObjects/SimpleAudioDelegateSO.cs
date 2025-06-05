using UnityEngine;

[CreateAssetMenu(fileName = "AudioDelegate")]
public class SimpleAudioDelegateSO : AudioDelegateSO
{
    public AudioClip[] clips;
    public RangedFloat volume;
    public RangedFloat pitch;
    public override void Play(AudioSource source)
    {
        if (clips.Length == 0 || source == null)
            return;
            source.clip = clips[Random.Range(0, clips.
            Length)];
            source.volume = Random.Range(volume.minValue,
            volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.
            maxValue);
            source.Play();
    }
}