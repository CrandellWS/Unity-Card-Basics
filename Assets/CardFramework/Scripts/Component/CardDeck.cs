using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour 
{
	[SerializeField]
	private GameObject _cardPrefab;	
	
	public readonly List<Card> CardList =  new List<Card>();							

	public void InstanatiateDeck(string cardBundlePath)
	{

		//AssetBundle cardBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, cardBundlePath));
		AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(cardBundlePath);
			//BundleSingleton.Instance.LoadBundle(Application.streamingAssetsPath +"/"+ cardBundlePath);

		//AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(DirectoryUtility.ExternalAssets() + cardBundlePath);

		string[] nameArray = cardBundle.GetAllAssetNames();

		ShuffleArray (nameArray);

		for (int i = 0; i < nameArray.Length; ++i)
		{
			GameObject cardInstance = (GameObject)Instantiate(_cardPrefab);
			Card card = cardInstance.GetComponent<Card>();
			card.gameObject.name = Path.GetFileNameWithoutExtension(nameArray[ i ]);
			card.TexturePath = nameArray[ i ];
			card.SourceAssetBundlePath = cardBundlePath;
			card.transform.position = new Vector3(0, 2, 0);
			card.FaceValue = card.gameObject.name;
			CardList.Add(card);
		}
	}

	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}

	private int StringToFaceValue(string input)
	{
		for (int i = 2; i < 11; ++i)
		{
			if (input.Contains(i.ToString()))
			{
				return i;
			}
		}
		if (input.Contains("jack") ||
		    input.Contains("queen") ||
		    input.Contains("king"))
		{
			return 10;
		}
		if (input.Contains("ace"))
		{
			return 11;
		}
		return 0;
	}
}
