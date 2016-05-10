using UnityEngine;
using System.Collections;

public class CropRenderer : MonoBehaviour {

    private SpriteRenderer m_renderer;

    [SerializeField]
    private Sprite[] Stages;

	// Use this for initialization
	void Start () {
        m_renderer = GetComponent<SpriteRenderer>();
        if (Stages.Length == 0)
        {
            Debug.Log("<CropRenderer> You need to put at least one sprite in the renderer.");
            enabled = false;
            return;
        }

        m_renderer.sprite = Stages[0];

        Crop crop = GetComponent<Crop>();
        if(crop.GetStageCount() != Stages.Length)
            Debug.Log("<CropRenderer> Not enough sprites to show all crops stages.");

        crop.StageChanged += OnStageChanged;
	}

    private void OnStageChanged(object _sender, int _stage)
    {
        if(Stages.Length > 0)
        {
            m_renderer.sprite = Stages[Mathf.Min(_stage, Stages.Length - 1)];
        }
    }
}
