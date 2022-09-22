using System.Collections.Generic;
using UnityEngine;

public class SymbolsTable : MonoBehaviour
{

    #region Variables

    [Header("-----SymbolsTable-----")]
    [SerializeField] List<string> Lexeme;
    [SerializeField] List<string> Token;
    [SerializeField] List<string> Type;
    [SerializeField] List<string> No_Line;
    [SerializeField] List<string> Value;
    [SerializeField] List<string> Desplacement;

    [Space(20)]
    [Header("-----TokenList-----")]
    [SerializeField] List<string> TL_Lexeme;
    [SerializeField] List<string> TL_Token;

    [Space(20)]
    [Header("-----AddElements-----")]
    [SerializeField] Element_SymbolsTable AddElement;
    [SerializeField] GameObject FatherLexeme;
    [SerializeField] GameObject FatherToken;
    [SerializeField] GameObject FatherType;
    [SerializeField] GameObject FatherLine;
    [SerializeField] GameObject FatherValue;
    [SerializeField] GameObject FatherTL_Token;
    [SerializeField] GameObject FatherTL_Lexeme;
    [SerializeField] GameObject FatherDesplacement;
    [SerializeField] RectTransform SymbolsTransform;
    [SerializeField] RectTransform TokensTransform;

    [Space(20)]
    [Header("-----DeleteElements-----")]
    [SerializeField] public List<GameObject> ObjLexeme;
    [SerializeField] public List<GameObject> ObjToken;
    [SerializeField] public List<GameObject> ObjType;
    [SerializeField] public List<GameObject> ObjNo_Line;
    [SerializeField] public List<GameObject> ObjNo_Value;
    [SerializeField] public List<GameObject> ObjNo_Desplacement;
    [SerializeField] public List<GameObject> ObjTL_Lexeme;
    [SerializeField] public List<GameObject> ObjTL_Token;

    #endregion


    private void Awake()
    {
        //Prellenado de la Tabla de simbolos
        string[] newLexeme = Lexeme.ToArray();
        string[] newToken = Token.ToArray();
        string[] newType = Type.ToArray();
        for (int i = 0; i < newLexeme.Length; i++)
        {
            AddElement.AddNewElement(newLexeme[i], FatherLexeme.transform, 30, "");
            AddElement.AddNewElement(newToken[i], FatherToken.transform, 30, "");
            if (newType[i] == "reservada")
            {
                AddElement.AddNewElement(newType[i], FatherType.transform, 28, "");
            }
            else
            {
                AddElement.AddNewElement(newType[i], FatherType.transform, 30, "");
            }
            AddElement.AddNewElement("", FatherValue.transform, 30, "");
            AddElement.AddNewElement("", FatherDesplacement.transform, 30, "");
            AddElement.AddNewElement("", FatherLine.transform, 30, "");
        }
    }


