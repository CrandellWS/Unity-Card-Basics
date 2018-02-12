using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TarotBasicDealerUI : MonoBehaviour 
{
	private TarotBasicDealer _dealer;

	public Text FaceValueText { get { return _faceValueText; } }
	[SerializeField]
	private Text _faceValueText;

	private void Awake()
	{
		_dealer = GameObject.Find("Dealer").GetComponent<TarotBasicDealer>();
		_dealer.TarotBasicDealerUIInstance = this;
	}

	public void Shuffle()
	{
		if (_dealer.DealInProgress == 0)
		{
			StartCoroutine(_dealer.ShuffleCoroutine());
		}
	}

	public void CutDeck()
	{
		if (_dealer.DealInProgress == 0)
		{
			StartCoroutine(_dealer.CutDeckCoroutine());
		}
	}
	
	public void Draw()
	{
		if (_dealer.DealInProgress == 0)
		{
			StartCoroutine(_dealer.DrawCoroutine());
		}
	}

	public void DrawDisplay(CardSlot mCardSlot){

		if (_dealer.DealInProgress == 0)
		{
			if (mCardSlot == _dealer._prior0CardSlot ||
				mCardSlot == _dealer._prior1CardSlot ||
				mCardSlot == _dealer._prior2CardSlot ||
				mCardSlot == _dealer._prior3CardSlot ||
				mCardSlot == _dealer._prior4CardSlot ||
				mCardSlot == _dealer._prior5CardSlot ||
				mCardSlot == _dealer._prior6CardSlot )
					StartCoroutine(_dealer.DisplayCoroutine(mCardSlot));
			else
				Draw ();
		}

	}

	void Update()
	{

		// Code for OnMouseDown in the iPhone. Unquote to test.
		RaycastHit hit = new RaycastHit();
		if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase.Equals (TouchPhase.Began)) {
					// Construct a ray from the current touch coordinates
					Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (i).position);
					if (Physics.Raycast (ray, out hit)) {
						Debug.Log ("Something Hit =" + hit.collider.name );
						DrawDisplay ((hit.collider.GetComponentInParent<Card>()).ParentCardSlot);
					}
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if(Input.GetMouseButton(0)) {
					if(Physics.Raycast(ray, out hit)) {
						print(hit.collider.name);
						Debug.Log ("Something Hit =" + hit.collider.name);
						Debug.Log ("CardSlot =" + (hit.collider.GetComponentInParent<Card>()).ParentCardSlot);
						DrawDisplay ((hit.collider.GetComponentInParent<Card>()).ParentCardSlot);
					}
				}
			}
		}

	}
}
