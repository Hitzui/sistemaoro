namespace SistemaOro.Data.Libraries;

public class Utilities
{
    public static string NumeroALetras(decimal numero)
    {
        // Separar parte entera y parte decimal
        var parteEntera = (int)Math.Truncate(numero);
        var parteDecimal = (int)Math.Round((numero - parteEntera) * 100);

        var palabras = NumeroALetras(parteEntera) + " con " + parteDecimal.ToString("00") + "/100";

        return palabras;
    }
    public static string NumeroALetras(int numero)
    {
        if (numero == 0)
            return "Cero";

        if (numero < 0)
            return "Menos " + NumeroALetras(Math.Abs(numero));

        string palabras = "";

        string[] unidades =
        {
            "", "Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve"
        };

        string[] decenas =
        {
            "", "Diez", "Veinte", "Treinta", "Cuarenta", "Cincuenta", "Sesenta", "Setenta", "Ochenta", "Noventa"
        };

        string[] especiales =
        {
            "Diez", "Once", "Doce", "Trece", "Catorce", "Quince", "Dieciséis", "Diecisiete", "Dieciocho", "Diecinueve"
        };

        string[] centenas =
        {
            "", "Ciento", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos"
        };

        if (numero < 10)
            palabras += unidades[numero];
        else if (numero < 20)
            palabras += especiales[numero - 10];
        else if (numero < 100)
            palabras += decenas[numero / 10] + " " + NumeroALetras(numero % 10);
        else if (numero < 1000)
            palabras += centenas[numero / 100] + " " + NumeroALetras(numero % 100);
        else if (numero < 1000000)
            palabras += NumeroALetras(numero / 1000) + " Mil " + NumeroALetras(numero % 1000);
        else if (numero < 1000000000)
            palabras += NumeroALetras(numero / 1000000) + " Millón " + NumeroALetras(numero % 1000000);

        return palabras;
    }

}