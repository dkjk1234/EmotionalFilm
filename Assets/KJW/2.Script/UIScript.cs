using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Image staminaFill;
    public Text staminaPercentText;

    public Image hpFill;
    public Text hpPercentText;

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
        // 선택 무기 시각화
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

        // 스테미나 텍스트
        int percentPaintValue = (int)GameManager.Instance.weapon.paintValue;
        staminaPercentText.text = percentPaintValue.ToString() + "%";

        // 스테미나 양
        staminaFill.rectTransform.sizeDelta = new Vector2(840, 70 + GameManager.Instance.weapon.paintValue * 7);

        // 체력 텍스트
        int hpPaintValue = (int)GameManager.Instance.playerHealth;
        hpPercentText.text = hpPaintValue.ToString() + "%";

        // 체력 양
        hpFill.rectTransform.sizeDelta = new Vector2(840, 70 + GameManager.Instance.playerHealth * 7);
    }
}
