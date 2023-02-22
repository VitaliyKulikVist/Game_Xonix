namespace Assets.Scripts.Enemies {
	public interface IEnemyController {
		void LocalMotion();
		void MoveToAttackPoint();
		void MoveAfterAttack(float _yTarget);
		void DestroyEnemy();
		void GetDamage();
	}
}
