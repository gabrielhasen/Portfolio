using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIColorManager : MonoBehaviour 
{
    public List<UIColor> colorElements = new List<UIColor>();

	public Color primary1;
	public Color primary2;
	public Color primary3;
	public Color primary4;
	public Color primary5;

	public Color neutral1;
	public Color neutral2;
	public Color neutral3;
	public Color neutral4;
	public Color neutral5;

	public Color accent1;
	public Color accent2;
	public Color accent3;
	public Color accent4;
	public Color accent5;

    public void Start() {
        setAlpha();
        if (colorElements.Count <= 0) return;

        foreach (UIColor element in colorElements) {
            element.Validate( SetColor(element.colorType) );
        }
    }

    private Color SetColor(ColorType colorType) {
        switch (colorType) {
            case ColorType.Primary_1:
                return primary1;
            case ColorType.Primary_2:
                return primary2;
            case ColorType.Primary_3:
                return primary3;
            case ColorType.Primary_4:
                return primary4;
            case ColorType.Primary_5:
                return primary5;

            case ColorType.Neutral_1:
                return neutral1;
            case ColorType.Neutral_2:
                return neutral2;
            case ColorType.Neutral_3:
                return neutral3;
            case ColorType.Neutral_4:
                return neutral4;
            case ColorType.Neutral_5:
                return neutral5;

            case ColorType.Accent_1:
                return accent1;
            case ColorType.Accent_2:
                return accent2;
            case ColorType.Accent_3:
                return accent3;
            case ColorType.Accent_4:
                return accent4;
            case ColorType.Accent_5:
                return accent5;

            default:
                break;
        }
        return new Color(0, 0, 0);
    }

    private void setAlpha() {
        primary1.a = 1;
        primary2.a = 1;
        primary3.a = 1;
        primary4.a = 1;
        primary5.a = 1;

        neutral1.a = 1;
        neutral2.a = 1;
        neutral3.a = 1;
        neutral4.a = 1;
        neutral5.a = 1;


        accent1.a = 1;
        accent2.a = 1;
        accent3.a = 1;
        accent4.a = 1;
        accent5.a = 1;
    }


    private void OnValidate() {
        Start();
    }
}
