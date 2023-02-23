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
		[SerializeField] protected Color _seaColor = Color.blue;
		[SerializeField] protected Color _landColor = Color.red;

		#region Get\Set
		public TEnum GetGridUnitType { get => _gridUnitType; }
		public bool IsFree { get; set; } = false;
		#endregion

		private void OnEnable() {
			HideUnit();
		}

		public virtual void SetType(TEnum gridUnitType) {

			_gridUnitType = gridUnitType;
		}
		public void ChangeColor(bool switcher) {
			_spriteRendererUnit.color = switcher ? _landColor : _seaColor;
		}

		public void ResetUnit() {
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;

			_container.position = Vector3.zero;
			_container.rotation = Quaternion.identity;
		}

		public virtual void ShowUnit(Vector3 _startPosition, TEnum enemyType) {
			if (!Equals(_gridUnitType, enemyType)) {
				return;
			}

			IsFree = false;
			transform.position = _startPosition;
			_container.gameObject.SetActive(true);
		}

		private void HideUnit() {
			IsFree = true;
			_container.gameObject.SetActive(false);
		}

		public void SetName(string _name) {
			gameObject.name = _name;
		}
	}
}
