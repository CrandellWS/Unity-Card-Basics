using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour 
{
	[SerializeField]
	private GameObject _cardPrefab;

	public Dictionary<string, string> tarotCardsNames = new Dictionary<string, string>();

	public Dictionary<string, string> tarotCardsDescriptions = new Dictionary<string, string>();
	
	public readonly List<Card> CardList =  new List<Card>();							

	public void InstanatiateDeck(string cardBundlePath)
	{
		SetDictionary ();
		//AssetBundle cardBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, cardBundlePath));
		AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(cardBundlePath);
			//BundleSingleton.Instance.LoadBundle(Application.streamingAssetsPath +"/"+ cardBundlePath);

		//AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(DirectoryUtility.ExternalAssets() + cardBundlePath);

		string[] nameArray = cardBundle.GetAllAssetNames();

		ShuffleArray (nameArray);

		for (int i = 0; i < nameArray.Length; ++i)
		{
			string NameTemp = Path.GetFileNameWithoutExtension (nameArray [i]);
			GameObject cardInstance = (GameObject)Instantiate(_cardPrefab);
			Card card = cardInstance.GetComponent<Card>();
			card.gameObject.name = StringToNameValue(NameTemp);
			card.TexturePath = nameArray[ i ];
			card.SourceAssetBundlePath = cardBundlePath;
			card.transform.position = new Vector3(0, 2, 0);
			card.FaceValue = StringToNameValue(NameTemp);
			card.Description = StringToDescriptionValue (NameTemp);
			CardList.Add(card);
		}
	}

	// Use this for initialization
	void SetDictionary () {

		tarotCardsNames.Add ("00_fool", "Fool");
		tarotCardsDescriptions.Add ("00_fool", "In many esoteric systems of interpretation, the Fool is usually interpreted as the protagonist of a story, and the Major Arcana is the path the Fool takes through the great mysteries of life and the main human archetypes. This path is known traditionally in cartomancy as the \"Fool's Journey\", and is frequently used to introduce the meaning of Major Arcana cards to beginners.");

		tarotCardsNames.Add ("01_magician", "Magician");
		tarotCardsDescriptions.Add ("01_magician", "In the Magician's right hand is a wand raised toward heaven, the sky or the element æther, while his left hand is pointing to the earth. This iconographic gesture has multiple meanings, but is endemic to the Mysteries and symbolizes divine immanence, the ability of the magician to bridge the gap between heaven and earth. On the table in front of the Magician the symbols of the four Tarot suits signify the Classical elements of earth, air, fire and water. Beneath are roses and lilies, the flos campi and lilium convallium,[a] changed into garden flowers, to show the culture of aspiration.");

		tarotCardsNames.Add ("02_high_priestess", "High Priestess");
		tarotCardsDescriptions.Add ("02_high_priestess", "This Tarot card was originally called La Papesse, or \"The Popess\". Some of the cards directly linked the woman on the cards to the papacy by showing the woman wearing a triregnum or Papal Tiara. There are also some modern versions of the Tarot of Marseilles which include the keys to the kingdom that are a traditional symbol of the papacy.[2] In Protestant post-reformation countries, Tarot cards in particular used images of the legendary Pope Joan, linking in to the mythology of how Joan, disguised as a man, was elected to the papacy and was only supposedly discovered to be a woman when she gave birth.[citation needed] However, Italian Catholics appear to only have seen the La Papesse as representing the Holy Mother Church in an allegorical form,[1] with the Pope taking office becoming married to the Body of Christ, which Catholics refer to in the feminine gender.");
	}


	public static void ShuffleArray<T>(T[] arr) {
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}

	private string StringToNameValue(string input)
	{ 	
		string result = "";
		if (!tarotCardsNames.TryGetValue (input, out result))
			result = input;

		return result;
	}

	private string StringToDescriptionValue(string input)
	{
		string result = "";
		if (!tarotCardsDescriptions.TryGetValue (input, out result))
			result = input;
		return result;
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
