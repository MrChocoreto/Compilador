using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analisis3 : MonoBehaviour
{
    [SerializeField] ControlTablas CT;
    [SerializeField] Generar Gen;
    [SerializeField] List<string> Token, Tipo,Tokens,Lexemas,Lex;
    int acumulado;
    List<int> Lineas;
    string TipoEncontrado,Identificador,Valor; 
    int e = 0;
    bool erroren,Pasar=true;
    Anotacion AN;
    private void Start()
    {
        AN = gameObject.GetComponent<Anotacion>();
    }
    public void Despasamientos()
    {
        acumulado = 0;
        e = 0;
        Lineas = CT.RegresarListaLineas();
        Tokens = CT.RegresarListaTokens();
        Lexemas = CT.RegresarListaLexemas();
        Token = CT.RegresarListaTokens2();
        Lex = CT.RegresarListaLexemas2();
        Tipo = CT.RegresarListaTipo();
        for (int i=0;i<Token.Count;i++)
        {
            if (Token[i]=="IDENTIFICADOR")
            {
                CT.agregarDes(i,acumulado);
                CT.agregarValorDes(i,ValorDespasamientos(Tipo[i]));
                acumulado += ValorDespasamientos(Tipo[i]);
                if (CT.RegresarValor(Lex[i])=="")
                {
                    CT.AgregarValor(Lex[i],ValorDefault(Tipo[i]));
                }
            }
        }
        for (int i = 0; i < Token.Count; i++)
        {
            if (Token[i] == "CLASE")
            {
                int val=0 , pos=i;
                i++;
                for (; i < Token.Count; i++)
                {
                    if (Token[i]=="CLASE")
                    {                      
                        i--;
                        break;
                    }
                    if (Token[i] == "IDENTIFICADOR")
                    {
                        val += int.Parse(CT.regresarDespla(i));
                    }
                }
                CT.agregarValorDes(pos, val);
            }
        }
        EncontrarValor();
        ComprobarComparacion();
        VerificarSwitch();
        VerificarIncreDecre();
        if (Pasar)
        {
            Gen.HacerCODIGO();
        }
    }
    void VerificarIncreDecre()
    {
        e = 0;
        for (; e < Tokens.Count; e++)
        {
            if (Tokens[e] == "INCREMENTO"|| Tokens[e] == "DECREMENTO")
            {
                if (CT.RegresarTipo(Lexemas[e-1])!="float" && CT.RegresarTipo(Lexemas[e - 1]) != "int")
                {
                    CT.AgregarMensaje("ERROR", Lexemas[e - 1] + " NO ES DEL TIPO: float o int", "" + Lineas[e]);
                    Pasar = false;
                }
            }
        }
    }
    void VerificarSwitch()
    {
        e = 0;
        for (; e < Tokens.Count; e++)
        {
            if (Tokens[e] == "SWICH")
            {
                Debug.Log(2);
                e += 2;
                if (Tokens[e] == "CADENA" || Tokens[e] == "CARACTER" || Tokens[e] == "ENTERO" || Tokens[e] == "DECIMAL" || Tokens[e] == "BOOL")
                {
                    TipoEncontrado = ValorTipo(Tokens[e]);
                }
                else if (Tokens[e] == "IDENTIFICADOR")
                {
                    TipoEncontrado = CT.RegresarTipo(Lexemas[e]);
                }
                for (; Tokens[e] != "LLC"; e++)
                {
                    if (Tokens[e] == "CASE")
                    {
                        if (ValorTipo(Tokens[e + 1]) != TipoEncontrado)
                        {
                            CT.AgregarMensaje("ERROR", Lexemas[e+1] + " NO ES DEL TIPO: " + TipoEncontrado, "" + Lineas[e]);
                        }
                    }
                }
            }
        }
    }
    void EncontrarValor()
    {      
        for (; e < Tokens.Count; e++)
        {
            if (Tokens[e]=="Asignacion")
            {
                erroren = false;
                AgregarTipoIden(CT.RegresarTipo(Lexemas[e - 1]),Lexemas[e -1]);
                TipoVariable();
                limpiar();
                if (erroren)
                {
                    Pasar = false;
                }
            }
        }
    }
    void ComprobarComparacion()
    {
        e = 0;
        for (; e < Tokens.Count; e++)
        {
            if (Tokens[e] == "MAYOR"|| Tokens[e] == "MAYORIGUAL"||Tokens[e] == "MENOR"|| Tokens[e] == "MENORIGUAL"|| Tokens[e] == "ESIGUAL" || Tokens[e] == "DIFERENTE")
            {
                VerificarTipos();
                limpiar();
            }
        }
    }
    void VerificarTipos()
    {
        string TipoEn = "";
        for (int i=e; Tokens[i] != "PYC" && Tokens[i] != "||" && Tokens[i] != "&&" && Tokens[i] != "PA"; i--)
        {
            if (Tokens[i] == "PC")
            {
                Debug.Log(1);
                for (; ;i--)
                {
                    if (Tokens[i]=="IDENTIFICADOR")
                    {
                        if (TipoEn=="")
                        {                           
                            TipoEn = CT.RegresarTipo(Lexemas[i]);
                        }else if (TipoEn!= CT.RegresarTipo(Lexemas[i]))
                        {
                            CT.AgregarMensaje("ERROR", Lexemas[i] + " NO ES DEL TIPO: " + TipoEn, "" + Lineas[i]);
                        }
                        break;
                    }
                }
            }else if (Tokens[i] == "IDENTIFICADOR")
            {
                if (TipoEn == "")
                {
                    TipoEn = CT.RegresarTipo(Lexemas[i]);
                }
                else if (TipoEn != CT.RegresarTipo(Lexemas[i]))
                {
                    CT.AgregarMensaje("ERROR", Lexemas[i] + " NO ES DEL TIPO: " + TipoEn, "" + Lineas[i]);
                    break;
                }
            }
            if (Tokens[i] == "CADENA" || Tokens[i] == "CARACTER" || Tokens[i] == "ENTERO" || Tokens[i] == "DECIMAL" || Tokens[i] == "BOOL")
            {
                if (TipoEn == "")
                {
                    TipoEn = ValorTipo(Tokens[i]);
                }
                else if (TipoEn != Tokens[i])
                {
                    CT.AgregarMensaje("ERROR", Lexemas[i] + " NO ES DEL TIPO: " + TipoEn, "" + Lineas[i]);
                    break;
                }
            }
        }
        for (int i = e; Tokens[i] != "PYC" && Tokens[i] != "||" && Tokens[i] != "&&" && Tokens[i] != "PC"; i++)
        {
            if (Tokens[i] == "IDENTIFICADOR")
            {
                int posE = e;
                e = i;
                TipoEncontrado = TipoEn;
                IdentificadorMetodo();
                i = e;
                e = posE;
            }
            if (Tokens[i] == "CADENA" || Tokens[i] == "CARACTER" || Tokens[i] == "ENTERO" || Tokens[i] == "DECIMAL" || Tokens[i] == "BOOL")
            {
                if (TipoEn == "")
                {
                    TipoEn = ValorTipo(Tokens[i]);
                }
                else if (TipoEn != ValorTipo(Tokens[i]))
                {
                    CT.AgregarMensaje("ERROR", Lexemas[i] + " NO ES DEL TIPO: " + TipoEn, "" + Lineas[i]);
                    break;
                }
            }
        }
    }
    int ValorDespasamientos(string t)
    {
        int valor=0;
        switch (t)
        {
            case "int":
                valor = 4;
                break;
            case "float":
                valor = 4;
                break;
            case "string":
                valor = 256;
                break;
            case "char":
                valor = 1;
                break;
            case "bool":
                valor = 1;
                break;
        }
        return valor;
    }
    string ValorTipo(string t)
    {
        string valor = "";
        switch (t)
        {
            case "ENTERO":
                valor = "int";
                break;
            case "DECIMAL":
                valor = "float";
                break;
            case "CADENA":
                valor = "string";
                break;
            case "CARACTER":
                valor = "char";
                break;
            case "BOOL":
                valor = "bool";
                break;
        }
        return valor;
    }
    string ValorDefault(string t)
    {
        string valor="";
        switch (t)
        {
            case "int":
                valor = ""+0;
                break;
            case "float":
                valor = "" + 0;
                break;
            case "string":
                valor = "a";
                break;
            case "char":
                valor = "";
                break;
            case "bool":
                valor = ""+true;
                break;
        }
        return valor;
    }
    public void AgregarTipoIden(string T,string I)
    {
        TipoEncontrado = T;
        Identificador = I;
    }
    void TipoVariable()
    {
            switch (TipoEncontrado)
            {
                case "int":
                TipoInt();
                    break;
                case "float":
                TipoFloat();
                    break;
                case "string":
                TipoString();
                    break;
                case "char":
                TipoChar();
                    break;
                case "bool":
                TipoBool();
                    break;
            }
    }
    private void TipoInt()
    {
        e++;
        float res = 0;
        List<string> Valores = new List<string>();
        for (; Tokens[e] != "PYC" && Tokens[e] != "COMA" && Tokens[e + 1] != "LLA"; e++)
        {
            if (Tokens[e] == "CADENA" || Tokens[e] == "CARACTER" || Tokens[e] == "BOLL" || Tokens[e] == "DECIMAL")
            {
                erroren = true;
                CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
                break;
            }
            if (Tokens[e] == "IDENTIFICADOR")
            {
                IdentificadorMetodo();
            }
            Valores.Add(Lexemas[e]);
        }
        if (Valores.Count > 1 && !erroren)
        {
            Debug.Log("hacer calculo");
            res =(int)AN.HacerCalculo(Valores);
        }
        else if(Valores.Count == 1 && !erroren)
        {
            res =float.Parse(Valores[0]);
        }

        if (res>int.MaxValue)
        {
            CT.AgregarMensaje("ERROR","EL VALOR DE "+Identificador+" ES DEMACIADO GRANDE",""+Lineas[e]);
        }
        else if(res < int.MinValue)
        {
            CT.AgregarMensaje("ERROR", "EL VALOR DE " + Identificador + " ES DEMACIADO PEQUEÑO", ""+Lineas[e]);
        }
        else
        {
            CT.AgregarValor(Identificador, "" + res);
        }
    }
    private void TipoFloat()
    {
        e++;
        float res =0;
        List<string> Valores = new List<string>();
        for (; Tokens[e] != "PYC" && Tokens[e] != "COMA" && Tokens[e + 1] != "LLA"; e++)
        {
            if (Tokens[e] == "CADENA" || Tokens[e] == "CARACTER" || Tokens[e] == "BOLL")
            {
                CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
                erroren = true;
                break;
            }
            if (Tokens[e] == "IDENTIFICADOR")
            {
                IdentificadorMetodo();
            }
            Valores.Add(Lexemas[e]);
        }
        if (Valores.Count> 1 && !erroren)
        {
            Debug.Log("hacer calculo");
            res=AN.HacerCalculo(Valores);
        }
        else if (Valores.Count == 1 && !erroren)
        {
            res = float.Parse(Valores[0]);
        }
        if (res > float.MaxValue)
        {
            CT.AgregarMensaje("ERROR", "EL VALOR DE " + Identificador + " ES DEMACIADO GRANDE", "" + Lineas[e]);
        }
        else if (res < float.MinValue)
        {
            CT.AgregarMensaje("ERROR", "EL VALOR DE " + Identificador + " ES DEMACIADO PEQUEÑO", "" + Lineas[e]);
        }
        else
        {
            CT.AgregarValor(Identificador, "" + res);
        }
    }
    private void TipoString()
    {
        e++;
        for (; Tokens[e] != "PYC" && Tokens[e] != "COMA" && Tokens[e + 1] != "LLA"; e++)
        {
            if (Tokens[e] == "MULTIPLICACION" || Tokens[e] == "DIVIDIR" || Tokens[e] == "POTENCIA" || Tokens[e] == "RESTA")
            {
                CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
                erroren = true;
                break;
            }
            if (Tokens[e] == "IDENTIFICADOR")
            {
                IdentificadorMetodo();
            }
            if (Tokens[e] != "SUMA")
            {
                Valor += Lexemas[e];
            }
        }
        if (Valor.Length>256)
        {
            CT.AgregarMensaje("ERROR",Identificador+ "EL VALOR SOBREPASA LOS 256 CARACTEREES","");
        }
        else
        {
            Valor =$"\"{Valor}\"";
            CT.AgregarValor(Identificador, Valor);
        }
    }
    private void TipoChar()
    {
        e++;
        for (; Tokens[e] != "PYC" && Tokens[e] != "COMA" && Tokens[e + 1] != "LLA"; e++)
        {
            if (Tokens[e] != "PC" && Tokens[e] != "PA" && Tokens[e] != "IDENTIFICADOR" && Tokens[e] != "CARACTER")
            {
                CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
                break;
            }
            if (Tokens[e] == "IDENTIFICADOR")
            {
                IdentificadorMetodo();
            }
            if (!erroren)
            {
                Valor += Lexemas[e];
            }
        }
        CT.AgregarValor(Identificador, Valor);
    }
    private void TipoBool()
    {
        e++;
        for (; Tokens[e] != "PYC" && Tokens[e] != "COMA" && Tokens[e + 1] != "LLA"; e++)
        {
            if (Tokens[e] != "PC" && Tokens[e] != "PA" && Tokens[e] != "IDENTIFICADOR" && Tokens[e] != "BOOL")
            {
                CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
                break;
            }
            if (Tokens[e] == "IDENTIFICADOR")
            {
                IdentificadorMetodo();
            }
            if (!erroren)
            {
                Valor += Lexemas[e];
            }
        }
        CT.AgregarValor(Identificador, Valor);
    }
    void IdentificadorMetodo()
    {
        if (TipoEncontrado==CT.RegresarTipo(Lexemas[e]) || CT.RegresarTipo(Lexemas[e])=="int" && TipoEncontrado =="float" ||TipoEncontrado=="string")
        {
            if (Tokens[e+1]=="PA")
            {
                Lexemas[e] = ValorDefault(TipoEncontrado);             
                for (; ;e++)
                {
                    if (Tokens[e]=="PC")
                    {
                        Lexemas[e] = ValorDefault(TipoEncontrado);
                        break;
                    }
                }
            }
            else
            {
                string v = CT.RegresarValor(Lexemas[e]);
                if (v=="")
                {
                    Lexemas[e] = ValorDefault(TipoEncontrado);
                }
                else
                {
                    Lexemas[e] = v;
                }
            }
        }
        else
        {
            erroren = true;
            CT.AgregarMensaje("ERROR", Lexemas[e] + " NO ES DEL TIPO :" + TipoEncontrado, "" + Lineas[e]);
        }
    }
    void limpiar()
    {
        TipoEncontrado = "";
        Identificador = "";
        Valor = "";
    }
}
