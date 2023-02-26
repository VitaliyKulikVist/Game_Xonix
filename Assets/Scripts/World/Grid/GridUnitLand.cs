using Assets.Scripts.Character.Trigger;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridUnitLand : GridUnit<GridUnitLandType> {
		public override void ShowUnit(UnityEngine.Vector3 _startPosition, GridUnitLandType enemyType) {
			base.ShowUnit(_startPosition, enemyType);
			if (_gridUnitType == enemyType) {

				IsFree = false;
				transform.position = _startPosition;

				gameObject.name = _tempName;
				_container.gameObject.SetActive(true);

				if (_debug) {
					Debug.Log($"<Color=green>Show Unit </color>\tstart= {_startPosition}\ttype= {enemyType}");
				}
			}
		}

		public override void ReactionToHitCharacter(Collider colliderHitObject, GridUnit<GridUnitLandType> gridUnit) {
			base.ReactionToHitCharacter(colliderHitObject, gridUnit);

			colliderHitObject.gameObject.GetComponent<CharacterTrigger>().HitLandUnit(gridUnit);
		}
	}
}
