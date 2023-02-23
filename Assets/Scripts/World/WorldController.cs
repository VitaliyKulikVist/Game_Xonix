using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.World {
	public class WorldController : MonoBehaviour, IValidateHalper {

		[Header("Base")]
		[SerializeField] private LevelStorage _levelStorageSO = default;

		[Header("Components")]
		[SerializeField] private Transform _container = default;

		[field: Header("On Validate")]
		[field: SerializeField] public bool IsValidate { get; set; }

		private void Awake() {
			PrepareCamera();
		}

		private void PrepareCamera() {
			var canvas = gameObject.GetComponent<Canvas>();

			if (canvas != null
				&& canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace
				&& canvas.worldCamera == null) {
				canvas.worldCamera = Camera.main;
			}
		}

		public void OnValidate() {
			if (IsValidate) {
				PrepareCamera();
			}
		}
	}
}
