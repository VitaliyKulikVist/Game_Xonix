using System;
using Assets.Scripts.Common;
using Assets.Scripts.Enemies;
using Assets.Scripts.World.Grid;
using UnityEngine;

namespace Assets.Scripts.Character.Trigger {
	public class CharacterTrigger : MonoBehaviour {

		#region Variables
		private CharacterController _characterController = default;
		#endregion

		private void OnEnable() {
			_characterController = GetComponentInParent<CharacterController>();
			if (_characterController == null) {
				throw new ArgumentNullException(nameof(_characterController));
			}
		}

		public void HitLandUnit(GridUnit<GridUnitLandType> gridUnit) {
			_characterController.HitLandUnit(gridUnit);
		}

		public void HitSeaUnit(GridUnit<GridUnitSeaType> gridUnit) {
			_characterController.HitSeaUnit(gridUnit);
		}

		public void HitEnemy<TEnum>(EnemyControllerAbstract<TEnum> enemy)
			where TEnum: System.Enum {
			_characterController.HitEnemt(enemy);
		}
	}
}
