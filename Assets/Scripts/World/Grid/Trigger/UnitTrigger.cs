using System;
using UnityEngine;

namespace Assets.Scripts.World.Grid.Trigger {
	public abstract class UnitTrigger<TEnum> : MonoBehaviour
		where TEnum: Enum{

		[Header("Debug")]
		[SerializeField] protected bool _debug = false;

		#region Variables
		protected GridUnit<TEnum> _gridUnit = default;
		#endregion

		protected virtual void OnEnable() {
			_gridUnit = GetComponentInParent<GridUnit<TEnum>>();

			if(_gridUnit == null) {
				throw new ArgumentNullException(nameof(_gridUnit));
			}
		}

		protected virtual void SwitchTag() {
			if (_debug) {
				Debug.LogError($"<color=red>Switch unit Tag</color> {_gridUnit.GetGridUnitType}");
			}
		}

		public virtual void ResetTagToDefault() {
			if (_debug) {
				Debug.LogError($"<color=red>Reset Tag To Default</color> {_gridUnit.GetGridUnitType}");
			}
		}

		protected virtual void OnTriggerEnter2D(Collider2D other) {
			if (_debug) {
				Debug.Log($"<color=red>On Trigger Enter</color> {_gridUnit.GetGridUnitType}\t\t {other.gameObject.name}");
			}
		}
	}
}
