using System.Collections;
using System.Collections.Generic;
using Core.GameData;
using Core.Managers;
using UnityEngine;

public class tester : MonoBehaviour
{
    // Start is called before the first frame update
    private int i = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            CoreManager.instance.StyleManager.ApplyStyle(Item.PastelStyle);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            i++;
            CoreManager.instance.ColorsManager.ChangeColorTheme(i%2==1 ? ColorThemeType.Mystic : ColorThemeType.Default);
            CoreManager.instance.SaveManager.Save(new ColorSaver(i%2==1 ?ColorThemeType.Mystic.ToString() : ColorThemeType.Default.ToString()));
        }

        if (Input.GetKey(KeyCode.D))
        {
            CoreManager.instance.SaveManager.ClearAllData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CoreManager.instance.BuffManager.AddBuff(new Vector3(0,0,0), new Color(1.000f, 0.000f, 0.000f, 1.000f), 5f);
            CoreManager.instance.BuffManager.AddBuff(new Vector3(0,0,0), new Color(0.000f, 0.342f, 1.000f, 1.000f), 5f);
            CoreManager.instance.BuffManager.AddBuff(new Vector3(0,0,0), new Color(0.968f, 1.000f, 0.000f, 1.000f), 5f);


        }
        
    }
    
}
