using UnityEngine;
using System.Collections;
using System.IO;

public class TarotDealer : MonoBehaviour 
{
	public TarotDealerUI TarotDealerUIInstance { get; set; }
    
	[SerializeField]
	private CardDeck _cardDeck;	

	[SerializeField]
	private CardSlot _pickupCardSlot;		

	[SerializeField]
	private CardSlot _stackCardSlot;		

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

	[SerializeField]
	public CardSlot _prior3CardSlot;

	[SerializeField]
	public CardSlot _prior4CardSlot;

	[SerializeField]
	public CardSlot _prior5CardSlot;

	[SerializeField]
	public CardSlot _prior6CardSlot;

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
		cheveronReset ();
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

		TarotDealerUIInstance.FaceValueText.text = " ";
		MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);
		cardCheveronCurrent = false;
		MoveCardSlotToCardSlot(_rightHandCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_leftHandCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior0CardSlot, _pickupCardSlot);
		if(_prior1CardSlot != null)
			MoveCardSlotToCardSlot(_prior1CardSlot, _pickupCardSlot);
		if(_prior2CardSlot != null)
			MoveCardSlotToCardSlot(_prior2CardSlot, _pickupCardSlot);
		if(_prior3CardSlot != null)
			MoveCardSlotToCardSlot(_prior3CardSlot, _pickupCardSlot);
		if(_prior4CardSlot != null)
			MoveCardSlotToCardSlot(_prior4CardSlot, _pickupCardSlot);
		if(_prior5CardSlot != null)
			MoveCardSlotToCardSlot(_prior5CardSlot, _pickupCardSlot);
		if(_prior6CardSlot != null)
			MoveCardSlotToCardSlot(_prior6CardSlot, _pickupCardSlot);
		//MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_pickupCardSlot, _stackCardSlot);

		cheveronReset();		
		yield return new WaitForSeconds(.1f);	

		DealInProgress--;
	}

	public IEnumerator GatherOthersCoroutine()
	{
		DealInProgress++;
		TarotDealerUIInstance.FaceValueText.text = " ";
		MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);
		cardCheveronCurrent = false;
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


		FlipCardSlotDown (_prior0CardSlot);
		cardCheveron0Lock = false;
		if (_prior1CardSlot != null) {
			FlipCardSlotDown (_prior1CardSlot);
			cardCheveron1Lock = false;
		} else {
			cardCheveron1Lock = true;
		}
		if (_prior2CardSlot != null) {
			FlipCardSlotDown (_prior2CardSlot);
			cardCheveron2Lock = false;
		} else {
			cardCheveron2Lock = true;
		}
		if (_prior3CardSlot != null) {
			FlipCardSlotDown (_prior3CardSlot);
			cardCheveron3Lock = false;
		} else {
			cardCheveron3Lock = true;
		}
		if (_prior4CardSlot != null) {
			FlipCardSlotDown (_prior4CardSlot);
			cardCheveron4Lock = false;
		} else {
			cardCheveron4Lock = true;
		}
		if (_prior5CardSlot != null) {
			FlipCardSlotDown (_prior5CardSlot);
			cardCheveron5Lock = false;
		} else {
			cardCheveron5Lock = true;
		}
		if (_prior6CardSlot != null) {
			FlipCardSlotDown (_prior6CardSlot);
			cardCheveron6Lock = false;
		} else {
			cardCheveron6Lock = true;
		}
	}

	bool cardCheveron0Lock,cardCheveron1Lock,cardCheveron2Lock,
	cardCheveron3Lock,cardCheveron4Lock,cardCheveron5Lock,cardCheveron6Lock = false;

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


		if (!cardCheveronCurrent && (
			!cardCheveron0Lock ||
			!cardCheveron1Lock ||
			!cardCheveron2Lock ||
			!cardCheveron3Lock ||
			!cardCheveron4Lock ||
			!cardCheveron5Lock ||
			!cardCheveron6Lock
		)) {
				_currentCardSlot.AddCard (_stackCardSlot.TopCard ());
				cardCheveronCurrent = true;
//			}
		}

		if(!cardCheveron0Lock && cardCheveronCurrent && _prior0CardSlot != null)
			if (_prior0CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron0Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);		
		}
		if(cardCheveron0Lock && !cardCheveron1Lock && cardCheveronCurrent && _prior1CardSlot != null)
			if (_prior1CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron1Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);	
			}
		if(cardCheveron1Lock && !cardCheveron2Lock && cardCheveronCurrent && _prior2CardSlot != null)
			if (_prior2CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron2Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		if(cardCheveron2Lock && !cardCheveron3Lock && cardCheveronCurrent && _prior3CardSlot != null)
			if (_prior3CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron3Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		if(cardCheveron3Lock && !cardCheveron4Lock && cardCheveronCurrent && _prior4CardSlot != null)
			if (_prior4CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron4Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		if(cardCheveron4Lock && !cardCheveron5Lock && cardCheveronCurrent && _prior5CardSlot != null)
			if (_prior5CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron5Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
		}
		if(cardCheveron5Lock && !cardCheveron6Lock && cardCheveronCurrent && _prior6CardSlot != null)
			if (_prior6CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron6Lock = true;
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
		TarotDealerUIInstance.FaceValueText.text = _currentCardSlot.FaceValue();
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
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron0Lock = false;
				}
			}
			if (mCardSlot == _prior1CardSlot) {
				float x = _prior1CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior1CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron1Lock = false;	
				}
			}
			if (mCardSlot == _prior2CardSlot) {
				float x = _prior2CardSlot.GetComponent<Transform> ().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior2CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron2Lock = false;	
				}
			}
			if (mCardSlot == _prior3CardSlot) {
				float x = _prior3CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior3CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron3Lock = false;	
				}
			}
			if (mCardSlot == _prior4CardSlot) {
				float x = _prior4CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior4CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron4Lock = false;	
				}
			}
			if (mCardSlot == _prior5CardSlot) {
				float x = _prior5CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior5CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron5Lock = false;
				}
			}
			if (mCardSlot == _prior6CardSlot) {
				float x = _prior6CardSlot.GetComponent<Transform>().rotation.eulerAngles.x;
				if (x == 270f) {
					FlipCardSlotUp (_prior6CardSlot);
					cardCheveronCurrent = false;
				} else {
					_currentCardSlot.AddCard (mCardSlot.TopCard ());
					cardCheveron6Lock = false;	
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
			TarotDealerUIInstance.FaceValueText.text = _currentCardSlot.FaceValue();
		DealInProgress--;
	}


	public void DrawDisplay(CardSlot mCardSlot){

		if (DealInProgress == 0)
		{
			if (mCardSlot == _prior0CardSlot ||
			    mCardSlot == _prior1CardSlot ||
			    mCardSlot == _prior2CardSlot ||
			    mCardSlot == _prior3CardSlot ||
			    mCardSlot == _prior4CardSlot ||
			    mCardSlot == _prior5CardSlot ||
				mCardSlot == _prior6CardSlot) {
				print ("DisplayCoroutine");
				StartCoroutine (DisplayCoroutine (mCardSlot));
			} else {
				print ("DrawCoroutine");
				StartCoroutine (DrawCoroutine ());
			}
		}

	}
}
