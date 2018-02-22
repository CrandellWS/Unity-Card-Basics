using UnityEngine;
using System.Collections;
using System.IO;

public class Tarot3Cards : MonoBehaviour 
{
	public Tarot3CardsDealerUI Tarot3CardsDealerUIInstance { get; set; }
    
	[SerializeField]
	private CardDeck _cardDeck;	

	[SerializeField]
	private CardSlot _pickupCardSlot;		

	[SerializeField]
	private CardSlot _stackCardSlot;	

	[SerializeField]
	private CardSlot _discardStackCardSlot;		

	[SerializeField]
	private CardSlot _discardHoverStackCardSlot;			

	[SerializeField]
	private CardSlot _rightHandCardSlot;

	[SerializeField]
	private CardSlot _leftHandCardSlot;

	[SerializeField]
	public CardSlot _currentCardSlot;	

	[SerializeField]
	public CardSlot _prior0CardSlot;	

	[SerializeField]
	public CardSlot _prior1CardSlot;	

	[SerializeField]
	public CardSlot _prior2CardSlot;

	private const float CardStackDelay = .001f;
	
	/// <summary>
	/// Counter which keeps track current dealing movements in progress.
	/// </summary>
	public int DealInProgress { get; set; }

	private void Awake()
	{
		_cardDeck.InstanatiateDeck("tarotbasic");
		//_cardDeck.InstanatiateDeck("cards");
		StartCoroutine(StackCardRangeOnSlot(0, _cardDeck.CardList.Count, _stackCardSlot));
	}

	private void MoveCardSlotToCardSlot(CardSlot sourceCardSlot, CardSlot targerCardSlot) 
	{
		Card card;
		while ((card = sourceCardSlot.TopCard()) != null)
		{
			targerCardSlot.AddCard(card);
		}
	}
	
	private IEnumerator StackCardRangeOnSlot(int start, int end, CardSlot cardSlot) 
	{
		DealInProgress++;
		for (int i = start; i < end; ++i)
		{
			cardSlot.AddCard(_cardDeck.CardList[i]);
			yield return new WaitForSeconds(CardStackDelay);
		}
		DealInProgress--;
	}

	public IEnumerator GatherAllCoroutine()
	{
		DealInProgress++;

		Tarot3CardsDealerUIInstance.FaceValueText.text = " ";
		MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);
		cardCheveronCurrent = false;
		MoveCardSlotToCardSlot(_discardStackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_discardHoverStackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_rightHandCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_leftHandCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior0CardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior1CardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior2CardSlot, _pickupCardSlot);
		//MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_pickupCardSlot, _stackCardSlot);

		cheveronReset();		
		yield return new WaitForSeconds(.1f);	

		DealInProgress--;
	}

	public IEnumerator GatherOthersCoroutine()
	{
		DealInProgress++;
		Tarot3CardsDealerUIInstance.FaceValueText.text = " ";
		MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);
		cardCheveronCurrent = false;
		MoveCardSlotToCardSlot(_discardStackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_discardHoverStackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_rightHandCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_leftHandCardSlot, _pickupCardSlot);
//		MoveCardSlotToCardSlot(_pickupCardSlot, _stackCardSlot);
		MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);
		yield return new WaitForSeconds(.1f);

		DealInProgress--;
	}





    /// <summary>
    /// Shuffle Coroutine.
    /// Moves all card to pickupCardSlot. Then shuffles them back
	/// to cardStackSlot.
    /// </summary>
	public IEnumerator ShuffleCoroutine()
	{
		DealInProgress++;

		yield return StartCoroutine(GatherOthersCoroutine ());

		int halfLength = _pickupCardSlot.CardList.Count / 2;
		int fullLength = _pickupCardSlot.CardList.Count;

		for (int i = 0; i < halfLength; ++i)
		{
			_leftHandCardSlot.AddCard(_pickupCardSlot.TopCard());
		}
		yield return new WaitForSeconds(.01f);	
		for (int i = 0; i < halfLength; ++i)
		{
			_rightHandCardSlot.AddCard(_pickupCardSlot.TopCard());
		}
		yield return new WaitForSeconds(.01f);	

		_stackCardSlot.AddCard(_pickupCardSlot.TopCard());//jic odd
		yield return new WaitForSeconds(CardStackDelay);
		for (int i = 0; i < fullLength; ++i)
		{
			if (i % 2 == 0)
			{
				_stackCardSlot.AddCard(_rightHandCardSlot.TopCard());
			}
			else
			{
				_stackCardSlot.AddCard(_leftHandCardSlot.TopCard());
			}
			yield return new WaitForSeconds(CardStackDelay);
		}
		//MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);	
		yield return new WaitForSeconds(.01f);

		DealInProgress--;
    }

	public IEnumerator CutDeckCoroutine()
	{
		DealInProgress++;
		int halfLength = _cardDeck.CardList.Count / 2;
		int thirdLength = _cardDeck.CardList.Count / 3;
		int randomLength = Random.Range(thirdLength, halfLength);

		for (int i = 0; i < randomLength; ++i)
		{
			_leftHandCardSlot.AddCard(_stackCardSlot.TopCard());
		}
		yield return new WaitForSeconds(.5f);	
		for (int i = 0; i < (_cardDeck.CardList.Count - randomLength); ++i)
		{
			_rightHandCardSlot.AddCard(_stackCardSlot.TopCard());
		}

		yield return new WaitForSeconds(.1f);
		for (int i = 0; i < randomLength; ++i)
		{
			_stackCardSlot.AddCard(_leftHandCardSlot.TopCard());
			yield return new WaitForSeconds(CardStackDelay);
		}
		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < (_cardDeck.CardList.Count - randomLength); ++i)
		{
			_stackCardSlot.AddCard(_rightHandCardSlot.TopCard());
			yield return new WaitForSeconds(CardStackDelay);
		}
		yield return new WaitForSeconds(.01f);
