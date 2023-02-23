using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Enemies.Storage;
using Assets.Scripts.Player;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Enemies {
	public class EnemySpowner : MonoBehaviour, IValidateHalper {
		[Header("Base")]
		[SerializeField] private DependencyInjections _dependencyInjections = default;
		[SerializeField] private EnemyStorage _enemyStorageSO = default;
		[SerializeField] private GameStorageSettings _gameStorageSettingsSO = default;
		[SerializeField] private LevelStorage _levelStorageSO = default;
		[SerializeField] private PlayerStorage _playerStorageSO = default;

		[Header("Enemy")]
		[SerializeField] private Transform _enemiesContainer = default;
		[SerializeField] private Transform _containerSea = default;
		[SerializeField] private Transform _containerLand = default;

		[Header("Pool settings")]
		[SerializeField] private int _startingEnemiesCountPool = 30;

		[field: Header("On Validate")]
		[field: SerializeField] public bool IsValidate { get; set; }

		#region Variables
		private Coroutine _spownEnemiesCoroutine = null;
		private Coroutine _selectNearTargetCoroutine = null;
		private List<EnemyControllerAbstract> _enemys = new List<EnemyControllerAbstract>();
		[SerializeField] private List<EnemyControllerAbstract> _enemies = new List<EnemyControllerAbstract>();
		[SerializeField] private Level _currentLevel = default;
		private int spownerNumber = 0;
		#endregion

		#region Get\Set
		public bool LevelStarted { get; set; } = false;
		#endregion

		#region Actions
		public static Action<Vector3, Vector3, EnemyType> SpawnEnemyAction = default;
		#endregion

		private void Awake() {
			PrepareEnemys();
			PrepareCamera();
		}

		private void OnEnable() {
			GameManager.LevelStartAction += StartSpownEnemies;
			GameManager.LevelFinishAction += StopSpownEnemies;
			EnemyControllerAbstract.EnemyKilledAction += EnemyKillReaction;
			EnemyControllerAbstract.EnemyRichPlayerAction += EnemyRichPlayer;

			SpawnEnemyAction += SpawnEnemy;
		}

		private void OnDisable() {
			GameManager.LevelStartAction -= StartSpownEnemies;
			GameManager.LevelFinishAction -= StopSpownEnemies;
			EnemyControllerAbstract.EnemyKilledAction -= EnemyKillReaction;
			EnemyControllerAbstract.EnemyRichPlayerAction -= EnemyRichPlayer;

			SpawnEnemyAction -= SpawnEnemy;
		}

		private void StartSpownEnemies() {
			_currentLevel = _levelStorageSO.GetNextLevel(_playerStorageSO.ConcretePlayer.PlayerLevel);

			//Change texture

			LevelStarted = true;

			if (_spownEnemiesCoroutine != null) {
				StopCoroutine(_spownEnemiesCoroutine);
			}
			_spownEnemiesCoroutine = StartCoroutine(SpownEnemies());

			if (_selectNearTargetCoroutine != null) {
				StopCoroutine(_selectNearTargetCoroutine);
			}
			_selectNearTargetCoroutine = StartCoroutine(SelectNearEnemy());
		}

		private void StopSpownEnemies(LevelResult _levelResult) {
			LevelStarted = false;

			if (_spownEnemiesCoroutine != null) {
				StopCoroutine(_spownEnemiesCoroutine);
				_spownEnemiesCoroutine = null;
			}

			if (_selectNearTargetCoroutine != null) {
				StopCoroutine(_selectNearTargetCoroutine);
			}
			_selectNearTargetCoroutine = null;

			EnemyControllerAbstract.KillEnemies?.Invoke();
		}

		private IEnumerator SelectNearEnemy() {
			while (LevelStarted) {

				yield return null;
			}
		}

		private IEnumerator SpownEnemies() {
			while (LevelStarted) {
				if (_enemies.Count + spownerNumber < _currentLevel.MaximumEnemies) {
					SpownEnemy();
				}
				yield return new WaitForSeconds(_currentLevel.GetTimeSpawnEnemies);
			}
		}

		private void SpownEnemy() {
			
		}

		private void PrepareEnemys() {
			for (int i = 0; i < _startingEnemiesCountPool; i++) {
				for (int j = 0; j < typeof(EnemyType).GetEnumValues().Length; j++) {
					if((EnemyType)j != EnemyType.None) {
						AddEnemy((EnemyType)j);
					}
				}
			}
		}

		private void AddEnemy(EnemyType enemyType) {
			var enemyControllerAbstract = _enemyStorageSO.GetEnemyByType(enemyType).GetEnemyController;

			Transform containerSpawn = null;
			if(enemyType == EnemyType.Land) {
				containerSpawn = _containerLand;
			}

			else if(enemyType == EnemyType.Sea) {
				containerSpawn = _containerSea;
			}
			var enemy = Instantiate(enemyControllerAbstract, containerSpawn);
			enemy.ResetEnemy();

			_enemys.Add(enemy);
		}

		private void SpawnEnemy(Vector3 _startPosition, Vector3 _direction, EnemyType enemyType) {
			if (_enemys.Find(enem => enem.IsFree && enem.EnemyType == enemyType) == null) {
				AddEnemy(enemyType);
			}

			_enemys.Find(someEnemy => someEnemy.IsFree).ShowEnemy(_startPosition, _direction, enemyType);
		}

		private void EnemyKillReaction(EnemyControllerAbstract _controller) {
			if (_enemies.Contains(_controller)) {
				_enemies.Remove(_controller);
			}

			if (_controller.IsKilled && _currentLevel.GetLevelEnemies.FindAll(someEnemy => someEnemy == _controller.EnemyType).Count > 0) {
				_currentLevel.GetLevelEnemies.Remove(_currentLevel.GetLevelEnemies.Find(someEnemy => someEnemy == _controller.EnemyType));
			}

			if (LevelStarted && _currentLevel.GetLevelEnemies.Count == 0) {
				GameManager.LevelFinishAction?.Invoke(LevelResult.Win);
			}
		}

		private void EnemyRichPlayer(EnemyControllerAbstract _controller) {
			if (_enemies.Contains(_controller)) {
				_enemies.Remove(_controller);
			}
		}

		private void PrepareCamera() {
			var canvas = gameObject.GetComponent<Canvas>();

			if (canvas != null 
				&& canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace 
				&& canvas.worldCamera == null) {
				canvas.worldCamera = Camera.main;
			}
		}

		public void OnValidate() {
			if(IsValidate) {
				PrepareCamera();
			}
		}
	}
}
