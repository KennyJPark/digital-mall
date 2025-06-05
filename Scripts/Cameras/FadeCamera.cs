using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place on Camera Object; CM vcam1
public class FadeCamera : MonoBehaviour
{
    //[SerializeField]
    private float desiredAlpha = 0;
    //[SerializeField]
    private float currentAlpha = 1;

    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));

    [SerializeField]
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private bool _started;
    private float _time;

    public void Reset()
    {
        Debug.Log("Reset");
        _done = false;
        _started = false;
        _alpha = 1;
        _time = 0;
        ResetCurrentAlpha();
        ResetDesiredAlpha();
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade()
    {
        Debug.Log("RedoFade");
        Reset();
    }

    public void OnGUI()
    {
        //Debug.Log("Current Alpha: " + currentAlpha);
        if (!_started)
        {
            Debug.Log("Current Alpha: " + currentAlpha);
            _started = true;

        }
        if (_done)
        {
            Debug.Log("Fade Done");
            _started = false;
            enabled = false;
            return;
        }

        if (_texture == null) _texture = new Texture2D(1, 1);

        _texture.SetPixel(0, 0, new Color(0, 0, 0, currentAlpha));
        _texture.Apply();

        //_time += Time.deltaTime;
        //_alpha = FadeCurve.Evaluate(_time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        if (currentAlpha <= 0)
        {
            _done = true;
            _started = false;
        }
        
    }
    
    void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, 1.0f * Time.deltaTime);
        //_alpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, 0.01f * Time.deltaTime);
        //Debug.Log("Current Alpha: " + currentAlpha);
    }


    
    public void ResetCurrentAlpha()
    {
        if (currentAlpha < 1)
        {
            //colorProperty.a = currentAlpha;
            currentAlpha = 1;
            //_alpha = 1;
        }

    }
    public void ResetDesiredAlpha()
    {
        if (desiredAlpha > 0)
        {
            desiredAlpha = 0;
        }
        
    }

    public void SetDesiredAlpha(float alpha)
    {
        desiredAlpha = alpha;
    }
}
