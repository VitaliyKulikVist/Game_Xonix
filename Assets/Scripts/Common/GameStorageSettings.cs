using UnityEngine;
using System;

namespace Assets.Scripts.Common
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameStorageSettings", fileName = "GameStorageSettings")]
    public class GameStorageSettings : ScriptableObject
    {
		[field: Header("UI Block")]
        [field: SerializeField, Range(0f, 2f)] public float UiDuration { get; } = 0.3f;
	}

	[Serializable]
	public static class BasePlayerConstants {
		public static int MaxPlayerLife = 3;
		public static int MinPlayerLevel = 0;
	}
}
