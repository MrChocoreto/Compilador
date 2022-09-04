using System;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Lexical_Analyzer : MonoBehaviour
{

    #region Variables

    [SerializeField, TextArea] private string Text = default;
    private int NumLine = default;

    #endregion


    #region UnityMethods

    private void Start()
    {
        SpaceAnalyzer();
    }

    #endregion


    #region SpaceAnalizer


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
                    WordAnalyzer(word, NumLine);
                }
                else if(word == null && ASCII == 10)
                {
                    // en caso de que haya un enter y no se haya generado una palabra
                    // es porque solo se ha hecho un salto de linea
                    NumLine++;
                    Debug.Log("Hay solamente un enter");
                }
                else
                {
                    // le mando la palabra y el numero de linea al metodo
                    WordAnalyzer(word, NumLine);

                    // y vuelvo a vaciar al variable que almacena cada frase
                    word = default;

                }


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


    #endregion


    #region WordAnalizer


    void WordAnalyzer(string Word, int Line)
    {
        bool SignalDected = false;
        //int ASCII = default;
        string NewWord = default;
        // Recorro cada caracter de la palabra
        for (int i = 0; i < Word.Length; i++)
        {
            // corroboro que el primer caracter de la palabra sea una letra o un guion bajo
            if (Word[i] == 95 || Word[i] >= 65 && Word[i] <= 90 || Word[i] >= 97 && Word[i] <= 122 || i > 0 && Word[i] >= 48 && Word[i] <= 57)
            {
                SignalDected = false;
                NewWord = NewWord + Word[i];
            }
            else if(Word[i] >= 48 && Word[i] <= 57)
            {
                SignalDected = false;
                i += NumA(Line, Word);
            }
            else
            {
                SignalDected = true;
                // en caso de que se trate de que lo que se escriba sea un numero es porque 
                if (NewWord != null)
                {
                    if (NewWord[0] >= 48 && NewWord[0] <= 57)
                    {
                        i += NumA(Line, NewWord);
                    }
                    else
                    {
                        Debug.Log(NewWord);
                    }
                }
                if (Word.Length > 1)
                {
                    if (i < Word.Length - 1)
                    {
                        if (SigA(Word[i], Word[i + 1], Line))
                        {
                            NewWord = default;
                            i += 1;
                        }
                        else
                        {
                            NewWord = default;
                            goto Exit;
                        }
                    }
                    else
                    {
                        SigA(Word[i], ' ', Line);
                        NewWord = default;
                        goto Exit;
                    }
                }
                else if (Word.Length-1 < 1 || i == Word.Length+1)
                {
                    SigA(Word[i], ' ', Line);
                    NewWord = default;
                    goto Exit;
                }
                
            }


            Exit:;
            
        }

        if (NewWord != null && !SignalDected || NewWord != null && SignalDected)
        {
            Debug.Log(NewWord);
        }

    }


    #endregion


    #region SNA

    /// <summary>
    /// SNA means Singal and Numbers Analizer
    /// </summary>
    /// <param name="Signal_1"></param>
    /// <param name="Signal_2"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    bool SigA(char Signal_1, char Signal_2, int line)
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
                    IsDouble = false;
                }
                break;

            case '-':
                if (Signal_2 == '-')
                {
                    Debug.Log(Signal_1 + "" + Signal_2 + " es un decremento, num. linea " + line);
                    IsDouble = true;
                }
                else
                {
                    Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                    IsDouble = false;
                }
                break;


            case '=': Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                IsDouble = false;
                break;

            case '{': Debug.Log(Signal_1 + " es una llave abierta, num. linea " + line);
                IsDouble = false;
                break;

            case '}': Debug.Log(Signal_1 + " es una llave cerrada, num. linea " + line);
                IsDouble = false;
                break;

            case '[':
                Debug.Log(Signal_1 + " es una llave cerrada, num. linea " + line);
                IsDouble = false;
                break;

            case ']':
                Debug.Log(Signal_1 + " es una llave cerrada, num. linea " + line);
                IsDouble = false;
                break;

            case '/':
                if (Signal_2 == '/')
                {
                    Debug.Log(Signal_1 + "" + Signal_2 + " es un comentario simple, num. linea " + line);
                    IsDouble = true;
                }
                else if (Signal_2 == '*')
                {
                    Debug.Log(Signal_1 + "" + Signal_2 + " es un comentario largo, num. linea " + line);
                    IsDouble = true;
                }
                else
                {
                    Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                    IsDouble = false;
                }
                break;
                
            case '*':
                if (Signal_2 == '/')
                {
                    Debug.Log(Signal_1 + "" + Signal_2 + " es un comentario largo, num. linea " + line);
                    IsDouble = true;
                }
                else
                {
                    Debug.Log(Signal_1 + " es un operador, num. linea " + line);
                    IsDouble = false;
                }
                break;

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


    #endregion



    int NumA(int line, string word)
    {
        string num = default;
        int ASCII = default;
        int ASCII_Next = default;
        int Plus = default;
        bool ForceExit = false;


        for (int i = 0; i < word.Length; i++)
        {
            ASCII = word[i];
            if (ASCII >= 48 && ASCII <= 57 && i < word.Length)
            {
                num = num + word[i];
            }
            else
            {
                if (i < word.Length-1 && ASCII == 46 && word[i + 1] >= 48 && word[i + 1] <= 57)
                {
                    num = num + word[i];
                }
                else if(i < word.Length - 1 && ASCII == 46 && word[i + 1] >= 48 && word[i + 1] <= 57
                    || i < word.Length - 1 && ASCII == 46 && word[i + 1] <= 46 && word[i + 1] >= 32
                    || i < word.Length - 1 && ASCII == 46 && word[i + 1] >= 65 && word[i + 1] <= 90
                    || i < word.Length - 1 && ASCII == 46 && word[i + 1] >= 65 && word[i + 1] <= 90
                    || i < word.Length - 1 && ASCII == 46 && word[i + 1] >= 97 && word[i + 1] <= 122)
                {
                    Debug.LogError("No esta bien escrito el numero");
                    ForceExit = true;
                    goto Exit;
                }
            }
            
        }

        for (int j = 0; j < num.Length; j++)
        {
            ASCII = num[j];
            if (j < num.Length-1)
            {
                ASCII_Next = num[j + 1];
            }

            if (ASCII == 46 && ASCII_Next >= 48 && ASCII_Next <= 57 && j <= num.Length)
            {
                Debug.Log(num + " es un numero decimal");
                ForceExit = false;
                goto Exit;
            }
            else if(j == num.Length-1)
            {
                Debug.Log(num + " es un numero entero");
                ForceExit = false;
                goto Exit;
            }
        }


        Exit:
        if (ForceExit)
        {
            Plus = num.Length;
        }
        else
        {
            Plus = num.Length - 1;
        }
        return Plus;
    }



}
