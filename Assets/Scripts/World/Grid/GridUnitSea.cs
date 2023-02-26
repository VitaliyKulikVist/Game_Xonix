using Assets.Scripts.Character.Trigger;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.World.Grid {
	public class GridUnitSea : GridUnit<GridUnitSeaType> {
		public override void ShowUnit(Vector3 _startPosition, GridUnitSeaType enemyType) {
			base.ShowUnit(_startPosition, enemyType);
			if (_gridUnitType == enemyType) {

				IsFree = false;
				transform.position = _startPosition;

				_container.gameObject.SetActive(true);

				if (_debug) {
					Debug.Log($"<Color=green>Show Unit </color>\tstart= {_startPosition}\ttype= {enemyType}");
				}
			}
		}

		public override void ReactionToHitCharacter(Collider colliderHitObject, GridUnit<GridUnitSeaType> gridUnit) {
			base.ReactionToHitCharacter(colliderHitObject, gridUnit);

			colliderHitObject.gameObject.GetComponent<CharacterTrigger>().HitSeaUnit(gridUnit);
		}
	}
}
