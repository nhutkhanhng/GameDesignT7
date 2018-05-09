using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapEffect : MonoBehaviour {

    public DOTweenAnimation animation;
    public AudioSource AudioSource;
    
    private void Start()
    {
        animation = GetComponent<DOTweenAnimation>();
        AudioSource = GetComponent<AudioSource>();
    }


    [ContextMenu("TrapTween")]
    public void Play()
    {
        if (animation)
        {
            animation.DOPlay();
        }
        //if (type == TweenType.Move)
        //{
        //    if (axis == AxisTween.X)
        //        obj.transform.DOLocalMoveX(endValue, duration);

        //    if (axis == AxisTween.Y)
        //        obj.transform.DOLocalMoveY(endValue, duration);

        //    if (axis == AxisTween.Z)
        //        obj.transform.DOLocalMoveZ(endValue, duration);
        //}
    }
}
