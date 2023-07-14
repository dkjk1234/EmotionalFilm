using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Image staminaFill;
    public Text staminaPercentText;

    public List<GameObject> selectWeaponImage;

    public List<GameObject> aim;

    public Slider bossHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �ð�ȭ
        switch (GameManager.Instance.weapon.weapon)
        {
            case WeaponScript.Weapon.Spray:
                for (int i = 0; i < selectWeaponImage.Count; i++)
                    selectWeaponImage[i].SetActive(false);
                selectWeaponImage[0].SetActive(true);
                break;

            case WeaponScript.Weapon.Brush:
                for (int i = 0; i < selectWeaponImage.Count; i++)
                    selectWeaponImage[i].SetActive(false);
                selectWeaponImage[1].SetActive(true);
                break;

            case WeaponScript.Weapon.PaintGun:
                for (int i = 0; i < selectWeaponImage.Count; i++)
                    selectWeaponImage[i].SetActive(false);
                selectWeaponImage[2].SetActive(true);
                break;

            case WeaponScript.Weapon.WaterBalloon:
                for (int i = 0; i < selectWeaponImage.Count; i++)
                    selectWeaponImage[i].SetActive(false);
                selectWeaponImage[3].SetActive(true);
                break;
        }

        // ���׹̳� �ؽ�Ʈ
        int percentPaintValue = (int)GameManager.Instance.weapon.paintValue;
        staminaPercentText.text = percentPaintValue.ToString() + "%";

        // ���׹̳� ��
        staminaFill.rectTransform.sizeDelta = new Vector2(840, 70 + GameManager.Instance.weapon.paintValue * 7);
    }
}
