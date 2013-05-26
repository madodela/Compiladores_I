/*
 * Created by SharpDevelop.
 * User: María Dolores Delgado Lara
 * 		 José Luis Diáz Montellano
 * Date: 21/05/2013
 * Time: 05:00 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Security.Principal;
using System.Collections;

namespace NSSyntacticAnalizer
{
	/// <summary>
	/// Syntactic analyzer's function.
	/// </summary>
	public class SyntacticAnalizer {
		
		public enum NodeKind { StmtK, ExpK , DecK};
		public enum StmtKind {
			None, IfK, IterationK, RepeatK, ReadK, WriteK,
			BlockK, AssignK, ProgramK
		};
		public enum ExpKind { None, IdK, OpK, ConstK, SimK };
		public enum ExpType { Void, Integer, Float, Boolean };
		public enum StmtCreation { newStmtNode, newExpNode , newDecNode};
		public int indexCurrentToken = 0;
		public ArrayList tokenList;
		public Token currentToken;
		private bool error;
		private int identno=0;
		private Token_types current_valType;
		public class TreeNode {
			
			public Token_types op;
			public Token_types varType;
			public int val;
			public string name;
			public TreeNode [] child = new TreeNode[5];
			public TreeNode sibling;
			public NodeKind nodekind;
			public StmtKind stmtK;
			public ExpKind expK;
			public ExpType type;
			
			//newStmtNode
			public TreeNode(StmtCreation kind, StmtKind kindy) {
				this.name=null;
				for(int i = 0; i<child.Length; i++){
					this.child[i] = null;
				}
				this.sibling = null;
				this.nodekind = NodeKind.StmtK;
				this.stmtK = kindy;
				this.expK= ExpKind.None;
			}
			//newExpNode
			public TreeNode(StmtCreation kind, ExpKind kindy) {
				this.name=null;
				for(int i = 0; i<child.Length; i++){
					this.child[i] = null;
				}
				this.sibling = null;
				this.nodekind = NodeKind.ExpK;
				this.stmtK= StmtKind.None;
				this.expK = kindy;
				type = ExpType.Void;
			}
			//newDecNode
			public TreeNode(StmtCreation kind) {
				this.name=null;
				for(int i = 0; i<child.Length; i++){
					this.child[i] = null;
				}
				this.sibling = null;
				this.nodekind = NodeKind.DecK;
				this.expK = ExpKind.None;
			}
			
		}
		//Output file of the analisys, it contains the syntactic tree
		FileStream syntacticTree;
		StreamWriter writerTree;
		//Other output file tah contains the errors founded
		FileStream infoSyntacticAnalisys;
		StreamWriter writerInfo;
		public Token getToken() {
			Token aux = (Token) tokenList[indexCurrentToken];
			indexCurrentToken++;
			return aux;
		}
		
		TreeNode Parse() {
			TreeNode t;
			currentToken = getToken();
			t = program();
			if(!currentToken.token_type.Equals( Token_types.TKN_EOF ))
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
		TreeNode declaration_list() {
			TreeNode t=null;
			while(currentToken.token_type != Token_types.TKN_IF && currentToken.token_type != Token_types.TKN_WRITE
			      && currentToken.token_type != Token_types.TKN_LBRACE && currentToken.token_type != Token_types.TKN_DO
			      && currentToken.token_type != Token_types.TKN_ID && currentToken.token_type != Token_types.TKN_READ
			      && currentToken.token_type != Token_types.TKN_WHILE && currentToken.token_type != Token_types.TKN_RBRACE){
				t = declaration();
				match(Token_types.TKN_SEMICOLON);
				t.sibling=declaration_list();
			}
			return t;
		}
		
		//declaración → tipo lista-variables
		TreeNode declaration() {
			TreeNode t = null;
			Token_types aux=currentToken.token_type;
			current_valType=aux;
			//tipo → int | float | bool
			if(currentToken.token_type== Token_types.TKN_INT || currentToken.token_type== Token_types.TKN_BOOL
			   || currentToken.token_type== Token_types.TKN_FLOAT)
				match(currentToken.token_type);
			
			t = variable_list();
			return t;
		}
		
		//lista-variables → identificador, lista-variables | identificador
		TreeNode variable_list(){
			TreeNode t = new TreeNode(StmtCreation.newDecNode);
			t.varType=current_valType;
			if ((t != null) && (currentToken.token_type==Token_types.TKN_ID)){
				t.name = currentToken.lexema;
				match(Token_types.TKN_ID);
			}
			if(currentToken.token_type == Token_types.TKN_COMMA){
				match(Token_types.TKN_COMMA);
				t.child[0] = variable_list();
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
					t.sibling=stmt_list();
				}
			}
			return t;
		}
		
		/*sentencia →  selección | iteración | repetición  | sent-read |
			sent-write | bloque | asignación*/
		TreeNode statement() {
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
					t = repeat_stmt();
					break;
				case Token_types.TKN_WHILE:
					t = iteration_stmt();
					break;
				case Token_types.TKN_LBRACE:
					t = block_stmt();
					break;
					default :// ERROR 1
						syntaxError("Unexpected token:");
					printToken(currentToken);
					break;
			}
			return t;
		}
		
		/*selección → if ( expresión ) bloque fi|
		if ( expresión ) bloque else bloque fi*/
		TreeNode select_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.IfK);
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
		TreeNode iteration_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.IterationK);
			match(Token_types.TKN_WHILE);
			match(Token_types.TKN_LPARENT);
			if (t != null) t.child[0] = exp();
			match(Token_types.TKN_RPARENT);
			if (t != null) t.child[0] = block_stmt();
			return t;
		}
		
		//repetición → do bloque until ( expresión ) ;
		TreeNode repeat_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.RepeatK);
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
		TreeNode read_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.ReadK);
			match(Token_types.TKN_READ);
			if ((t != null) && (currentToken.token_type==Token_types.TKN_ID))
				t.name = currentToken.lexema;
			match(Token_types.TKN_ID);
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//sent-write → write expresión ;
		TreeNode write_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.WriteK);
			match(Token_types.TKN_WRITE);
			if (t != null) t.child[0] = exp();
			match(Token_types.TKN_SEMICOLON);
			return t;
		}
		
		//bloque → { lista-sentencia }
		TreeNode block_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.BlockK);
			match(Token_types.TKN_LBRACE);
			if( t != null) t.child[0] = stmt_list();
			match(Token_types.TKN_RBRACE);
			return t;
		}
		
		//asignación → identificador = expresión ;
		TreeNode assign_stmt() {
			TreeNode t = new TreeNode(StmtCreation.newStmtNode, StmtKind.AssignK);
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
		TreeNode exp() {
			TreeNode t = simple_exp();
			//relacion → <= | < | > | >= | == | !=
			if ((currentToken.token_type == Token_types.TKN_LETHAN)
			    || (currentToken.token_type == Token_types.TKN_LTHAN)
			    || (currentToken.token_type == Token_types.TKN_GETHAN)
			    || (currentToken.token_type == Token_types.TKN_GETHAN)
			    || (currentToken.token_type == Token_types.TKN_EQUAL)
			    || (currentToken.token_type == Token_types.TKN_NEQUAL)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				if (p != null) {
					p.child[0] = t;
					p.op = currentToken.token_type;
					t = p;
				}
				match(currentToken.token_type);
				if (t != null)
					t.child[1] = simple_exp();
			}
			return t;
		}
		
		//expresión-simple → expresión-simple suma-op termino | termino
		TreeNode simple_exp() {
			TreeNode t=term();
			//suma-op → + | -
			while ((currentToken.token_type == Token_types.TKN_MINUS)
			       || (currentToken.token_type == Token_types.TKN_ADD)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				if (p != null) {
					p.child[0] = t;
					p.op = currentToken.token_type;
					t = p;
					match(currentToken.token_type);
					t.child[1] = simple_exp();
				}
			}
			return t;
		}
		
		//termino → termino mult-op factor | factor
		TreeNode term() {
			TreeNode t;
			if(currentToken.token_type== Token_types.TKN_LPARENT || currentToken.token_type== Token_types.TKN_ID || currentToken.token_type== Token_types.TKN_NUM)
				t = factor();
			else
				t=term();
			//mult-op → * | /
			while ((currentToken.token_type == Token_types.TKN_PRODUCT)
			       || (currentToken.token_type == Token_types.TKN_DIVISION)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				if (p != null) {
					p.child[0] = t;
					p.op = currentToken.token_type;
					t = p;
					match(currentToken.token_type);
					t.child[1] = factor();
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
					if ((t != null) && (currentToken.token_type == Token_types.TKN_NUM))
						t.val = (Convert.ToInt32(currentToken.lexema));
					match(Token_types.TKN_NUM);
					break;
					//id option
				case Token_types.TKN_ID:
					t = new TreeNode(StmtCreation.newExpNode, ExpKind.IdK);
					if ((t != null) && (currentToken.token_type == Token_types.TKN_ID))
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
					syntaxError("Unexpected token:");
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
				Console.WriteLine("Expected token:{0}",expected);
				printToken(currentToken);
			}
		}
		
		//Functions to print the Tree and errors, in a file
		void printToken(Token t) {
			Console.WriteLine("{0},{1}",t.token_type,t.lexema);
			writerInfo.WriteLine("{0},{1}",t.token_type,t.lexema);
		}
		
		void syntaxError(string msj){
			Console.WriteLine("Error {0}",msj);
			writerInfo.WriteLine("Error {0}",msj);
			error = true;
		}
		void printSpaces()
		{ int i;
			for (i=0;i<this.identno;i++){
				Console.Write(" ");
				writerTree.Write(" ");
			}
		}
		void printTree(TreeNode tree)
		{
			int i;
			identno+=2;
			while (tree != null) {
				printSpaces();
				if (tree.nodekind==NodeKind.StmtK)
				{
					switch (tree.stmtK) {
						case StmtKind.IfK:
							Console.Write("If\n");
							writerTree.Write("If\n");
							break;
						case StmtKind.IterationK:
							Console.Write("Iteration\n");
							writerTree.Write("Iteration\n");
							break;
						case StmtKind.ProgramK:
							Console.Write("Program\n");
							writerTree.Write("Program\n");
							break;
						case StmtKind.BlockK:
							Console.Write("Block\n");
							writerTree.Write("Block\n");
							break;
						case StmtKind.RepeatK:
							Console.Write("Repeat\n");
							writerTree.Write("Repeat\n");
							break;
						case StmtKind.AssignK:
							Console.Write("Assign to: {0}\n",tree.name);
							writerTree.Write("Assign to: {0}\n",tree.name);
							break;
						case StmtKind.ReadK:
							Console.Write("Read: {0}\n",tree.name);
							writerTree.Write("Read: {0}\n",tree.name);
							break;
						case StmtKind.WriteK:
							Console.Write("Write\n");
							writerTree.Write("Write\n");
							break;
						default:
							Console.Write("Unknown ExpNode kind\n");
							writerInfo.Write("Unknown ExpNode kind\n");
							break;
					}
				}
				else{
					if (tree.nodekind==NodeKind.ExpK)
					{
						switch (tree.expK) {
							case ExpKind.OpK:
								string op;
								switch(tree.op){
										case Token_types.TKN_LTHAN:op="<";break;
										case Token_types.TKN_LETHAN:op="<=";break;
										case Token_types.TKN_GETHAN:op=">=";break;
										case Token_types.TKN_GTHAN:op=">";break;
										case Token_types.TKN_EQUAL:op="==";break;
										case Token_types.TKN_NEQUAL:op="!=";break;
										case Token_types.TKN_ADD:op="+";break;
										case Token_types.TKN_MINUS:op="-";break;
										case Token_types.TKN_PRODUCT:op="*";break;
										case Token_types.TKN_DIVISION:op="/";break;
										default: op="Unknown operator";break;//assumed this error never occur
								}
								Console.Write("Op:{0}\n",op);
								writerTree.Write("Op:{0}\n",op);
								break;
							case ExpKind.ConstK:
								Console.Write("Const: {0}\n",tree.val);
								writerTree.Write("Const: {0}\n",tree.val);
								break;
							case ExpKind.IdK:
								Console.Write("Id: {0}\n",tree.name);
								writerTree.Write("Id: {0}\n",tree.name);
								break;
							default:
								Console.Write("Unknown ExpNode kind\n");
								writerInfo.Write("Unknown ExpNode kind\n");//assumed this error never occur
								break;
						}
					}
					else{
						if(tree.nodekind==NodeKind.DecK){
							string valTypeString;
							switch(tree.varType){
									case Token_types.TKN_INT:valTypeString="INT";break;
									case Token_types.TKN_FLOAT:valTypeString="FLOAT";break;
									case Token_types.TKN_BOOL:valTypeString="BOOL";break;
									default: valTypeString="Unknown type";break;
							}
							Console.Write("{0}:{1}\n",valTypeString,tree.name);
							writerTree.Write("{0}:{1}\n",valTypeString,tree.name);
						}else { Console.Write("Unknown node kind\n");}
					}
				}
				for (i=0;i<5;i++)
					printTree(tree.child[i]);
				tree = tree.sibling;
			}
			identno-=2;
		}
		
		public SyntacticAnalizer() {
			Lexico lexResults = new Lexico("LexiconAnalisysTokens.txt");
			tokenList = lexResults.AnalizadorLexico();
			try{
				TreeNode SyntacticTree=Parse();//Parse function creates the Syntactic Tree
				syntacticTree=new FileStream("SyntcticTree.txt",FileMode.Create,FileAccess.Write);
				writerTree=new StreamWriter(syntacticTree);
				infoSyntacticAnalisys=new FileStream("infoSyntacticAnalisys.txt",FileMode.Create,FileAccess.Write);
				writerInfo=new StreamWriter(infoSyntacticAnalisys);
				printTree(SyntacticTree);
				writerTree.Close();
				writerInfo.Close();
				Console.ReadKey();
			}
			catch(FileNotFoundException e){Console.WriteLine("File Not Found");}
			catch(ArgumentException e){Console.WriteLine("Cannot read file");}
		}
		
		public static void Main(string[] args) {
			SyntacticAnalizer analizador = new SyntacticAnalizer();
			Console.ReadKey(true);
		}
	}
}
