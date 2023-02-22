using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Character {
	public class CharacterController : MonoBehaviour {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;

		[Header("Components")]
		[SerializeField] private RectTransform _container = default;
		[SerializeField]private Rigidbody2D _rigidbody2D = default;

		[Header("Settings")]
		[SerializeField] private float _speedMove = 5f;

		#region Variable
		private bool _canMove = false;
		private DynamicJoystick _dynamicJoystick = default;
		#endregion

		private void Awake() {
			ResetPlayer();
			_dependencyInjections.PlayerPosition = _container;
		}
		private void Start() {
			_dynamicJoystick = _dependencyInjections.DynamicJoystick;
		}
		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartGame;
			GameManager.LevelFinishAction+= ReactionFinishGame;
			GameManager.PausedLevelAction += ReactionPaused;
			GameManager.PlayLevelAction += ReactionPlay;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartGame;
			GameManager.LevelFinishAction -= ReactionFinishGame;
			GameManager.PausedLevelAction -= ReactionPaused;
			GameManager.PlayLevelAction -= ReactionPlay;
		}
		private void FixedUpdate() {
			if (_canMove) {
				Vector3 direction = Vector3.up * _dynamicJoystick.Vertical + Vector3.right * _dynamicJoystick.Horizontal;

				_rigidbody2D.AddForce(direction * _speedMove * Time.fixedDeltaTime);
			}
		}

		private void ResetPlayer() {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			_container.position = Vector3.zero;
			_container.rotation= Quaternion.identity;
			_container.localScale = Vector3.one;
		}

		#region Reaction to Action
		private void ReactionStartGame() {
			_canMove = true;
		}

		private void ReactionFinishGame(LevelResult levelResult) {
			_canMove =false;
		}
		private void ReactionPaused() {
			_canMove =false;
		}

		private void ReactionPlay() {
			_canMove = true;
		}
		#endregion
	}
}
