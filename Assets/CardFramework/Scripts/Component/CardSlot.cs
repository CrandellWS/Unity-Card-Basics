using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSlot : MonoBehaviour 
{
	public readonly List<Card> CardList = new List<Card>();

	[SerializeField]
	private bool _inverseStack;

	[Range(0.05f, 0.3f)]
	[SerializeField]
	private float _positionDamp = .05f;

	[Range(0.05f, 0.3f)]
	[SerializeField] 
	private float _rotationDamp = .05f;   
	
	private void Awake()
	{
		GetComponent<MeshRenderer>().enabled = false;
	}

	public string FaceValue()
	{
		if(TopCard() != null)
			return TopCard().FaceValue;
		else {
			return " ";
		}
	}
    
	public Card TopCard()
	{
		if (CardList.Count > 0)
		{
			return CardList[ CardList.Count - 1 ];
		}
		else
		{
			return null;
		}	
	}
    
	public Card BottomCard()
	{
		if (CardList.Count > 0)
		{
			return CardList[ 0 ];
		}
		else
		{
			return null;
		}			
	}
	
	public bool AddCard(Card card)
	{
		if (card != null)
		{
			if (card.ParentCardSlot != null)
			{
				card.ParentCardSlot.RemoveCard(card);
			}
			card.ParentCardSlot = this;
			CardList.Add(card);
			card.TargetTransform.rotation = transform.rotation;
			card.TargetTransform.Rotate(card.TargetTransform.forward  * Time.deltaTime, Random.Range(-.4f, .4f), Space.Self);
			float cardHeight = card.GetComponent<BoxCollider>().size.z;
			card.TargetTransform.position = transform.position;
			if (_inverseStack)
			{
				card.TargetTransform.Translate(new Vector3(0, 0, CardList.Count * (float)cardHeight) * -1f, Space.Self);
			}
			else
			{
				card.TargetTransform.Translate(new Vector3(0, 0, CardList.Count * (float)cardHeight), Space.Self);
			}
			card.SetDamp(_positionDamp, _rotationDamp);
			return true;
		}
		else
		{
			return false;
		}
	}

	public void RemoveCard(Card card)
	{
		card.ParentCardSlot = null;
		CardList.Remove(card);
	}
}