//		MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);	
		yield return new WaitForSeconds(.01f);
		DealInProgress--;
	}




	void cheveronReset(){
		cardCheveron0Lock = false;
		cardCheveron1Lock = false;
		cardCheveron2Lock = false;


		FlipCardSlotDown (_prior0CardSlot);
		FlipCardSlotDown (_prior1CardSlot);
		FlipCardSlotDown (_prior2CardSlot);
	}

	bool cardCheveron0Lock,cardCheveron1Lock,cardCheveron2Lock = false;

	bool cardCheveronCurrent = false;

	public IEnumerator DrawCoroutine()
	{
		DealInProgress++;

		//		if (_discardHoverStackCardSlot.AddCard(_prior6CardSlot.TopCard()))
		//		{	
		//			yield return new WaitForSeconds(CardStackDelay);	
		//		}	
		//		if (_discardStackCardSlot.AddCard(_discardHoverStackCardSlot.TopCard()))
		//		{
		//			yield return new WaitForSeconds(CardStackDelay);
		//		}


		if (!cardCheveron2Lock && !cardCheveronCurrent) {
				_currentCardSlot.AddCard (_stackCardSlot.TopCard ());
				cardCheveronCurrent = true;
//			}
		}

		if(!cardCheveron0Lock && cardCheveronCurrent)
			if (_prior0CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron0Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);		
		}
		if(cardCheveron0Lock && !cardCheveron1Lock && cardCheveronCurrent)
			if (_prior1CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron1Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);	
			}
		if(cardCheveron1Lock && !cardCheveron2Lock && cardCheveronCurrent)
			if (_prior2CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron2Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		//		int collectiveFaceValue = _prior0CardSlot.FaceValue();
		//		collectiveFaceValue += _prior1CardSlot.FaceValue();
		//		collectiveFaceValue += _prior2CardSlot.FaceValue();
		//		collectiveFaceValue += _prior3CardSlot.FaceValue();
		//		collectiveFaceValue += _prior4CardSlot.FaceValue();
		//		collectiveFaceValue += _prior5CardSlot.FaceValue();
		//		collectiveFaceValue += _prior6CardSlot.FaceValue();
		//		collectiveFaceValue += _currentCardSlot.FaceValue();	
		Tarot3CardsDealerUIInstance.FaceValueText.text = _currentCardSlot.FaceValue();
		DealInProgress--;
	}

	void FlipCardSlotUp(CardSlot mCardSlot){

		float y = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.y;
		float z = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.z;
		Quaternion rot = transform.localRotation;
		rot.eulerAngles = new Vector3 (90f, y, z);
		Transform tTemp = mCardSlot.GetComponent<Transform> ();
		tTemp.rotation = rot;
		mCardSlot.TopCard ().TargetTransform.rotation = rot;
	}

	void FlipCardSlotDown(CardSlot mCardSlot){

		float y = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.y;
		float z = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.z;
		Quaternion rot = transform.localRotation;
		rot.eulerAngles = new Vector3 (270f, y, z);
		Transform tTemp = mCardSlot.GetComponent<Transform> ();
		tTemp.rotation = rot;
	}


	public IEnumerator DisplayCoroutine(CardSlot mCardSlot)
	{
		DealInProgress++;
			cardCheveronCurrent = true;

		if (_currentCardSlot.CardList.Count == 0) {
							
			if (mCardSlot == _prior0CardSlot) {

				float x = _prior0CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					Debug.Log ("x = " + x);
					FlipCardSlotUp (_prior0CardSlot);
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron0Lock = false;
				}
			}
			if (mCardSlot == _prior1CardSlot) {
				float x = _prior1CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f)
					FlipCardSlotUp (_prior1CardSlot);
				else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron1Lock = false;	
				}
			}
			if (mCardSlot == _prior2CardSlot) {
				float x = _prior2CardSlot.GetComponent<Transform> ().rotation.eulerAngles.x;
				if (x == 270f)
					FlipCardSlotUp (_prior2CardSlot);
				else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron2Lock = false;	
				}
			}
			
			yield return new WaitForSeconds (CardStackDelay);
		}
			//		int collectiveFaceValue = _prior0CardSlot.FaceValue();
			//		collectiveFaceValue += _prior1CardSlot.FaceValue();
			//		collectiveFaceValue += _prior2CardSlot.FaceValue();
			//		collectiveFaceValue += _prior3CardSlot.FaceValue();
			//		collectiveFaceValue += _prior4CardSlot.FaceValue();
			//		collectiveFaceValue += _prior5CardSlot.FaceValue();
			//		collectiveFaceValue += _prior6CardSlot.FaceValue();
			//		collectiveFaceValue += _currentCardSlot.FaceValue();	
			Tarot3CardsDealerUIInstance.FaceValueText.text = _currentCardSlot.FaceValue();
		DealInProgress--;
	}


	public void DrawDisplay(CardSlot mCardSlot){

		if (DealInProgress == 0)
		{
			if (mCardSlot == _prior0CardSlot ||
			    mCardSlot == _prior1CardSlot ||
			    mCardSlot == _prior2CardSlot ) {
				print ("DisplayCoroutine");
				StartCoroutine (DisplayCoroutine (mCardSlot));
			} else {
				print ("DrawCoroutine");
				StartCoroutine (DrawCoroutine ());
			}
		}

	}
}
