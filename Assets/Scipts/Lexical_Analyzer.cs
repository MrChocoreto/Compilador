using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Lexical_Analyzer : MonoBehaviour
{
    [SerializeField, TextArea] private string Text = default;
    private int NumLine = default;

    private void Start()
    {
        SpaceAnalyzer();
    }



    [ContextMenu("Lexical_Analyzer")]
    void SpaceAnalyzer()
    {
        // Inicializo el numero de linea siempre en 1
        NumLine = 1;

        // Creo las variables donde almacenare cada palabra y donde comparare los ASCII
        int ASCII = default;
        string word = default;


        // Recorro cada letra de la frase para poder identificar cada palabra y saber
        // el numero de linea de cada linea
        for (int i = 0; i < Text.Length; i++)
        {
            // Aqui solo le doy el valor ASCII de cada uno de los caracteres
            ASCII = Text[i];

            // Reviso si hay un espacio, un salto de linea o si es el final de
            // la frase
            if (ASCII == 32 || ASCII == 10 || i == Text.Length - 1)
            {

                // si es cierto entonces es por que ya completamos una palabra
                // y por si acaso reviso que en el caso de que sea el final de
                // la frase acomplete la palabra
                if (i == Text.Length - 1)
                {
                    word = word + Text[Text.Length-1];
                }


                // le mando la palabra y el numero de linea al metodo
                WordAnalyzer(word, NumLine);

                // y vuelvo a vaciar al variable que almacena cada frase
                word = default;
            }
            else
            {
                // aqui solo añado una letra cada que recorro los caracteres
                word = word + Text[i];
            }
            if (ASCII == 10)
            {
                // y aqui solo voy aumentando la variable cada que hay un salto de linea
                NumLine++;
            }
        }
    }
    
    /// <summary>
    /// Este metodo analiza palabras
    /// </summary>
    /// 
    /// <param name="Word">La palabra que recibe</param>
    /// <param name="Line">linea en la que esta la palabra</param>
    void WordAnalyzer(string Word, int Line)
    {
        bool ResetWord = false;
        bool SignalDected = false;
        //int ASCII = default;
        string NewWord = default;
        string Signal = default;
        // Recorro cada caracter de la palabra
        for (int i = 0; i < Word.Length; i++)
        {
            // corroboro que el primer caracter de la palabra sea una letra o un guion bajo
            if (Word[i] == 95 || Word[i] >= 65 && Word[i] <= 90 || Word[i] >= 97 && Word[i] <= 122 || Word[i] >= 47 && Word[i] <= 58)
            {
                NewWord = NewWord + Word[i];
            }
            else
            {
                SignalDected = true;
                if (NewWord != null)
                {
                    Debug.Log(NewWord);
                }
                if (Word.Length-1 > 1)
                {
                    if (i < Word.Length - 1)
                    {
                        if (SignalAnalyzer(Word[i], Word[i + 1], Line))
                        {
                            NewWord = default;
                            i += 2;
                        }
                        else
                        {
                            NewWord = default;
                            goto Exit;
                        }
                    }
                    else
                    {
                        SignalAnalyzer(Word[i], ' ', Line);
                    }
                }
                else if (Word.Length < 1 || i == Word.Length+1)
                {
                   SignalAnalyzer(Word[i], ' ', Line);
                }
                
                //Debug.Log(Word[i] + " es un simbolo, num. linea " + Line);
            }


            Exit:;
        }


        if (NewWord != null && !SignalDected)
        {
            Debug.Log(NewWord);
        }
    }


    bool SignalAnalyzer(char Signal_1, char Signal_2, int line)
    {
        bool IsDouble = false;
        switch (Signal_1)
        {
            case '+':
                if (Signal_2 == '+')
                {
                    Debug.Log(Signal_1 + "" + Signal_2 + " es un incremento, num. linea " + line);
                    IsDouble = true;
                }
                else
                {
                    Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                }
                break;

            case '=': Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                IsDouble = false;
                break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            //case:
            //    break;

            default: IsDouble = false; Debug.Log(Signal_1 + " soy un simbolo, num. linea " + line);
                break;
        }
        return IsDouble;
    }
}