    void Add_new_Lexeme(string NewLexeme, string NewToken, int Num_Line)
    {
        //llenado de la Tabla de simbolos

        SymbolsTransform.sizeDelta = new Vector2(SymbolsTransform.sizeDelta.x, SymbolsTransform.sizeDelta.y + 57);


        Lexeme.Add(NewLexeme);
        AddElement.AddNewElement(NewLexeme, FatherLexeme.transform, 30, "Lexeme");
        Token.Add(NewToken);
        if (NewToken == "Identificador")
        {
            AddElement.AddNewElement(NewToken, FatherToken.transform, 24, "Token");
        }
        else
        {
            AddElement.AddNewElement(NewToken, FatherToken.transform, 30, "Token");
        }


        switch (NewLexeme)
        {
            case "int":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme, FatherType.transform, 30, "Type");
                break;
            case "float":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme, FatherType.transform, 36, "Type");
                break;
            default:
                AddElement.AddNewElement("", FatherType.transform, 30, "Type");
                break;
        }

        No_Line.Add(Num_Line.ToString());
        AddElement.AddNewElement(Num_Line.ToString(), FatherLine.transform, 30, "No_Line");
        AddElement.AddNewElement("", FatherValue.transform, 30, "Value");
        AddElement.AddNewElement("", FatherDesplacement.transform, 30, "Desplacement");


    }


    public void Add_New_Token(string NewLexeme, string NewToken, int Num_Line)
    {
        //Añadido de los elementos a la lista de Tokens
        TokensTransform.sizeDelta = new Vector2(TokensTransform.sizeDelta.x, TokensTransform.sizeDelta.y + 37);
        TL_Lexeme.Add(NewLexeme);
        AddElement.AddNewElement(NewLexeme, FatherTL_Lexeme.transform, 30, "TL_Lexeme");

        //Revision de los tokes
        TL_Token.Add(NewToken);
        NewToken = Update_Tokens(NewLexeme, NewToken);
        AddElement.AddNewElement(NewToken, FatherTL_Token.transform, 24, "TL_Token");
        if (NewToken == "Identificador")
        {
            //Añadido de los elementos a la tabla de simbolos
            Add_new_Lexeme(NewLexeme, NewToken, Num_Line);
        }
    }


    string Update_Tokens(string lexeme,string lasttoken)
    {
        string token = default;
        string[] newTL_Token = TL_Token.ToArray();
        switch (lexeme)
        {
            case "int":
                TL_Token.RemoveAt(newTL_Token.Length - 1);
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            case "float":
                TL_Token.RemoveAt(newTL_Token.Length - 1);
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            default:
                token = lasttoken;
                break;
        }
        return token;
    }

   


    [ContextMenu("Clean")]
    public void Clean()
    {
        TL_Lexeme = new List<string>();
        TL_Token = new List<string>();
        GameObject[] newleme = ObjLexeme.ToArray();
        GameObject[] newtoken = ObjToken.ToArray();
        GameObject[] newtype = ObjType.ToArray();
        GameObject[] newline = ObjNo_Line.ToArray();
        GameObject[] newvalue = ObjNo_Value.ToArray();
        GameObject[] newtl_lexeme = ObjTL_Lexeme.ToArray();
        GameObject[] newvtl_token = ObjTL_Token.ToArray();
        GameObject[] newdesplacement = ObjNo_Desplacement.ToArray();

        #region Reset_Symbols_Table

        for (int i = Lexeme.ToArray().Length - 1; i > 8; i--)
        {
            Lexeme.RemoveAt(i);
        }

        for (int i = Token.ToArray().Length - 1; i > 8; i--)
        {
            Token.RemoveAt(i);
        }

        for (int i = Type.ToArray().Length - 1; i > 8; i--)
        {
            Type.RemoveAt(i);
        }

        No_Line = new List<string>();
        #endregion

        for (int i = 0; i < newleme.Length; i++)
        {
            Destroy(newleme[i]);
            Destroy(newtoken[i]);
            Destroy(newtype[i]);
            Destroy(newline[i]);
            Destroy(newvalue[i]);
            Destroy(newdesplacement[i]);
            Destroy(newtl_lexeme[i]);
            Destroy(newvtl_token[i]);
            SymbolsTransform.sizeDelta = new Vector2(SymbolsTransform.sizeDelta.x, SymbolsTransform.sizeDelta.y - 74);
            TokensTransform.sizeDelta = new Vector2(TokensTransform.sizeDelta.x, TokensTransform.sizeDelta.y - 37);

        }

        ObjLexeme = new List<GameObject>();
        ObjToken = new List<GameObject>();
        ObjType = new List<GameObject>();
        ObjNo_Line = new List<GameObject>();
        ObjNo_Value = new List<GameObject>();
        ObjNo_Desplacement = new List<GameObject>();
        ObjTL_Token = new List<GameObject>();
        ObjTL_Lexeme = new List<GameObject>();




    }
}