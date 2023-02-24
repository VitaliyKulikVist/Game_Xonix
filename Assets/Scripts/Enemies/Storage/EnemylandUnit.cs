using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.Enemies.Storage {
	[Serializable]
	public class EnemylandUnit : EnemyUnit<EnemyLandType> {
		public override void SetEnemyControllerType() {
			base.SetEnemyControllerType();

		}

		public override void SetEnemyMoveSpeed() {
			base.SetEnemyMoveSpeed();

		}

		public override void SetEnemyAttackDistance() {
			base.SetEnemyAttackDistance();

		}
	}
}
