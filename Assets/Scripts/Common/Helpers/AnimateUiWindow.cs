using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Common.Helpers
{
    public class AnimationUiWindow
    {
        public enum PositionMoveType
        {
            None,
            OX,
            OY,
        }
        public static void AnimationWindowControll(Transform windowTransform, bool _switch, float _duration, Action _callBack = null)
        {
            windowTransform.DOKill();
            if (_switch)
            {
                windowTransform.gameObject.SetActive(true);
                windowTransform.DOKill();
                windowTransform.DOScale(Vector3.one, _duration).OnComplete(() =>
                {
                    _callBack?.Invoke();
                });
            }
            else
            {
                windowTransform.DOKill();
                windowTransform.DOScale(Vector3.zero, _duration).OnComplete(() =>
                {
                    windowTransform.gameObject.SetActive(false);
                    _callBack?.Invoke();
                });
            }
        }
        public static void AnimationWindowControll(RectTransform windowTransform, PositionMoveType positionMoveType, bool _switch, float _duration, Action _callBack = null)
        {
            windowTransform.DOKill();
            if (_switch)
            {
                windowTransform.gameObject.SetActive(true);
                windowTransform.DOKill();
                windowTransform.DOAnchorPos(Vector3.zero, _duration).OnComplete(() =>
                {
                    _callBack?.Invoke();
                });
            }
            else
            {
                windowTransform.DOKill();
                windowTransform.DOAnchorPos(GetPositionMoveByType(positionMoveType, _switch), _duration).OnComplete(() =>
                {
                    windowTransform.gameObject.SetActive(false);
                    _callBack?.Invoke();
                });
            }
        }
        private static Vector3 GetPositionMoveByType(PositionMoveType _positionMoveType, bool _switch, float _lenght=2800f)
        {
            if (_switch)
            {
                switch (_positionMoveType)
                {
                    case PositionMoveType.None:
                        return Vector3.zero;
                    case PositionMoveType.OX:
                        return new Vector3 ( -_lenght, 0, 0);
                    case PositionMoveType.OY:
                        return new Vector3( 0, -_lenght, 0);
                    default:
                        return Vector3.zero;
                }
            }
            else
            {
                switch (_positionMoveType)
                {
                    case PositionMoveType.None:
                        return Vector3.zero;
                    case PositionMoveType.OX:
                        return new Vector3(_lenght, 0, 0);
                    case PositionMoveType.OY:
                        return new Vector3(0, _lenght, 0);
                    default:
                        return Vector3.zero;
                }
            }
        }
    }
}


