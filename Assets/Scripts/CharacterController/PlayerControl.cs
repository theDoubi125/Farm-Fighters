using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    private CharacterController2D m_character;

    private void Start()
    {
        m_character = GetComponent<CharacterController2D>();
    }

	// Update is called once per frame
	private void Update ()
    {
        Vector2 input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

        if (input.sqrMagnitude > 1)
            input.Normalize();

        m_character.Move(input * m_character.MaxSpeed);
	}
}
