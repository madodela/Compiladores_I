/*
 * Created by SharpDevelop.
 * User: Loli
 * Date: 21/05/2013
 * Time: 02:21 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.IO;

namespace DeLexico
{
	class Lexico
	{
		private FileStream inputFile;
		private StreamReader reader;
		private ArrayList listaTokens;
		private string archivo;
		
		//contructor
		public Lexico(string nombreArchivo) {
			archivo = nombreArchivo;
		}
		
		public struct Token {
			public Token_types token_type;
			public String lexema;
		}
		
		public enum Token_types {
			//KEYWORDS
			TKN_IF, TKN_ELSE, TKN_FI, TKN_DO, TKN_UNTIL, TKN_WHILE,
			TKN_READ, TKN_WRITE, TKN_FLOAT, TKN_INT, TKN_BOOL, TKN_PROGRAM,
			//SYMBOLS
			TKN_ADD, TKN_MINUS, TKN_PRODUCT, TKN_DIVISION, TKN_LTHAN,
			TKN_LETHAN, TKN_GTHAN, TKN_GETHAN, TKN_EQUAL, TKN_NEQUAL,
			TKN_ASSIGN, TKN_SEMICOLON, TKN_COMMA, TKN_LPARENT, TKN_RPARENT,
			TKN_LBRACE, TKN_RBRACE, TKN_COMMENT, TKN_MLCOMMENT,
			//IDENTIFIERS AND NUMBERS
			TKN_ID, TKN_NUM,
			//ERROR
			TKN_ERROR,
			//END OF FILE
			TKN_EOF
		};
		
		public ArrayList AnalizadorLexico() {
			listaTokens = new ArrayList();
			LlenarListaTokens();
			return listaTokens;
		}
		
		
		void LlenarListaTokens() {
			try{
				inputFile = new FileStream(archivo,FileMode.Open,FileAccess.Read);
				reader = new StreamReader(inputFile);
				String line;
				line = reader.ReadLine();
				while(line != null){
					String [] tokenParts = line.Split('\t');
					Token token = new Token();
					//token.token_type=tokenParts[0];
					switch(tokenParts[0]) {
						case "TKN_IF":
							token.token_type=Token_types.TKN_IF;
							break;
						case "TKN_ELSE":
							token.token_type=Token_types.TKN_ELSE;
							break;
						case "TKN_FI":
							token.token_type=Token_types.TKN_FI;
							break;
						case "TKN_DO":
							token.token_type=Token_types.TKN_DO;
							break;
						case "TKN_UNTIL":
							token.token_type=Token_types.TKN_UNTIL;
							break;
						case "TKN_WHILE":
							token.token_type=Token_types.TKN_WHILE;
							break;
						case "TKN_READ":
							token.token_type=Token_types.TKN_READ;
							break;
						case "TKN_WRITE":
							token.token_type=Token_types.TKN_WRITE;
							break;
						case "TKN_FLOAT":
							token.token_type=Token_types.TKN_FLOAT;
							break;
						case "TKN_INT":
							token.token_type=Token_types.TKN_INT;
							break;
						case "TKN_BOOL":
							token.token_type=Token_types.TKN_BOOL;
							break;
						case "TKN_PROGRAM":
							token.token_type=Token_types.TKN_PROGRAM;
							break;
						case "TKN_ADD":
							token.token_type=Token_types.TKN_ADD;
							break;
						case "TKN_MINUS":
							token.token_type=Token_types.TKN_MINUS;
							break;
						case "TKN_PRODUCT":
							token.token_type=Token_types.TKN_PRODUCT;
							break;
						case "TKN_DIVISION":
							token.token_type=Token_types.TKN_DIVISION;
							break;
						case "TKN_LTHAN":
							token.token_type=Token_types.TKN_LTHAN;
							break;
						case "TKN_LETHAN":
							token.token_type=Token_types.TKN_LETHAN;
							break;
						case "TKN_GTHAN":
							token.token_type=Token_types.TKN_GTHAN;
							break;
						case "TKN_GETHAN":
							token.token_type=Token_types.TKN_GETHAN;
							break;
						case "TKN_EQUAL":
							token.token_type=Token_types.TKN_EQUAL;
							break;
						case "TKN_NEQUAL":
							token.token_type=Token_types.TKN_NEQUAL;
							break;
						case "TKN_ASSIGN":
							token.token_type=Token_types.TKN_ASSIGN;
							break;
						case "TKN_SEMICOLON":
							token.token_type=Token_types.TKN_SEMICOLON;
							break;
						case "TKN_COMMA":
							token.token_type=Token_types.TKN_COMMA;
							break;
						case "TKN_LPARENT":
							token.token_type=Token_types.TKN_LPARENT;
							break;
						case "TKN_RPARENT":
							token.token_type=Token_types.TKN_RPARENT;
							break;
						case "TKN_LBRACE":
							token.token_type=Token_types.TKN_LBRACE;
							break;
						case "TKN_RBRACE":
							token.token_type=Token_types.TKN_RBRACE;
							break;
						case "TKN_COMMENT":
							token.token_type=Token_types.TKN_COMMENT;
							break;
						case "TKN_MLCOMMENT":
							token.token_type=Token_types.TKN_MLCOMMENT;
							break;
						case "TKN_ID":
							token.token_type=Token_types.TKN_ID;
							break;
						case "TKN_NUM":
							token.token_type=Token_types.TKN_NUM;
							break;
						case "TKN_EOF":
							token.token_type= Token_types.TKN_EOF;
							break;
					}
					token.lexema = tokenParts[1];
					listaTokens.Add(token);
					line = reader.ReadLine();
				}
				reader.Close();
			} catch(FileNotFoundException e) { Console.WriteLine("File Not Found" + e);
			} catch(ArgumentException e) { Console.WriteLine("Cannot read file" + e);
			}
		}
		public void imprimirTokensDeLista( IEnumerable myList) {
			foreach ( Token estructura in myList )
				Console.WriteLine( "{0},{1}", estructura.token_type, estructura.lexema);
			Console.WriteLine();
		}
//		public static void Main(string[] args) {
//			Lexico lexic = new Lexico("LexiconAnalisysTokens.txt");
//			lexic.imprimirTokensDeLista(lexic.AnalizadorLexico());
//			Console.ReadKey(true);
//		}
		
	}
}