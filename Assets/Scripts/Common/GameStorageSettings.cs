using UnityEngine;
using System;

namespace Assets.Scripts.Common {
	[CreateAssetMenu(menuName = "ScriptableObjects/GameStorageSettings", fileName = "GameStorageSettings")]
    public class GameStorageSettings : ScriptableObject
    {
		[field: Header("UI Block")]
        [field: SerializeField, Range(0f, 2f)] public float UiDuration { get; set; } = 0.3f;
	}
}
