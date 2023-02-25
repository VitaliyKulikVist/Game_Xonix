using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Enemies.Current {
	public class EnemySea : EnemyControllerAbstract<EnemySeaType> {
		protected override void Awake() {
			base.Awake();

		}
		public override void OnEnable() {
			base.OnEnable();

		}

		public override void OnDisable() {
			base.OnDisable();

		}

		public override void ShowEnemy(Vector3 _startPosition, Vector3 _direction, EnemySeaType enemyType) {
			base.ShowEnemy(_startPosition, _direction, enemyType);
			if (enemyType != _enemyType) {
				return;
			}

			Debug.Log($"Show Enemy Sea {enemyType}\t\t start = {_startPosition}\t\t move {_direction}");
		}

		public override void LocalMotion() {
			base.LocalMotion();
		}

		public override void MoveToAttackPoint() {
			base.MoveToAttackPoint();

		}

		public override void MoveAfterAttack(float _yTarget) {
			base.MoveAfterAttack(_yTarget);

		}

		public override void DestroyEnemy() {
			base.DestroyEnemy();

		}

		#region Trigger reaction
		public override void ReactionOnCharacter(Collider collider) {
			base.ReactionOnCharacter(collider);
		}
		public override void ReactionOnLand(Collider collider) {
			base.ReactionOnLand(collider);

		}
		public override void ReactionOnSea(Collider collider) {
			base.ReactionOnSea(collider);

		}
		#endregion

		public override void GetDamage() {
			base.GetDamage();

		}
	}
}
