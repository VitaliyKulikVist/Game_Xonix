using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class UiHelper
    {
        public static void AnimationWindowControll(Transform windowTransform, bool _switch, float _duration, Action _callBack = null)
        {
            windowTransform.DOComplete();
            if (_switch)
            {
                windowTransform.gameObject.SetActive(true);
                windowTransform.DOScale(Vector3.one, _duration).OnComplete(() =>
                {
                    _callBack?.Invoke();
                });
            }
            else
            {
                windowTransform.transform.DOScale(Vector3.zero, _duration).OnComplete(() =>
                {
                    windowTransform.gameObject.SetActive(false);
                    _callBack?.Invoke();
                });
            }
        }
    }
}
