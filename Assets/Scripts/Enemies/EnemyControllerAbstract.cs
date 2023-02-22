using Assets.Scripts.Common;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public abstract class EnemyControllerAbstract : Enemy, IEnemyController {

		public virtual void OnEnable() {

			HideEnemy();

			LocalMotion();
			KillEnemies += DestroyEnemy;
		}

		public virtual void OnDisable() {
			enemyContainer.DOKill();
			KillEnemies -= DestroyEnemy;
		}

		public virtual void OnDestroy() {
			transform.DOKill();
		}

		private void HideEnemy() {
			IsFree = true;
			enemyContainer.gameObject.SetActive(false);
		}

		public void ShowEnemy(Vector3 _startPosition, Vector3 _direction, EnemyType enemyType) {
			if(enemyType != _enemyType) {
				return;
			}

			IsFree = false;
			target = _startPosition + _direction * 50f;
			transform.position = _startPosition;
			transform.forward = target - _startPosition;
			enemyContainer.gameObject.SetActive(true);
			transform.DOMove(target + transform.forward * 5f, Vector3.Distance(_startPosition, target) / _enemySpeed).SetEase(Ease.Linear).OnComplete(() => {
				HideEnemy();
			});
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
			EnemyRichPlayerAction?.Invoke(this);
			Vector3 target = transform.TransformPoint(Vector3.forward * 40f);
			target.y = _yTarget;
			transform.DORotateQuaternion(Quaternion.LookRotation(target - transform.position), 1f);
			transform.DOMove(target, Vector3.Distance(target, transform.position) / EnemySpeed).SetEase(Ease.Linear).OnComplete(() => {
				DestroyEnemy();
			});
		}

		public virtual void DestroyEnemy() {
			EnemyKilledAction?.Invoke(this);
				enemyContainer.gameObject.SetActive(false);
				Destroy(gameObject);
		}

		public virtual void GetDamage() {
				IsKilled = true;
				enemyContainer.DOKill();
				DestroyEnemy();
		}
	}
}
