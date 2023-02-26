using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Enemies.Trigger;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public abstract class GridUnit<TEnum> : MonoBehaviour
		where TEnum : System.Enum {
		[Header("Just visual Controll")]
		[SerializeField] protected TEnum _gridUnitType = default;

		[Header("Components")]
		[SerializeField] protected RectTransform _container = default;
		[SerializeField] protected SpriteRenderer _spriteRendererUnit = default;

		[Header("Settings")]
		[SerializeField] protected Color _defaultColor = Color.white;
		[SerializeField] protected Color _activeColor = Color.red;

		[Header("Debug")]
		[SerializeField] protected bool _debug = true;

		#region Get\Set
		public TEnum GetGridUnitType { get => _gridUnitType; }
		public bool IsFree { get; set; } = false;
		#endregion

		#region Variable
		private Coroutine _tempCoroutine = null;
		protected string _tempName = null!;
		#endregion

		private void OnEnable() {
			HideUnit();
		}

		public virtual void SetType(TEnum gridUnitType) {

			_gridUnitType = gridUnitType;
		}

		#region Color controll
		private void ChangeColor(bool switcher) {
			_spriteRendererUnit.color = switcher ? _activeColor : _defaultColor;

			if (switcher) {
				_tempCoroutine = StartCoroutine(StartActiveColor());
			}

			else if (_tempCoroutine != null) {
				StopCoroutine(_tempCoroutine);
			}
		}
		public IEnumerator StartActiveColor() {

			yield return new WaitForSecondsRealtime(5);
			ChangeColor(false);
			_tempCoroutine = null!;
			HideUnit();
		}
		#endregion

		public void ResetUnit() {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;

			_container.position = Vector3.zero;
			_container.rotation = Quaternion.identity;
		}

		public virtual void ShowUnit(Vector3 _startPosition, TEnum enemyType) {
			if(_debug) {
				Debug.Log($"<Color=red>Show Unit </color> {enemyType}");
			}
		}

		public void HideUnit() {
			IsFree = true;
			_container.gameObject.SetActive(false);
			gameObject.name = $"HIDDEN {_tempName}";
		}
		#region Trigger reaction
		public void ReactionToHitSeaEnemy(Collider colliderHitObject) {
			colliderHitObject.gameObject.GetComponent<EnemyTriger<EnemySeaType>>().Damage(colliderHitObject);
		}

		public void ReactionToHitLandEnemy(Collider colliderHitObject) {
			colliderHitObject.gameObject.GetComponent<EnemyTriger<EnemyLandType>>().Damage(colliderHitObject);
		}

		public virtual void ReactionToHitCharacter(Collider colliderHitObject, GridUnit<TEnum> gridUnit) {
			ChangeColor(true);
		}
		#endregion

		public void SetName(string _name) {
			_tempName = _name;
			gameObject.name = _name;
			
		}
	}
}
