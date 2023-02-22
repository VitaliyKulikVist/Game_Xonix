using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.World {
	public class WorldController : MonoBehaviour {

		[Header("Base")]
		[SerializeField] private LevelStorage _levelStorageSO = default;

		[Header("Components")]
		[SerializeField] private Transform _container = default;

		[Header("Images")]
		[SerializeField]private Image _imageSea = default;
		[SerializeField] private Image _imageLand = default;

		private void OnEnable() {
			GameManager.LevelStartAction += ReactionStartLevel;
		}
		private void OnDisable() {
			GameManager.LevelStartAction -= ReactionStartLevel;
		}

		private void ReactionStartLevel() {
			PrepareSeaImage();
			PrepareLandImage();
		}

		private void PrepareSeaImage() {
			_imageSea.sprite = _levelStorageSO.BaseLevelSettings.GetRandomSeaSprite;
		}
		private void PrepareLandImage() {
			_imageLand.sprite = _levelStorageSO.BaseLevelSettings.GetRandomLandSprite;
		}
	}
}
