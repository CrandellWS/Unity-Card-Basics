using UnityEngine;
using System.Collections;
using System.IO;

public class TarotBasicDealer : MonoBehaviour 
{
	public TarotBasicDealerUI TarotBasicDealerUIInstance { get; set; }
    
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
	private CardSlot _currentCardSlot;	

	[SerializeField]
	private CardSlot _prior0CardSlot;	

	[SerializeField]
	private CardSlot _prior1CardSlot;	

	[SerializeField]
	private CardSlot _prior2CardSlot;		

	[SerializeField]
	private CardSlot _prior3CardSlot;

	[SerializeField]
	private CardSlot _prior4CardSlot;

	[SerializeField]
	private CardSlot _prior5CardSlot;

	[SerializeField]
	private CardSlot _prior6CardSlot;

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
    
    /// <summary>
    /// Shuffle Coroutine.
    /// Moves all card to pickupCardSlot. Then shuffles them back
	/// to cardStackSlot.
    /// </summary>
	public IEnumerator ShuffleCoroutine()
	{
		DealInProgress++;
		TarotBasicDealerUIInstance.FaceValueText.text = " ";
		MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);		
		MoveCardSlotToCardSlot(_prior0CardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior1CardSlot, _pickupCardSlot);	
		MoveCardSlotToCardSlot(_prior2CardSlot, _pickupCardSlot);		
		MoveCardSlotToCardSlot(_prior3CardSlot, _pickupCardSlot);	
		MoveCardSlotToCardSlot(_prior4CardSlot, _pickupCardSlot);	
		MoveCardSlotToCardSlot(_prior5CardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_prior6CardSlot, _pickupCardSlot);	
		MoveCardSlotToCardSlot(_discardStackCardSlot, _pickupCardSlot);
		MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);			
		yield return new WaitForSeconds(.01f);	
		int halfLength = _cardDeck.CardList.Count / 2;

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
		for (int i = 0; i < _cardDeck.CardList.Count; ++i)
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

		cheveronReset();

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
		DealInProgress--;
	}




	void cheveronReset(){
		cardCheveron0Lock = false;
		cardCheveron1Lock = false;
		cardCheveron2Lock = false;
		cardCheveron3Lock = false;
		cardCheveron4Lock = false;
		cardCheveron5Lock = false;
		cardCheveron6Lock = false;
	}

	bool cardCheveron0Lock,cardCheveron1Lock,cardCheveron2Lock,
	cardCheveron3Lock,cardCheveron4Lock,cardCheveron5Lock,cardCheveron6Lock = false;

	bool cardCheveronCurrent = false;
	bool cardCheveronPlacing = false;

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

		if(cardCheveron5Lock && !cardCheveron6Lock && cardCheveronCurrent)
			if (_prior6CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron6Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}

		if(cardCheveron4Lock && !cardCheveron5Lock && cardCheveronCurrent)
			if (_prior5CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron5Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		if(cardCheveron3Lock && !cardCheveron4Lock && cardCheveronCurrent)
			if (_prior4CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron4Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);
			}
		if(cardCheveron2Lock && !cardCheveron3Lock && cardCheveronCurrent)
			if (_prior3CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron3Lock = true;
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
		if(cardCheveron0Lock && !cardCheveron1Lock && cardCheveronCurrent)
			if (_prior1CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron1Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);	
			}
		if(!cardCheveron0Lock && cardCheveronCurrent)
			if (_prior0CardSlot.AddCard(_currentCardSlot.TopCard()))
			{
				cardCheveron0Lock = true;
				cardCheveronCurrent = false;
				yield return new WaitForSeconds(CardStackDelay);		
			}

		if (!cardCheveron6Lock && !cardCheveronCurrent) {
			if (cardCheveronPlacing) {
				cardCheveronPlacing = false;
			} else {
				_currentCardSlot.AddCard (_stackCardSlot.TopCard ());
				cardCheveronCurrent = true;
				cardCheveronPlacing = true;
			}
		}

//		int collectiveFaceValue = _prior0CardSlot.FaceValue();
//		collectiveFaceValue += _prior1CardSlot.FaceValue();
//		collectiveFaceValue += _prior2CardSlot.FaceValue();
//		collectiveFaceValue += _prior3CardSlot.FaceValue();
//		collectiveFaceValue += _prior4CardSlot.FaceValue();
//		collectiveFaceValue += _prior5CardSlot.FaceValue();
//		collectiveFaceValue += _prior6CardSlot.FaceValue();
//		collectiveFaceValue += _currentCardSlot.FaceValue();	
		TarotBasicDealerUIInstance.FaceValueText.text = _currentCardSlot.FaceValue();
		
		DealInProgress--;
	}	
}
