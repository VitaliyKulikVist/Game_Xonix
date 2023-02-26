using Assets.Scripts.Character.Trigger;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public abstract class EnemyControllerAbstract<TEnum> : Enemy<TEnum>, IEnemyController 
		where TEnum : System.Enum{

		public virtual void OnEnable() {

			HideEnemy();

			//LocalMotion();
			KillEnemies += DestroyEnemy;
		}

		protected override void Awake() {
			base.Awake();

		}
		public virtual void OnDisable() {
			enemyContainer.DOKill();
			KillEnemies -= DestroyEnemy;
		}

		public virtual void OnDestroy() {
			transform.DOKill();
		}

		public void HideEnemy() {
			IsFree = true;
			enemyContainer.gameObject.SetActive(false);
			gameObject.name = $"HIDDEN {_tempName}";
			ResetEnemy();
		}

		public virtual void ShowEnemy(Vector3 _startPosition, Vector3 _direction, TEnum enemyType) {
			if(_debug) {
				Debug.Log($"<Color=red>Show Enemy</color> {enemyType}");
			}
		}

		public void HitPlayer() {
			transform.DOKill();
			HideEnemy();
		}

		public virtual void LocalMotion() {
			if (!IsKilled) {

			}
		}

		public virtual void MoveToAttackPoint() {
			var positionAttack = _dependencyInjections.PlayerPosition.position;
			float distance = Vector3.Distance(transform.position, positionAttack);
			Vector3 target = positionAttack + (transform.position - positionAttack) * (_attackDistance / distance);

			transform.DOMove(target, Vector3.Distance(target, transform.position) / EnemySpeed).SetEase(Ease.Linear);
		}

		public virtual void MoveAfterAttack(float _yTarget) {
			Vector3 target = transform.TransformPoint(Vector3.forward * 40f);
			target.y = _yTarget;
			transform.DORotateQuaternion(Quaternion.LookRotation(target - transform.position), 1f);
			transform.DOMove(target, Vector3.Distance(target, transform.position) / EnemySpeed).SetEase(Ease.Linear).OnComplete(() => {
				DestroyEnemy();
			});
		}

		public virtual void DestroyEnemy() {
				enemyContainer.gameObject.SetActive(false);
				Destroy(gameObject);
		}

		#region Trigger reaction
		public virtual void ReactionOnCharacter(Collider2D collider) {
			collider.gameObject.GetComponent<CharacterTrigger>().HitEnemy(this);
		}
		public virtual void ReactionOnLand(Collider2D collider) {

		}
		public virtual void ReactionOnSea(Collider2D collider) {

		}
		#endregion

		public virtual void GetDamage() {
				IsKilled = true;
				enemyContainer.DOKill();
				DestroyEnemy();
		}
	}
}
