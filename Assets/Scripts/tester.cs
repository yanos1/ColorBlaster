using System.Collections;
using System.Collections.Generic;
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
            CoreManager.instance.StyleManager.ApplyStyle(StyleName.Pastel);
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
            CoreManager.instance.BuffManager.ActivateBuff(new Vector3(0,0,0), new Color(1.000f, 0.000f, 0.000f, 1.000f), 6f);

        }
        
    }
    
}
