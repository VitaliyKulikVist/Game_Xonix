using System;
using Assets.Scripts.Common;
using Assets.Scripts.Enemies;
using Assets.Scripts.World.Grid;
using UnityEngine;

namespace Assets.Scripts.Character.Trigger {
	public class PlayerTrigger : MonoBehaviour {

		[Header("Debug")]
		[SerializeField] private bool _debug = false;

		#region Variables
		private PlayerController _characterController = default;
		#endregion

		private void OnEnable() {
			_characterController = GetComponentInParent<PlayerController>();
			if (_characterController == null) {
				throw new ArgumentNullException(nameof(_characterController));
			}
		}

		public void HitLandUnit(GridUnit<GridUnitLandType> gridUnit) {
			_characterController.HitLandUnit(gridUnit);

			if (_debug) {
				Debug.Log($"<color=green>Hit Land Unit</color> {gridUnit.GetGridUnitType}");
			}
		}

		public void HitSeaUnit(GridUnit<GridUnitSeaType> gridUnit) {
			_characterController.HitSeaUnit(gridUnit);

			if (_debug) {
				Debug.Log($"<color=green>Hit Sea Unit</color> {gridUnit.GetGridUnitType}");
			}
		}

		public void HitEnemy<TEnum>(EnemyControllerAbstract<TEnum> enemy)
			where TEnum : Enum {
			_characterController.HitEnemt(enemy);

			if (_debug) {
				Debug.Log($"<color=green>Hit Enemy</color> {enemy.EnemyType}");
			}
		}
	}
}
