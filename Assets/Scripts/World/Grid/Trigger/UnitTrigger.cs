using System;
using UnityEngine;

namespace Assets.Scripts.World.Grid.Trigger {
	public abstract class UnitTrigger<TEnum> : MonoBehaviour
		where TEnum: System.Enum{
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

		}

		public virtual void ResetTagToDefault() {

		}

		protected virtual void OnTriggerEnter(Collider other) {

		}
	}
}
