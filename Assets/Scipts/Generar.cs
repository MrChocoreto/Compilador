using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class Generar : MonoBehaviour{

    #region Variables


        [SerializeField]List<string> tokens = new List<string>();
        [SerializeField]List<string> Type = new List<string>();
        [SerializeField]List<string> Despla = new List<string>();
        [SerializeField]List<string> Value = new List<string>();
    [SerializeField] ControlTablas CT;
    //Ruta del documento y el objeto encargado de escribir 
    //en el documento
    string DataPath = default, VoidWord = default;
        string DesplaConverted = default;
        string ValueConverted = default;

        int ASCII = 34;
        char Letter = default;

        StreamWriter formatter;


    #endregion


    #region Testing


        //[ContextMenu("Crear")]
        //void TryToSee()
        //{
        //    FileCreate();
        //    //Recorre cada lexema para poder sacar cada "desplazamiento" y "Valor"
        //    for (int i = 0; i < Type.ToArray().Length; i++)
        //    {
        //        //Dependiendo de cada tipo de dato asi mismo se ira 
        //        //eligiendo que operaciones realizar

        //        DataReader(Type[i], Despla[i], Value[i]);

        //    }

        //    FileClose();
        //}


    #endregion


    #region My_Methods

        public void HacerCODIGO()
        {
        FileCreate();
        tokens = CT.RegresarListaTokens2();
        Type = CT.RegresarListaTipo();
        Despla = CT.RegresarListaDesplasamiento();
        Value = CT.RegresarListaValor();
        for (int i=0;i<tokens.Count ;i++)
        {
            if (tokens[i]=="IDENTIFICADOR")
            {
                DataReader(Type[i],Despla[i],Value[i]);
            }
        }
        FileClose();
        }
        public void FileCreate()
        {

                //Me aseguro de empatar la doble comilla como el mi regla para
                //vacio y la que esta implementada en el compilador, es por ello que creo
                //una variable que contiene las dobles comillas
                Letter = (char)ASCII;
                VoidWord = $"{Letter.ToString() + Letter.ToString()}";


                //Reviso si la no existe la carpeta, y si es correcto procedera a crearla
                //y si ya existe pues solo hara lo demas
                if (!File.Exists(Application.dataPath + "/Codigo_Intermedio"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/Codigo_Intermedio");
                }


                //Aqui solo se crea el documento y se aigna una ruta
                DataPath = Application.dataPath + "/Codigo_Intermedio/data.txt";
                formatter = new StreamWriter(DataPath);
                Debug.Log(DataPath);
            }


        public void DataReader(string type, string despla, string value)
        {
            switch (type)
            {
            case "int":

                if (value == VoidWord)
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, "0", false);
                }
                else
                {
                    DesplaConverted = HexaConverter(despla);
                    ValueConverted = HexaConverter(value);
                    FileWritter(DesplaConverted, ValueConverted, false);
                }

                break;

            case "float":

                if (value == VoidWord)
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, "0", false);
                }
                else
                {

                    //Aqui lo que hago para la parte decimal es solo crear dos variables
                    //en las cuales se almacenaran los valores por independiente del valor 
                    //flotante
                    string DecimalValue = default, IntValue = default;

                    //lo unico que hago es decirle con que voy a separar los datos y en que
                    //parte de quiero extraer la informacion, [0] antes del punto o [1] despues
                    //del punto
                    IntValue = value.Split('.')[0];
                    DecimalValue = value.Split('.')[1];

                    //aqui le doy todo normal solo cambio la posicion de la lista por el valor entero
                    //que acabo de obtener
                    DesplaConverted = HexaConverter(despla);
                    ValueConverted = HexaConverter(IntValue);
                    FileWritter(DesplaConverted, ValueConverted, false);


                    //En esta parte solo vuelvo a vaciar la variable donde tengo el desplazamiento y
                    //la lleno con el desplazamiento del lexema mas dos, por ejemplo: 15 + 2
                    //lo que y eso se lo doy nuevamente al conversor para crear la linea para los decimales


                    DesplaConverted = default;
                    DesplaConverted = $"{int.Parse(despla) + 2}";

                    DesplaConverted = HexaConverter(DesplaConverted);
                    ValueConverted = HexaConverter(DecimalValue);
                    FileWritter(DesplaConverted, ValueConverted, false);

                }

                break;


            case "string":

                //Para el caso de los strings no hace falta convertir nada a hex
                //ya que por motivos de la materia solo se requiere pasar el string
                //tal y como se encuentra en la tabla de simbolos

                if (value == VoidWord)
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, "", true);
                }
                else
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, value, true);
                }
                break;


            case "char":

                //El en caso de char se debe de obtener el codigo ASCII para
                //poder transformarlo a hex y ya en ese momento formatearlo
                if (value == VoidWord)
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, "0", false);
                }
                else
                {
                    char[] a = value.ToCharArray();
                    int ASCII = a[0];
                    ValueConverted = "" + ASCII;

                    DesplaConverted = HexaConverter(despla);
                    ValueConverted = HexaConverter(ValueConverted);
                    FileWritter(DesplaConverted, ValueConverted, false);
                }
                break;


            case "bool":

                //En el caso de bool es mas sencillo ya que solo devuelve un valor
                //un 0 o un 1, cosa que es super facil de hacer solo tomando en cuenta
                //que valor contiene el bool
                if (value == VoidWord)
                {
                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, "0", false);
                }
                else
                {
                    if (value == "false") ValueConverted = "0";
                    else ValueConverted = "1";

                    DesplaConverted = HexaConverter(despla);
                    FileWritter(DesplaConverted, ValueConverted, false);
                }
                break;
        }
        }


        void FileWritter(string Despla, string Value, bool String)
        {
            string FormatDespla = default;
            string FormatValue = default;



            //Aqui solo le doy el valor de desplazamiento al metodo, para
            //formatearlo y despues de formatearlo solo le agrego la palabra
            //"MOV" antes de la palabra generada para poder generar la intruccion

            if (!String)
            {
                FormatDespla = Formateador_Hex(Despla);
                FormatValue = Formateador_Hex(Value);
                formatter.WriteLine("MOV " + FormatDespla + ", " + FormatValue + "H");
            }
            else
            {
                FormatDespla = Formateador_Hex(Despla);
                formatter.WriteLine("MOV " + FormatDespla + ", " + Value);
            }




            //Paso a vaciar la variable por cada vuelta para poder generar una
            //nueva instruccion con su asignacion correspondiente
            FormatDespla = default;
            FormatDespla = default;

        }


        string Formateador_Hex(string Hexadecimal)
        {
            string Formateador = default;
            int counter = default;
            int Refiller = default;


            //En caso de que el formato no este completo el ciclo for
            //se encargara de relleñar con ceros la parte restante ejemplo:
            // 34 -> 0000000000000034
            if (Hexadecimal.Length < 16)
            {
                Refiller = 16 - Hexadecimal.Length;
                for (int i = 0; i < Refiller; i++)
                {
                    Hexadecimal = "0" + Hexadecimal;
                }
            }



            //Recorre cada desplazamiento para poder hacer el formateo de 4 en 4 caracteres
            for (int j = Hexadecimal.Length - 1; j > -1; j--)
            {

                //cada 4 digitos realiza el formateo de los datos
                if (counter == 4)
                {
                    counter = 1;
                    Formateador = Hexadecimal[j] + " " + Formateador;

                }

                //caso contrario solo pasa a aumentar el contador y a agregar mas
                //caracteres a la variable string
                else
                {
                    counter++;
                    Formateador = Hexadecimal[j] + Formateador;
                }
            }


            return Formateador;
        }


        string HexaConverter(string No_Decimal) 
        {
            //se crea una variable string donde se almacenara el resultado
            string hexValue = default;
        
            //devuelve el Hex del valor requerido, cabe señalar que el
            //formateador requiere de un valor numerico el cual debe ser
            //aparentemente entero de lo contrario no podra funcionar adecuadamente
            hexValue = string.Format("{0:X}", int.Parse(No_Decimal));

            return hexValue;
        }


        public void FileClose()
        {
            formatter.Close();
        }


    #endregion


}