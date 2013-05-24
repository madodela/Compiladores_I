/*
 * Created by SharpDevelop.
 * User: Loli
 * Date: 21/05/2013
 * Time: 05:00 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Security.Principal;
using System.Collections;

namespace DeLexico
{
	/// <summary>
	/// Syntactic analyzer's function.
	/// </summary>
	public class Sintactico {
		
		public enum NodeKind { StmtK, ExpK };
		public enum StmtKind {
			None, IfK, IterationK, RepeatK, ReadK, WriteK,
			BlockK, AssignK, ProgramK
		};
		public enum ExpKind { None, IdK, OpK, ConstK, SimK };
		public enum ExpType { Void, Integer, Float, Boolean };
		public enum StmtCreation { newStmtNode, newExpNode };
		public int indexCurrentToken = 0;
		public ArrayList tokenList;
		public Token currentToken;
		private Lexico lex;
		private bool error;
		
		public class TreeNode {
			
			public Token_types op;
			public int val;
			public string name;
			public TreeNode [] child = new TreeNode[5];
			public TreeNode sibling;
			public int numOfLine;
			public NodeKind nodekind;
			public StmtKind stmtK;
			public ExpKind expK;
			public ExpType type;
			//newStmtNode
			public TreeNode(StmtCreation kind, StmtKind kindy) {
				for(int i = 0; i<child.Length; i++){
					this.child[i] = null;
					this.sibling = null;
					this.nodekind = NodeKind.StmtK;
					this.stmtK = kindy;
					//this.numOfLine = numOfLine;
				}
			}
			//newExpNode
			public TreeNode(StmtCreation kind, ExpKind kindy) {
				for(int i = 0; i<child.Length; i++){
					this.child[i] = null;
					this.sibling = null;
					this.nodekind = NodeKind.ExpK;
					this.expK = kindy;
					type = ExpType.Void;
					//this.numOfLine = numOfLine;
				}
				
			}
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
		
		public struct Token {
			public Token_types token_type;
			public String lexema;
		}
		
		public Token getToken() {
			Token aux = (Token) tokenList[indexCurrentToken];
			indexCurrentToken++;
			return aux;
		}
		
		TreeNode Parse() {
			TreeNode t;
			currentToken = getToken();
			t = program();
			if(! currentToken.Equals( Token_types.TKN_EOF ))
				syntaxError("Code ends before file\n");
			return t;
		}

		//------------------G R A M M A R S------------------//
		
		//programa → program { lista-declaración lista-sentencias }
		TreeNode program() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.ProgramK);
			match(Token_types.TKN_PROGRAM);
			match(Token_types.TKN_LBRACE);
			if(t != null) {
				t.child[0] = declaration_list();
				t.child[1] = stmt_list();
			}
			match(Token_types.TKN_RBRACE);
			return t;
		}
		
		//lista-declaración → declaración ; lista-declaración | vació
		TreeNode declaration_list() { // this is not complete... not even a little
			TreeNode t = null;
//			TreeNode t = declaration();
//			TreeNode p = t;
//			while() {
//
//			}
			return t;
		}
		
		//declaración → tipo lista-variables
		TreeNode declaration() {
			TreeNode t = null;
			//tipo → int | float | bool
			if(currentToken.token_type == Token_types.TKN_INT
			   || currentToken.token_type == Token_types.TKN_FLOAT
			   || currentToken.token_type == Token_types.TKN_BOOL) {
				match(currentToken.token_type);  // I'm not truely sure of this
				t = variable_list();
			}
			return t;
		}
		
		//lista-variables → identificador, lista-variables | identificador
		TreeNode variable_list(){ // not complete, it needs a stop criteria... which is the ";"... i'm a little crazy right now
			TreeNode t = null;
			while(currentToken.token_type != Token_types.TKN_SEMICOLON){
				match(Token_types.TKN_ID);
				match(Token_types.TKN_COMMA);
				t = variable_list();
			}
			return t;
		}
		
		//lista-sentencias → sentencia lista-sentencias | sentencia | vació
		TreeNode stmt_list() {
			TreeNode t = null;
			while(currentToken.token_type!= Token_types.TKN_RBRACE) {
				t=statement();
				if(currentToken.token_type == Token_types.TKN_IF || currentToken.token_type == Token_types.TKN_WRITE
				   || currentToken.token_type == Token_types.TKN_LBRACE || currentToken.token_type == Token_types.TKN_DO
				   || currentToken.token_type == Token_types.TKN_ID || currentToken.token_type == Token_types.TKN_READ
				   || currentToken.token_type == Token_types.TKN_WHILE) {
					TreeNode p = t;
					TreeNode q;
					q = stmt_list();
					if(q != null) {
						if(t != null) {
							t = p = q;
						} else {
							//it cant be null
							p.sibling = q;
							p = q;
						}
					}
				}
			}
			return t;
		}
		
		/*sentencia →  selección | iteración | repetición  | sent-read |
			sent-write | bloque | asignación*/
		TreeNode statement() {  //OK
			TreeNode t = null;
			switch(currentToken.token_type) {
				case Token_types.TKN_IF:
					t = select_stmt(); //SELECCION
					break;
				case Token_types.TKN_ID:
					t = assign_stmt();
					break;
				case Token_types.TKN_READ:
					t = read_stmt();
					break;
				case Token_types.TKN_WRITE:
					t = write_stmt();
					break;
				case Token_types.TKN_DO:
					//case Token_types.TKN_UNTIL:
					t = repeat_stmt();
					break;
				case Token_types.TKN_WHILE:
					t = iteration_stmt();
					break;
				case Token_types.TKN_LBRACE:
					t = block_stmt();
					break;
				default:// ERROR 1
					syntaxError("Unexpected token:");
					printToken(currentToken);
			}
			return t;
		}
		
		/*selección → if ( expresión ) bloque fi|
		if ( expresión ) bloque else bloque fi*/
		TreeNode select_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.IfK);
			//TreeNode t = newStmtNode(StmtKind.IfK);
			match(Token_types.TKN_IF);
			match(Token_types.TKN_LPARENT);
			if(t != null) {
				t.child[0] = exp();
			}
			match(Token_types.TKN_RPARENT);
			if(t != null) {
				t.child[1] = block_stmt();
			}
			if(currentToken.token_type == Token_types.TKN_ELSE) {
				match(Token_types.TKN_ELSE);
				if(t != null) {
					t.child[2] = block_stmt();
				}
			}
			match(Token_types.TKN_FI);
			return t;
		}
		
		//iteración → while ( expresión ) bloque
		TreeNode iteration_stmt() {//OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.IterationK);
			//TreeNode t = newStmtNode(StmtKind.IterationK);
			match(Token_types.TKN_WHILE);
			match(Token_types.TKN_LPARENT);
			if (t != null) t.child[0] = exp();
			match(Token_types.TKN_RPARENT);
			if (t != null) t.child[0] = block_stmt();
			return t;
		}
		
		//repetición → do bloque until ( expresión ) ;
		TreeNode repeat_stmt() { //OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.RepeatK);
			//TreeNode t = newStmtNode(StmtKind.RepeatK);
			match(Token_types.TKN_DO);
			if (t != null) t.child[0] = block_stmt();
			match(Token_types.TKN_UNTIL);
			match(Token_types.TKN_LPARENT);
			if (t != null) t.child[1] = exp();
			match(Token_types.TKN_RPARENT);
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//sent-read → read identificador ;
		TreeNode read_stmt() {  //OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.ReadK);
			//TreeNode t = newStmtNode(StmtKind.ReadK);
			match(Token_types.TKN_READ);
			if ((t != null) && (currentToken.token_type==Token_types.TKN_ID))
				t.name = currentToken.lexema;
			match(Token_types.TKN_ID);
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//sent-write → write expresión ;
		TreeNode write_stmt() {//OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.WriteK);
			//TreeNode  t = newStmtNode(StmtKind.WriteK);
			match(Token_types.TKN_WRITE);
			if (t != null) t.child[0] = exp();
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//bloque → { lista-sentencia }
		TreeNode block_stmt() {  //OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.BlockK);
			//TreeNode t = newStmtNode(StmtKind.BlockK);
			match(Token_types.TKN_LBRACE);
			if( t != null) t.child[0] = declaration_list();
			match(Token_types.TKN_RBRACE);
			return t;
		}
		
		//asignación → identificador = expresión ;
		TreeNode assign_stmt() { //OK
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.AssignK);
			//TreeNode t = newStmtNode(StmtKind.AssignK);
			if ((t != null) && (currentToken.token_type==Token_types.TKN_ID))
				t.name = currentToken.lexema;
			match(Token_types.TKN_ID);
			match(Token_types.TKN_ASSIGN);
			if (t != null) t.child[0] = exp();
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//expresión → expresión-simple relación expresión-simple |
		//expresión-simple
		TreeNode exp() { //OK
			TreeNode t = simple_exp();
			//relacion → <= | < | > | >= | == | !=
			if ((currentToken.token_type == Token_types.TKN_LETHAN)
			    || (currentToken.token_type == Token_types.TKN_LTHAN)
			    || (currentToken.token_type == Token_types.TKN_GETHAN)
			    || (currentToken.token_type == Token_types.TKN_GETHAN)
			    || (currentToken.token_type == Token_types.TKN_EQUAL)
			    || (currentToken.token_type == Token_types.TKN_NEQUAL)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				//TreeNode p = newExpNode(ExpKind.OpK);
				if (p != null) {
					p.child[0] = t;
					p.op = currentToken.token_type;
					t = p;
				}
				//match(currentToken);
				if (t != null)
					t.child[1] = simple_exp();
			}
			return t;
		}
		
		//expresión-simple → expresión-simple suma-op termino | termino
		TreeNode simple_exp() { //OK
			TreeNode t = term();
			//suma-op → + | -
			while ((currentToken.token_type == Token_types.TKN_MINUS)
			       || (currentToken.token_type == Token_types.TKN_ADD)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				//TreeNode p = newExpNode(ExpKind.OpK);
				if (p != null) {
					p.child[1] = t;
					p.op = currentToken.token_type;
					t = p;
					//match(currentToken);
					t.child[0] = simple_exp();
				}
			}
			return t;
		}
		
		//termino → termino mult-op factor | factor
		TreeNode term() {  //OK
			TreeNode t = factor();
			//mult-op → * | /
			while ((currentToken.token_type == Token_types.TKN_PRODUCT)
			       || (currentToken.token_type == Token_types.TKN_DIVISION)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				//TreeNode p = newExpNode(ExpKind.OpK);
				if (p != null) {
					p.child[1] = t;
					p.op = currentToken.token_type;
					t = p;
					//match(currentToken);
					t.child[0] = term();
				}
			}
			return t;
		}
		
		//factor → ( expresión ) | numero | identificador
		TreeNode factor() {  //OK
			TreeNode t = null;
			switch (currentToken.token_type) {
					//num option
				case Token_types.TKN_NUM:
					t = new TreeNode(StmtCreation.newExpNode, ExpKind.ConstK);
					//t = newExpNode(ExpKind.ConstK);
					if ((t != null) && (currentToken.token_type == Token_types.TKN_NUM))
						t.val = (int)(Convert.ToInt64(currentToken.lexema));
					match(Token_types.TKN_NUM);
					break;
					//id option
				case Token_types.TKN_ID:
					t = new TreeNode(StmtCreation.newExpNode, ExpKind.IdK);
					//t = newExpNode(ExpKind.IdK);
					if ((t != null) && (currentToken.token_type == Token_types.TKN_NUM))
						t.name = currentToken.lexema;
					match(Token_types.TKN_ID);
					break;
					//(exp) option
				case Token_types.TKN_LPARENT :
					match(Token_types.TKN_LPARENT);
					t = exp();
					match(Token_types.TKN_RPARENT);
					break;
				default:
					syntaxError("unexpected token -> ");
					printToken(currentToken);
					currentToken = getToken();
					break;
			}
			return t;
		}
		
		void match(Token_types expected) {
			if(currentToken.token_type == expected) {
				currentToken = getToken();
			}
			else {
				syntaxError("Unexpected token:");
				printToken(currentToken);
			}
		}
		
		void printToken(Token t) {
			Console.WriteLine("{0},{1}",t.token_type,t.lexema);
		}
		
		void syntaxError(string msj){
			Console.WriteLine("Error {0}",msj);
			error = true;
		}
		
		public Sintactico() {
			lex = new Lexico("LexiconAnalisysTokens.txt");
			tokenList = lex.AnalizadorLexico();
			lex.imprimirTokensDeLista(tokenList);
			
		}
		
		public static void Main(string[] args) {
			Sintactico analizador = new Sintactico();
			Console.ReadKey(true);
		}
	}
}