using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ColorType {
	Primary_1,
	Primary_2,
	Primary_3,
	Primary_4,
	Primary_5,

	Neutral_1,
	Neutral_2,
	Neutral_3,
	Neutral_4,
	Neutral_5,

	Accent_1,
	Accent_2,
	Accent_3,
	Accent_4,
	Accent_5
};

[ExecuteInEditMode]
public class UIColor : MonoBehaviour
{
	public ColorType colorType;
    public UIColorManager colorManager;

    public void Validate(Color newColor) 
	{
		// Image valid check
		Image tempImage = GetComponent<Image>();
		if(tempImage != null) {
			tempImage.color = newColor;
		}

		// SpriteRenderer valid check
		SpriteRenderer tempSprite = GetComponent<SpriteRenderer>();
		if(tempSprite != null) {
			tempSprite.color = newColor;
		}

		// Text valid check
		Text tempText = GetComponent<Text>();
		if(tempText != null) {
			tempText.color = newColor;
		}
	}

    private void OnValidate() {
        if(colorManager != null)
            colorManager.Start();
    }

}
