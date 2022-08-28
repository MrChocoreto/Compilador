using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexical_Analyzer : MonoBehaviour
{
    [SerializeField, TextArea] private string Text = default;
    private int ASCII = default;


    [ContextMenu("Lexical_Analyzer")]
    public void Analyzer()
    {
        for (int i = 0; i < Text.Length; i++)
        {
            ASCII = Text[i];
            if (ASCII >= 65 && ASCII <= 90)
            {
                Debug.Log(Text[i] +" es una Mayuscula");
            }
            else if (ASCII >= 97 && ASCII <= 122)
            {
                Debug.Log(Text[i] + " es una Minuscula");
            }
        }
    }


}
