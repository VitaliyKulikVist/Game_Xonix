using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.World {
	public class WorldController : MonoBehaviour, IValidateHalper {
		[Header("Components")]
		[SerializeField] private Transform _container = default;

		[field: Header("On Validate")]
		[field: SerializeField] public bool IsValidate { get; set; }

		private void Awake() {
			PrepareCamera();
		}
		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartGame;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
		}

		private void ReactionStartGame() {
			ControllVisabilityWorld();
		}

		private void ControllVisabilityWorld() {
			if(!_container.gameObject.activeSelf) {
				_container.gameObject.SetActive(true);
			}
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
