using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Helpers
{
    public class AnimationUiImage
    {
        public static void ControlImageAlpha(Image _image, bool _switch, float _duration,float _maxAlpha=1f)
        {
            _image.DOKill();
            _image.DOColor(new Color(_image.color.r, _image.color.g, _image.color.b, _switch ? _maxAlpha : 0f), _duration).SetEase(Ease.Linear);
        }
        public static void ControlImageAlpha(Image _image, bool _switch, float _duration, float _maxAlpha, Action callBack=null)
        {
            _image.DOKill();
            _image.DOColor(new Color(_image.color.r, _image.color.g, _image.color.b, _switch ? _maxAlpha : 0f), _duration).SetEase(Ease.Linear).OnComplete(()=> 
            {
                callBack?.Invoke();
            }) ;
        }
    }
}
