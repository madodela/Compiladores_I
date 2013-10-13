/*
 * Created by SharpDevelop.
 * User: María Dolores Delgado Lara
 * 		 José Luis Diáz Montellano
 * Date: 21/05/2013
 * Time: 05:00 p.m.
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
	
	public class TreeNode
	{

		public Token_types op;
		public Token_types varType;
		public int nline;
		public int valInt;
		public double valDouble;
		public bool valBool;
		public bool isIntType = true;
		public string name;
		public TreeNode[] child = new TreeNode[5];
		public TreeNode sibling;
		public NodeKind nodekind;
		public StmtKind stmtK;
		public ExpKind expK;
		public ExpType type;

		//newStmtNode
		public TreeNode(StmtCreation kind , StmtKind kindy)
		{
			this.name = null;
			for (int i = 0 ; i < child.Length ; i++)
			{
				this.child[i] = null;
			}
			this.sibling = null;
			this.nodekind = NodeKind.StmtK;
			this.stmtK = kindy;
			this.expK = ExpKind.None;
		}
		//newExpNode
		public TreeNode(StmtCreation kind , ExpKind kindy)
		{
			this.name = null;
			for (int i = 0 ; i < child.Length ; i++)
			{
				this.child[i] = null;
			}
			this.sibling = null;
			this.nodekind = NodeKind.ExpK;
			this.stmtK = StmtKind.None;
			this.expK = kindy;
			type = ExpType.Void;
		}
		//newDecNode
		public TreeNode(StmtCreation kind)
		{
			this.name = null;
			for (int i = 0 ; i < child.Length ; i++)
			{
				this.child[i] = null;
			}
			this.sibling = null;
			this.nodekind = NodeKind.DecK;
			this.expK = ExpKind.None;
		}
	}
	public enum NodeKind { StmtK , ExpK , DecK };
	public enum StmtKind
	{
		None , IfK , IterationK , RepeatK , ReadK , WriteK ,
		BlockK , AssignK , ProgramK
	};
	public enum ExpKind { None , IdK , OpK , ConstK , SimK };
	public enum ExpType { Void , Integer , Float , Boolean };
	public enum StmtCreation { newStmtNode , newExpNode , newDecNode };


	public class SyntacticAnalizer {

		TreeNode SyntacticTree;
		public int indexCurrentToken = 0;
		public ArrayList tokenList;
		public Token currentToken;
		//private bool error;
		private int identno = 0;
		private Token_types current_valType;
		private bool isDec;
		
		
		//Output file of the analysis, it contains the syntactic tree
		FileStream syntacticTree;
		StreamWriter writerTree;
		//Other output file containing the errors found
		FileStream infoSyntacticAnalisys;
		StreamWriter writerInfo;
		//Objetc to save the symbol table
		SymbolTable symbolTable = new SymbolTable();
		//string currentId;
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
			TreeNode t = null;
			while(currentToken.token_type != Token_types.TKN_IF && currentToken.token_type != Token_types.TKN_WRITE
			      && currentToken.token_type != Token_types.TKN_LBRACE && currentToken.token_type != Token_types.TKN_DO
			      && currentToken.token_type != Token_types.TKN_ID && currentToken.token_type != Token_types.TKN_READ
			      && currentToken.token_type != Token_types.TKN_WHILE && currentToken.token_type != Token_types.TKN_RBRACE) {
				t = declaration();
				match(Token_types.TKN_SEMICOLON);
				t.sibling = declaration_list();
			}
			return t;
		}
		
		//declaración → tipo lista-variables
		TreeNode declaration() {
			isDec = true;
			TreeNode t = null;
			Token_types aux = currentToken.token_type;
			current_valType = aux;
			//tipo → int | float | bool
			if (currentToken.token_type == Token_types.TKN_INT || currentToken.token_type == Token_types.TKN_BOOL
			    || currentToken.token_type == Token_types.TKN_FLOAT)
				match(currentToken.token_type);
			t = variable_list();
			isDec = false;
			return t;
		}
		
		//lista-variables → identificador, lista-variables | identificador
		TreeNode variable_list() {
			TreeNode t = new TreeNode(StmtCreation.newDecNode);
			t.varType = current_valType;
			if ((t != null) && (currentToken.token_type != Token_types.TKN_ID)){
				syntaxError("Wrong declaration");
				//it has to found the comma or the semmicolon token, 'cause that's the end of the wrong variable declaration
				while((currentToken.token_type != Token_types.TKN_COMMA) && (currentToken.token_type != Token_types.TKN_SEMICOLON)){
					currentToken = getToken();
				}
			}
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
			while(currentToken.token_type != Token_types.TKN_RBRACE && currentToken.token_type != Token_types.TKN_EOF
			      && currentToken.token_type != Token_types.TKN_UNTIL && currentToken.token_type != Token_types.TKN_ELSE
			      && currentToken.token_type != Token_types.TKN_FI) {
				t = statement();
				if(currentToken.token_type == Token_types.TKN_IF || currentToken.token_type == Token_types.TKN_WRITE
				   || currentToken.token_type == Token_types.TKN_LBRACE || currentToken.token_type == Token_types.TKN_DO
				   || currentToken.token_type == Token_types.TKN_ID || currentToken.token_type == Token_types.TKN_READ
				   || currentToken.token_type == Token_types.TKN_WHILE) {
					if(t != null)
						t.sibling = stmt_list();
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
				default:
					syntaxError("Unexpected token:");
					printToken(currentToken);
					currentToken = getToken();
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
			if ((t != null) && (currentToken.token_type == Token_types.TKN_ID))
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
			if ((t != null) && (currentToken.token_type == Token_types.TKN_ID))
			{
				t.name = currentToken.lexema;
				
			}
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
			    || (currentToken.token_type == Token_types.TKN_GTHAN)
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
			TreeNode t = term();
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
			if(currentToken.token_type== Token_types.TKN_LPARENT || currentToken.token_type== Token_types.TKN_ID
			   || currentToken.token_type== Token_types.TKN_NUM)
				t = factor();
			else
				t = term();
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
					if ((t != null) && (currentToken.token_type == Token_types.TKN_NUM)) {
						if(currentToken.lexema.Contains(".")) {
							t.valDouble = (Convert.ToDouble(currentToken.lexema));
							t.isIntType = false;
							//symbolTable.st_insert(currentId , t.nline , t.valInt , t.valDouble , false, isDec);
						} else {
							t.valInt = (Convert.ToInt32(currentToken.lexema));
							t.isIntType = true;
							//symbolTable.st_insert(currentId , t.nline , t.valInt , t.valDouble , true , isDec);
						}
					}
					match(Token_types.TKN_NUM);
					break;
					//id option
				case Token_types.TKN_ID:
					t = new TreeNode(StmtCreation.newExpNode, ExpKind.IdK);
					if ((t != null) && (currentToken.token_type == Token_types.TKN_ID))
					{
						t.name = currentToken.lexema;
						t.nline = currentToken.nline;
					}
					match(Token_types.TKN_ID);
					break;
					//(exp) option
				case Token_types.TKN_LPARENT:
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

				if (currentToken.token_type == Token_types.TKN_ID)
				{
					if (isDec)
					{
						string tipo;
						if (current_valType == Token_types.TKN_INT)
							tipo = "Int";
						else if (current_valType == Token_types.TKN_FLOAT)
							tipo = "Float";
						else
							tipo = "Bool";
						symbolTable.st_insert(currentToken.lexema , currentToken.nline , 0 , 0 , true, tipo , true);
					}
					else
					{
						symbolTable.st_insert(currentToken.lexema , currentToken.nline , 0 , 0 ,true, null , false);
					}
				}
				currentToken = getToken();
			} else {
				syntaxError("Unexpected token:");
				Console.WriteLine("Expected token:{0}", expected);
				writerInfo.WriteLine("Expected token:{0}", expected);
				printToken(currentToken);
			}
		}
		
		//Functions to print the Tree and errors, in a file
		void printToken(Token t) {
			Console.WriteLine("{0},{1}", t.token_type, t.lexema);
			writerInfo.WriteLine("{0},{1}", t.token_type, t.lexema);
		}
		
		void syntaxError(string msj){
			Console.WriteLine("Error {0}", msj);
			writerInfo.WriteLine("Error {0}", msj);
			//error = true;
		}
		
		void printSpaces() {
			int i;
			for (i = 0; i<this.identno ; i++){
				Console.Write(" ");
				writerTree.Write(" ");
			}
		}
		
		void printTree(TreeNode tree) {
			int i;
			bool flag = false; // when true should not print the end of the node
			identno += 2;
			while (tree != null) {
				printSpaces();
				if (tree.nodekind == NodeKind.StmtK) {
					switch (tree.stmtK) {
						case StmtKind.IfK:
							Console.Write("If\n");
							writerTree.WriteLine("<node>\"if\"");
							break;
						case StmtKind.IterationK:
							Console.Write("Iteration\n");
							writerTree.WriteLine("<node>\"Iteration\"");
							break;
						case StmtKind.ProgramK:
							Console.Write("Program\n");
							writerTree.WriteLine("<node>\"Program\"");
							break;
						case StmtKind.BlockK:
							Console.Write("Block\n");
							//writerTree.WriteLine("<node>\"Block\"");
							flag = true;
							break;
						case StmtKind.RepeatK:
							Console.Write("Repeat\n");
							writerTree.WriteLine("<node>\"Repeat\"");
							break;
						case StmtKind.AssignK:
							Console.Write("Assign to: {0}\n", tree.name);
							//writerTree.WriteLine("<node>\"AssignTo: {0}\"", tree.name);
							writerTree.WriteLine("<node>\"=\"");
							writerTree.WriteLine("<node>\"{0}\"</node>",tree.name);
							break;
						case StmtKind.ReadK:
							Console.Write("Read: {0}\n", tree.name);
							writerTree.WriteLine("<node>\"Read: {0}\"", tree.name);
							break;
						case StmtKind.WriteK:
							Console.Write("Write\n");
							writerTree.WriteLine("<node>\"Write\"");
							break;
						default:
							Console.Write("Unknown ExpNode kind\n");
							writerInfo.WriteLine("Unknown ExpNode kind");
							break;
					}
				} else {
					if (tree.nodekind==NodeKind.ExpK) {
						switch (tree.expK) {
							case ExpKind.OpK:
								string op;
								switch(tree.op){
										case Token_types.TKN_LTHAN: op = "&lt;"; break; //&lt; is the character '<' in xml
										case Token_types.TKN_LETHAN: op = "&lt;="; break;
										case Token_types.TKN_GETHAN: op = "&gt;="; break; //&gt; is the character '>' in xml
										case Token_types.TKN_GTHAN: op = "&gt;"; break;
										case Token_types.TKN_EQUAL: op = "=="; break;
										case Token_types.TKN_NEQUAL: op = "!="; break;
										case Token_types.TKN_ADD: op = "+"; break;
										case Token_types.TKN_MINUS: op = "-"; break;
										case Token_types.TKN_PRODUCT: op = "*"; break;
										case Token_types.TKN_DIVISION: op = "/"; break;
										default: op = "Unknown operator"; break; //assumed this error never occur
								}
								Console.Write("Op: {0}\n" ,op);
								writerTree.WriteLine("<node>\"Op: {0}\"", op);
								break;
							case ExpKind.ConstK:
								if(tree.isIntType) {
									Console.Write("Const: {0}\n", tree.valInt);
									writerTree.WriteLine("<node>\"Const: {0}\"", tree.valInt);
								} else {
									Console.Write("Const: {0}\n", tree.valDouble);
									writerTree.WriteLine("<node>\"Const: {0}\"", tree.valDouble);
								}
								break;
							case ExpKind.IdK:
								Console.Write("Id: {0}\n", tree.name);
								writerTree.WriteLine("<node>\"Id: {0}    N° Linea:{1}\"", tree.name, tree.nline);
								break;
							default:
								Console.Write("Unknown ExpNode kind\n");
								writerInfo.WriteLine("Unknown ExpNode kind"); //assumed this error never occur
								break;
						}
					} else {
						if(tree.nodekind == NodeKind.DecK) {
							if(tree.name != null) { // if the variable was correctly defined should create its node
								string valTypeString;
								switch(tree.varType) {
										case Token_types.TKN_INT: valTypeString = "INT"; break;
										case Token_types.TKN_FLOAT: valTypeString = "FLOAT"; break;
										case Token_types.TKN_BOOL: valTypeString = "BOOL"; break;
										default: valTypeString = "Unknown type"; break;
								}
								Console.Write("{0}:{1}\n", valTypeString, tree.name);
								writerTree.WriteLine("<node>\"{0}:{1}\"", valTypeString, tree.name);
							} else {
								flag = true;
							}
						} else { Console.WriteLine("Unknown node kind");}
					}
				}
				if (StmtKind.AssignK == tree.stmtK)
				{
					if (tree.child[1]==null &&  tree.child[0].child[0] ==null)
					{
						if (tree.child[0].expK == ExpKind.ConstK)
						{
							symbolTable.st_insert(tree.name , tree.nline , tree.child[0].valInt , tree.child[0].valDouble , tree.child[0].valBool , null , false);
						}
						else
						{
							BucketListRec l = symbolTable.st_lookup(tree.name);
							if(l!=null)
								symbolTable.st_insert(tree.name , tree.nline , l.valI , l.valF , l.valB , null , false);
						}
					}
				}
				for (i = 0; i < 5; i++){
					
					printTree(tree.child[i]);
				}
				if(!flag)
					writerTree.WriteLine("</node>");
				flag = false;
				tree = tree.sibling;
			}
			identno -= 2;
		}

		public void PreorderTraversal(TreeNode node)
		{
			if (node == null)
			{
				return;
			}

			Console.WriteLine(node.nodekind.ToString()+" " + node.name + " " +node.valInt);

			PreorderTraversal(node.child[0]);
			PreorderTraversal(node.child[1]);
			if (node.sibling != null)
				Console.WriteLine("sibling" + " " + node.nodekind);
			PreorderTraversal(node.sibling);
		}

		public SyntacticAnalizer() {
			Lexico lexResults = new Lexico("LexiconAnalisysTokens.txt");
			tokenList = lexResults.AnalizadorLexico();
			try {
				syntacticTree = new FileStream("SyntacticTree.xml", FileMode.Create, FileAccess.Write);
				writerTree = new StreamWriter(syntacticTree);
				infoSyntacticAnalisys = new FileStream("infoSyntacticAnalisys.txt", FileMode.Create, FileAccess.Write);
				writerInfo = new StreamWriter(infoSyntacticAnalisys);
				writerTree.WriteLine("<?xml version=\"1.0\"?>");
				SyntacticTree = Parse(); //Parse function creates the Syntactic Tree
				printTree(SyntacticTree);
				// PreorderTraversal(SyntacticTree);
			} catch(FileNotFoundException e){Console.WriteLine("File Not Found because " + e.Message);
			} catch(ArgumentException e){Console.WriteLine("Cannot read file because " + e.Message);
			} finally {
				writerTree.Close();
				writerInfo.Close();
			}
		}
		
		public static void Main(string[] args) {
			SyntacticAnalizer analizer = new SyntacticAnalizer();
			analizer.symbolTable.printSymTab();
			Console.ReadKey();
		}
	}
}