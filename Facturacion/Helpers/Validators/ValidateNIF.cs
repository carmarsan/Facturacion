using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Facturacion.Helpers
{
	public class ValidateNIF
	{
		public static Boolean valida_NIFCIFNIE(string data)
		{
			if (String.IsNullOrEmpty(data) || data.Length < 8)
				return false;

			var initialLetter = data.Substring(0, 1).ToUpper();
			if (Char.IsLetter(data, 0))
			{
				switch (initialLetter)
				{
					case "X":
						data = "0" + data.Substring(1, data.Length - 1);
						return validarNIF(data);
					case "Y":
						data = "1" + data.Substring(1, data.Length - 1);
						return validarNIF(data);
					case "Z":
						data = "2" + data.Substring(1, data.Length - 1);
						return validarNIF(data);
					default:
						if (new Regex("[A-Za-z][0-9]{7}[A-Za-z0-9]{1}$").Match(data).Success)
							return validadCIF(data);
						break;
				}
			}
			else if (Char.IsLetter(data, data.Length - 1))
			{
				if (new Regex("[0-9]{8}[A-Za-z]").Match(data).Success || new Regex("[0-9]{7}[A-Za-z]").Match(data).Success)
					return validarNIF(data);
			}
			return false;
		}

		private static string getLetra(int id)
		{
			Dictionary<int, String> letras = new Dictionary<int, string>();
			letras.Add(0, "T");
			letras.Add(1, "R");
			letras.Add(2, "W");
			letras.Add(3, "A");
			letras.Add(4, "G");
			letras.Add(5, "M");
			letras.Add(6, "Y");
			letras.Add(7, "F");
			letras.Add(8, "P");
			letras.Add(9, "D");
			letras.Add(10, "X");
			letras.Add(11, "B");
			letras.Add(12, "N");
			letras.Add(13, "J");
			letras.Add(14, "Z");
			letras.Add(15, "S");
			letras.Add(16, "Q");
			letras.Add(17, "V");
			letras.Add(18, "H");
			letras.Add(19, "L");
			letras.Add(20, "C");
			letras.Add(21, "K");
			letras.Add(22, "E");
			return letras[id];
		}

		private static bool validarNIF(string data)
		{
			if (data == String.Empty)
				return false;
			try
			{
				String letra;
				letra = data.Substring(data.Length - 1, 1);
				data = data.Substring(0, data.Length - 1);
				int nifNum = int.Parse(data);
				int resto = nifNum % 23;
				string tmp = getLetra(resto);
				if (tmp.ToLower() != letra.ToLower())
					return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			return true;
		}

		private static bool validadCIF(string data)
		{
			try
			{
				int pares = 0;
				int impares = 0;
				int suma;
				string ultima;
				int unumero;
				string[] uletra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "0" };
				string[] fletra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
				int[] fletra1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
				string xxx;

				/*
				* T      P      P      N  N  N  N  N  C
				Siendo:
				T: Letra de tipo de Organización, una de las siguientes: A,B,C,D,E,F,G,H,K,L,M,N,P,Q,S.
				P: Código provincial.
				N: Númeración secuenial dentro de la provincia.
				C: Dígito de control, un número ó letra: Aó1,Bó2,Có3,Dó4,Eó5,Fó6,Gó7,Hó8,Ió9,Jó0.
				*
				*
				A.    Sociedades anónimas.
				B.    Sociedades de responsabilidad limitada.
				C.    Sociedades colectivas.
				D.    Sociedades comanditarias.
				E.    Comunidades de bienes y herencias yacentes.
				F.    Sociedades cooperativas.
				G.    Asociaciones.
				H.    Comunidades de propietarios en régimen de propiedad horizontal.
				I.    Sociedades civiles, con o sin personalidad jurídica.
				J.    Corporaciones Locales.
				K.    Organismos públicos.
				L.    Congregaciones e instituciones religiosas.
				M.    Órganos de la Administración del Estado y de las Comunidades Autónomas.
				N.    Uniones Temporales de Empresas.
				O.    Otros tipos no definidos en el resto de claves.

				*/
				data = data.ToUpper();

				ultima = data.Substring(8, 1);

				int cont = 1;
				for (cont = 1; cont < 7; cont++)
				{
					xxx = (2 * int.Parse(data.Substring(cont++, 1))) + "0";
					impares += int.Parse(xxx.ToString().Substring(0, 1)) + int.Parse(xxx.ToString().Substring(1, 1));
					pares += int.Parse(data.Substring(cont, 1));
				}

				xxx = (2 * int.Parse(data.Substring(cont, 1))) + "0";
				impares += int.Parse(xxx.Substring(0, 1)) + int.Parse(xxx.Substring(1, 1));

				suma = pares + impares;
				unumero = int.Parse(suma.ToString().Substring(suma.ToString().Length - 1, 1));
				unumero = 10 - unumero;
				if (unumero == 10) unumero = 0;

				if ((ultima == unumero.ToString()) || (ultima == uletra[unumero - 1]))
					return true;
				else
					return false;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}