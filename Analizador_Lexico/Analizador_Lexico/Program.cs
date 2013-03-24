/*
 * Created by SharpDevelop.
 * User: Mária Dolores and José Luis
 * Date: 19/03/2013
 * Time: 05:06 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
namespace Analizador_Lexico
{
	class AnalizadorLexico
	{
		const int MAXLENBUF=3000;
		public enum token_types{
			//KEYWORDS
			TKN_IF, TKN_THEN, TKN_ELSE, TKN_FI, TKN_DO, TKN_UNTIL, TKN_WHILE,
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
		
		public enum States{
			IN_START, IN_ID, IN_NUM, IN_LPAREN, IN_RPAREN, IN_SEMICOLON,
			IN_COMMA, IN_EQU, IN_NEQU, IN_ADD, IN_MINUS, IN_EOF, IN_ERROR, IN_DONE, IN_LESS, IN_GR
		};
		public class Token{
			token_types tokenval;
			string lexema;
			public Token(){
				this.Lexema="";
			}
			public Token(token_types tokenval ,string lexema){
				this.tokenval=tokenval;
				this.lexema=lexema;
			}
			public token_types TokenType{
				get{
					return tokenval;
				}
				set{
					this.tokenval=value;
				}
			}
			public string Lexema{
				get{
					return lexema;
				}
				set{
					lexema=value;
				}
			}
		};
		
		Token[] ReserveWords={
			new Token(token_types.TKN_IF,"if"),
			new Token(token_types.TKN_THEN,"then"),
			new Token(token_types.TKN_ELSE,"else"),
			new Token(token_types.TKN_FI,"fi"),
			new Token(token_types.TKN_DO,"do"),
			new Token(token_types.TKN_UNTIL,"until"),
			new Token(token_types.TKN_WHILE,"while"),
			new Token(token_types.TKN_READ,"read"),
			new Token(token_types.TKN_WRITE,"write"),
			new Token(token_types.TKN_FLOAT,"float"),
			new Token(token_types.TKN_INT,"int"),
			new Token(token_types.TKN_BOOL,"bool"),
			new Token(token_types.TKN_PROGRAM,"program")
		};
		int nline=0;
		int ncol=0;
		int n=0;//Caracteres en buffer
		char [] buffer=new char[MAXLENBUF];
		
		void LookUpReservedWords( Token tok,string s){
			int i;
			for(i=0;i<ReserveWords.Length;i++){
				
				if(ReserveWords[i].Lexema.Equals(s)){
					tok.TokenType=ReserveWords[i].TokenType;
					tok.Lexema=ReserveWords[i].Lexema;
					goto EndFunction;
				}
			}
			tok.Lexema=s;
			tok.TokenType=token_types.TKN_ID;
			EndFunction:;
		}
		char GetChar(StreamReader readerFile){
			if(ncol==0 || ncol==n){
				String linea=readerFile.ReadLine();
				if(linea!=null){
					buffer=linea.ToCharArray();
					n=buffer.Length;
					ncol=0;
					nline++;
				}
				else
					return '$';//End of file
			}
			return(buffer[ncol++]);
		}
		void unGetChar(){
			ncol--;
		}
		bool isDelim(char c){
			if(c==' ' || c=='\t' || c=='\n')
				return true;
			return false;
			
		}
		Token GetToken(StreamReader readerFile){
			char c=' ';
			States state= States.IN_START;
			Token token=new Token();
			while(state!=States.IN_DONE){
				switch(state){//Selection of state
						case States.IN_START:{
							c=GetChar(readerFile);
							while(isDelim(c)){//While the character is a delimiter
								c=GetChar(readerFile);
							}
							if(Char.IsLetterOrDigit(c)){
								state=States.IN_ID;
								token.Lexema+=c.ToString();
							}
							else if(Char.IsDigit(c)){
								state= States.IN_NUM;
								token.Lexema+=c.ToString();
							}
							else if(c=='('){
								token.TokenType=token_types.TKN_LPARENT;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c==')'){
								token.TokenType=token_types.TKN_RPARENT;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='}'){
								token.TokenType=token_types.TKN_RBRACE;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='{'){
								token.TokenType=token_types.TKN_LBRACE;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c==';'){
								token.TokenType=token_types.TKN_SEMICOLON;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c==','){
								token.TokenType=token_types.TKN_COMMA;
								state= States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='='){
								state=States.IN_EQU;
								token.TokenType=token_types.TKN_ASSIGN;
								token.Lexema+=c.ToString();
							}
							else if(c=='!'){
								state=States.IN_NEQU;
								token.Lexema+=c.ToString();
							}
							else if(c=='+'){
								token.TokenType=token_types.TKN_ADD;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='-'){
								token.TokenType=token_types.TKN_MINUS;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='*'){
								token.TokenType=token_types.TKN_PRODUCT;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='/'){
								token.TokenType=token_types.TKN_DIVISION;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else if(c=='<'){
								token.TokenType=token_types.TKN_LTHAN;
								state=States.IN_LESS;
								token.Lexema+=c.ToString();
							}
							else if(c=='>'){
								token.TokenType=token_types.TKN_GTHAN;
								state=States.IN_GR;
								token.Lexema+=c.ToString();
							}
							else if(c=='$'){
								token.TokenType=token_types.TKN_EOF;
								state=States.IN_DONE;
								token.Lexema+=c.ToString();
							}
							else{
								token.TokenType=token_types.TKN_ERROR;
								state=States.IN_ERROR;
							}
							break;
						}
						case States.IN_NUM:{
							c=GetChar(readerFile);
							token.Lexema+=c.ToString();
							if(!Char.IsDigit(c)){
								token.TokenType=token_types.TKN_NUM;
								state=States.IN_DONE;
								unGetChar();
							}
							break;
						}
						case States.IN_LESS:{ 
							c=GetChar(readerFile);
							if(c=='='){//pudiera ser el operador <= 
								token.Lexema+=c.ToString();
								token.TokenType=token_types.TKN_LETHAN;
							}else{//o solo ser <
								unGetChar();
							}
							state=States.IN_DONE;
							break;
						}
						case States.IN_GR:{
							c=GetChar(readerFile);
							if(c=='='){//pudiera ser el operador >=
								token.Lexema+=c.ToString();
								token.TokenType=token_types.TKN_GETHAN;
							}else{//o solo ser >
								unGetChar();
							}
							state=States.IN_DONE;
							break;
						}
						case States.IN_NEQU: {
							c=GetChar(readerFile);
							if (c=='=') {
								token.Lexema+=c.ToString();
								token.TokenType=token_types.TKN_NEQUAL;
							}
							state=States.IN_DONE;
							//unGetChar();
							break;
						}
						case States.IN_EQU:{
							c=GetChar(readerFile);
							if(c=='='){
								token.Lexema+=c.ToString();
								token.TokenType=token_types.TKN_EQUAL;
							}else{
								unGetChar();
							}
							state=States.IN_DONE;
							//unGetChar();
							break;
						}
						case States.IN_ID:{
							c=GetChar(readerFile);
							token.Lexema+=c.ToString();
							if(!((Char.IsLetterOrDigit(c))||(c=='_'))){
								token.TokenType=token_types.TKN_ID;
								state=States.IN_DONE;
								unGetChar();
								token.Lexema=token.Lexema.Substring(0,token.Lexema.Length-1);
								LookUpReservedWords(token,token.Lexema);
							}
							break;
						}
						default:{
							token.TokenType= token_types.TKN_ERROR;
							state=States.IN_DONE;
							token.Lexema+=c.ToString();
							break;
						}
				}//end switch
			}//end while
			if(token.TokenType== token_types.TKN_ERROR){
				Console.WriteLine("Line {0}:{1}, Error:Character {2} does not"+
				                  "match any token.",nline,ncol,c);
			}
			return token;
		}
		void ExecuteAnalisys(string fileName){
			//Open File
			Token token;
			//Catch exception
			try{
				FileStream file = new FileStream(fileName,FileMode.Open,FileAccess.Read);
				StreamReader reader = new StreamReader(file);
				token=GetToken(reader);
				while(token_types.TKN_EOF!=token.TokenType){
					Console.WriteLine("({0},{1})",token.TokenType,token.Lexema);
					token=GetToken(reader);
				}
				Console.WriteLine("Analized Lines {0}",nline);
			}
			catch(FileNotFoundException e){Console.WriteLine("File Not Found" + e);}
			catch(ArgumentException e){Console.WriteLine("Cannot read file" + e);}
		}
		public static void Main(string[] args)
		{
			//Instance of AnalizadorLexico
			AnalizadorLexico AL=new AnalizadorLexico();
			if(args.Length==1)
				AL.ExecuteAnalisys(args[0]);
			else
				Console.WriteLine("Usage:programa_name file_name");
			//AL.ExecuteAnalisys("prueba.txt");
			Console.ReadKey();
		}
	}
}