using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	// Constants defining the grid size and card spacing
	public const int gridRows = 2;
	public const int gridCols = 4;
	public const float offsetX = 2f;
	public const float offsetY = 2.5f;

	// Variables to track the currently revealed cards and the player's score
	private MemoryCard _firstRevealed;
	private MemoryCard _secondRevealed;
	private int _score = 0;

	// Reference to the original card prefab and an array of card images
	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	
	[SerializeField] private TextMeshPro scoreLabel;

	// Start is called before the first frame update
	void Start()
	{
		// Get the initial position of the original card
		Vector3 startPos = originalCard.transform.position;

		// Array of numbers representing card IDs, shuffled to randomize card positions
		int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3,};
		numbers = ShuffleArray(numbers);

		// Nested loops to instantiate and position cards in a grid
		for (int i = 0; i < gridCols; i++)
		{
			for (int j = 0; j < gridRows; j++)
			{
				MemoryCard card;

				// If it's the first card, use the original card, otherwise instantiate a new one
				if (i == 0 && j == 0)
				{
					card = originalCard;
				}
				else
				{
					card = Instantiate(originalCard) as MemoryCard;
				}

				// Calculate the index and ID for the card
				int index = j * gridCols + i;
				int id = numbers[index];

				// Set the card's ID and image
				card.SetCard(id, images[id]);

				// Calculate and set the position of the card
				float posX = (offsetX * i) + startPos.x;
				float posY = -(offsetY * j) + startPos.y;
				card.transform.position = new Vector3(posX, posY, startPos.z);
			}
		}
	}

	// Method to shuffle an array using the Fisher-Yates algorithm
	private int[] ShuffleArray(int[] numbers)
	{
		int[] newArray = numbers.Clone() as int[];
		for (int i = 0; i < newArray.Length; i++)
		{
			int temp = newArray[i];
			int r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = temp;
		}
		return newArray;
	}

	// Property to check if a new card can be revealed
	public bool canReveal
	{
		get { return _secondRevealed == null; }
	}

	// Method called when a card is revealed
	public void CardRevealed(MemoryCard card)
	{
		if (_firstRevealed == null)
		{
			// If it's the first card revealed, store it in _firstRevealed
			_firstRevealed = card;
		}
		else
		{
			// If it's the second card revealed, store it in _secondRevealed and check for a match
			_secondRevealed = card;
			StartCoroutine(CheckMatch());
		}
	}

	// Coroutine to check if the two revealed cards match
	private IEnumerator CheckMatch()
	{
		if (_firstRevealed.id == _secondRevealed.id)
		{
			// If the cards match, increment the score
			_score++;
			scoreLabel.text = "Score: " + _score;
		}
		else
		{
			// If the cards don't match, wait for a short time and then unreveal them
			yield return new WaitForSeconds(.5f);
			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();
		}

		// Reset the revealed cards
		_firstRevealed = null;
		_secondRevealed = null;
	}

    [System.Obsolete]
    public void Restart(){
		Application.LoadLevel("SampleScene");
	}
}
