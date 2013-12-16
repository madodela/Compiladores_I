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

        public Token_types op , varType;
		//public Token_types varType;
        public int nline , valInt;
		//public int valInt;
		public double valDouble;
        public bool valBool , typeError , undeclaredError , isIntType = true;
		//public bool isIntType = true;
		public string name;
		public TreeNode[] child = new TreeNode[5];
		public TreeNode sibling;
		public NodeKind nodekind;
		public StmtKind stmtK;
		public ExpKind expK;
		public ExpType type;
        //public bool typeError , undeclaredError;

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
		public bool tieneVal = false;

		TreeNode SyntacticTree;
		public int indexCurrentToken = 0;
		public ArrayList tokenList;
		public Token currentToken;
		//private bool error;
		private int identno = 0;
		private Token_types current_valType;
		private bool isDec; //es declaracion, solo si es, pasa el tipo
		String tipoActual;
		private static int memloc = 0;
		
		//Output file of the analysis, it contains the syntactic tree
		FileStream syntacticTree;
		StreamWriter writerTree;
		//Output file of the analysis, it contains the syntactic tree
		FileStream semanticTree;
		StreamWriter writerSemanticTree;
		//Other output file containing the errors found
		FileStream infoSyntacticAnalisys;
        FileStream infoSemanticAnalisys;
		StreamWriter writerInfo;
        StreamWriter writerInfoSemantic;
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
			if (t != null) t.child[1] = block_stmt();
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
            TreeNode t ;
           if (currentToken.token_type == Token_types.TKN_LPARENT || currentToken.token_type == Token_types.TKN_ID
               || currentToken.token_type == Token_types.TKN_NUM)
                t = term();
            else
                t = simple_exp();
			//suma-op → + | -
			while ((currentToken.token_type == Token_types.TKN_MINUS)
			       || (currentToken.token_type == Token_types.TKN_ADD)) {
				TreeNode p = new TreeNode(StmtCreation.newExpNode, ExpKind.OpK);
				if (p != null) {
					p.child[0] = t;
					p.op = currentToken.token_type;
					t = p;
					match(currentToken.token_type);
					t.child[1] = term();
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
							
						} else {
							t.valInt = (Convert.ToInt32(currentToken.lexema));
							t.isIntType = true;
							
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
						symbolTable.st_insert(currentToken.lexema , currentToken.nline , 0 , 0 , true, tipo , true, false, memloc++);
					}
					else
					{
						BucketListRec l = symbolTable.st_lookup(currentToken.lexema);
						if (l !=null)
							symbolTable.st_insert(currentToken.lexema , currentToken.nline , 0 , 0 , true , null , false , false, memloc++);
						else
							Console.WriteLine("Variable not declared: "+currentToken.lexema);
					}
				}
				currentToken = getToken();
			} else {
				syntaxError("Unexpected token:");
				Console.WriteLine("Expected token:{0}", expected);
				writerInfo.WriteLine("Expected token:{0}", expected);
				Console.WriteLine("Line:{0}" , currentToken.nline);
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
		
		void printBothTrees(TreeNode tree) { // imprime el árbol sintáctico y el semántico
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
                            writerSemanticTree.WriteLine("<node>\"if\"");
							break;
						case StmtKind.IterationK:
							Console.Write("Iteration\n");
							writerTree.WriteLine("<node>\"Iteration\"");
							writerSemanticTree.WriteLine("<node>\"Iteration\"");
							break;
						case StmtKind.ProgramK:
							Console.Write("Program\n");
							writerTree.WriteLine("<node>\"Program\"");
							writerSemanticTree.WriteLine("<node>\"Program\"");
							break;
						case StmtKind.BlockK:
							Console.Write("Block\n");
							//writerTree.WriteLine("<node>\"Block\"");
							flag = true;
							break;
						case StmtKind.RepeatK:
							Console.Write("Repeat\n");
							writerTree.WriteLine("<node>\"Repeat\"");
							writerSemanticTree.WriteLine("<node>\"Repeat\"");
							break;
						case StmtKind.AssignK:
							Console.Write("Assign to: {0}\n", tree.name);
							//writerTree.WriteLine("<node>\"AssignTo: {0}\"", tree.name);
							writerTree.WriteLine("<node>\"=\"");
							writerTree.WriteLine("<node>\"{0}\"</node>", tree.name);
							writerSemanticTree.WriteLine("<node>\"=\"");
							//writerSemanticTree.WriteLine("<node>\"{0}\"</node>", tree.name);
                           // writerTree.WriteLine("<node>\"Id: {0}\"", tree.name);
								BucketListRec l = symbolTable.st_lookup(tree.name);
								if (l == null) {
									writerSemanticTree.WriteLine("<node>\"Id: {0} Error: Variable not declared\"</node>", tree.name);
									Console.WriteLine("Id: {0} Error:Variable not declared", tree.name);
								} else {
									switch (l.tipo) {
										case "Int":
                                            writerSemanticTree.WriteLine("<node>\"{0} ({1}, {2}) \"</node>" , tree.name , l.tipo , tree.valInt);
											Console.WriteLine("{0} ({1}, {2}) ", tree.name, l.tipo, tree.valInt);
											break;
										case "Float":
                                            writerSemanticTree.WriteLine("<node>\"{0} ({1}, {2}) \"</node>" , tree.name , l.tipo , tree.valDouble);
											Console.WriteLine("{0} ({1}, {2}) ", tree.name, l.tipo, tree.valDouble);
											break;
										case "Bool":
                                            writerSemanticTree.WriteLine("<node>\"{0} ({1}, {2}) \"</node>" , tree.name , l.tipo , tree.valBool);
											Console.WriteLine("{0} ({1}, {2}) ", tree.name, l.tipo, tree.valBool);
											break;
									}
								}
							break;
						case StmtKind.ReadK:
							Console.Write("Read: {0}\n", tree.name);
							writerTree.WriteLine("<node>\"Read: {0}\"", tree.name);
							writerSemanticTree.WriteLine("<node>\"Read: {0}\"", tree.name);
							break;
						case StmtKind.WriteK:
							Console.Write("Write\n");
							writerTree.WriteLine("<node>\"Write\"");
							writerSemanticTree.WriteLine("<node>\"Write\"");
							break;
						default:
							Console.Write("Unknown ExpNode kind\n");
							writerInfo.WriteLine("Unknown ExpNode kind");
							break;
					}
				} else {
					if (tree.nodekind == NodeKind.ExpK) {
						switch (tree.expK) {
							case ExpKind.OpK:
								string op;
								switch (tree.op) {
									case Token_types.TKN_LTHAN:
										op = "&lt;";
										break; //&lt; is the character '<' in xml
									case Token_types.TKN_LETHAN:
										op = "&lt;=";
										break;
									case Token_types.TKN_GETHAN:
										op = "&gt;=";
										break; //&gt; is the character '>' in xml
									case Token_types.TKN_GTHAN:
										op = "&gt;";
										break;
									case Token_types.TKN_EQUAL:
										op = "==";
										break;
									case Token_types.TKN_NEQUAL:
										op = "!=";
										break;
									case Token_types.TKN_ADD:
										op = "+";
										break;
									case Token_types.TKN_MINUS:
										op = "-";
										break;
									case Token_types.TKN_PRODUCT:
										op = "*";
										break;
									case Token_types.TKN_DIVISION:
										op = "/";
										break;
									default:
										op = "Unknown operator";
										break; //assumed this error never occur
								}
                                if (op == "/" || op == "*" || op == "+" || op == "-" )
                                {
                                    if (tree.isIntType)
                                    {
                                        Console.Write("Op: {0} ({1})\n" , op, tree.valInt);
                                        writerTree.WriteLine("<node>\"Op: {0} ({1})\"" , op, tree.valInt);
                                        writerSemanticTree.WriteLine("<node>\"Op: {0} ({1})\"" , op, tree.valInt);
                                    }
                                    else
                                    {
                                        Console.Write("Op: {0} ({1})\n" , op, tree.valDouble);
                                        writerTree.WriteLine("<node>\"Op: {0} ({1})\"" , op, tree.valDouble);
                                        writerSemanticTree.WriteLine("<node>\"Op: {0} ({1})\"" , op, tree.valDouble);
                                    }
                                }
                                else
                                {
                                    Console.Write("Op: {0} ({1})\n" , op, tree.valBool);
                                    writerTree.WriteLine("<node>\"Op: {0}\"" , op);
                                    writerSemanticTree.WriteLine("<node>\"Op: {0} ({1})\"" , op, tree.valBool);
                                }

								break;
							case ExpKind.ConstK:
								if (tree.isIntType) {
									Console.Write("Const: {0}\n", tree.valInt);
                                    writerTree.WriteLine("<node>\"Const: {0}\"", tree.valInt);
									writerSemanticTree.WriteLine("<node>\"Const: {0}\"", tree.valInt);
								} else {
									Console.Write("Const: {0}\n", tree.valDouble);
									writerTree.WriteLine("<node>\"Const: {0}\"", tree.valDouble);
									writerSemanticTree.WriteLine("<node>\"Const: {0}\"", tree.valDouble);
								}
								break;
							case ExpKind.IdK:
								//Console.Write("Id: {0}\n", tree.name);
								writerTree.WriteLine("<node>\"Id: {0}\"", tree.name);
								BucketListRec l = symbolTable.st_lookup(tree.name);
								if (l == null) {
									writerSemanticTree.WriteLine("<node>\"Id: {0} Error: Variable not declared\"", tree.name);
									Console.WriteLine("Id: {0} Error:Variable not declared", tree.name);
								} else {
									switch (l.tipo) {
										case "Int":
											writerSemanticTree.WriteLine("<node>\"Id: {0} ({1}, {2}) \"", tree.name, l.tipo, l.valI);
											Console.WriteLine("Id: {0} ({1}, {2}) ", tree.name, l.tipo, l.valI);
											break;
										case "Float":
											writerSemanticTree.WriteLine("<node>\"Id: {0} ({1}, {2}) \"", tree.name, l.tipo, l.valF);
											Console.WriteLine("Id: {0} ({1}, {2}) ", tree.name, l.tipo, l.valF);
											break;
										case "Bool":
											writerSemanticTree.WriteLine("<node>\"Id: {0} ({1}, {2}) \"", tree.name, l.tipo, l.valB);
											Console.WriteLine("Id: {0} ({1}, {2}) ", tree.name, l.tipo, l.valB);
											break;
									}
								}
								break;
							default:
								Console.Write("Unknown ExpNode kind\n");
								writerInfo.WriteLine("Unknown ExpNode kind"); //assumed this error never occur
								break;
						}
					} else {
						if (tree.nodekind == NodeKind.DecK) {
							if (tree.name != null) { // if the variable was correctly defined should create its node
								string valTypeString;
								switch (tree.varType) {
									case Token_types.TKN_INT:
										valTypeString = "INT";
										break;
									case Token_types.TKN_FLOAT:
										valTypeString = "FLOAT";
										break;
									case Token_types.TKN_BOOL:
										valTypeString = "BOOL";
										break;
									default:
										valTypeString = "Unknown type";
										break;
								}
								Console.Write("{0}:{1}\n", valTypeString, tree.name);
								writerTree.WriteLine("<node>\"{0}:{1}\"", valTypeString, tree.name);
								writerSemanticTree.WriteLine("<node>\"{0}:{1}\"", valTypeString, tree.name);
							} else {
								flag = true;
							}
						} else {
							Console.WriteLine("Unknown node kind");
						}
					}
				}
				for (i = 0; i < 5; i++) {

					printBothTrees(tree.child[i]);
				}
				if (!flag) {
					writerTree.WriteLine("</node>");
					writerSemanticTree.WriteLine("</node>");
				}
				flag = false;
				tree = tree.sibling;
			}
			identno -= 2;
		}

		//***************************************************************************************************
		void cGen(TreeNode tree)
		{
			if (tree != null)
			{
				switch (tree.nodekind)
				{
					case NodeKind.StmtK:
						genStmt(tree);
						break;
					case NodeKind.ExpK:
						genExp(tree);
						break;
					default:
						break;
				}
				cGen(tree.sibling);
			}
		}
		//int cvalI;
		//double cvalF;
        bool enBloque = false;
		void genStmt(TreeNode tree)
		{
			TreeNode p1 , p2 /*, p3*/;
			BucketListRec l;
            
			switch (tree.stmtK)
			{
                case StmtKind.IfK:
                    cGen(tree.child[0]);
                    
                    break;
                case StmtKind.RepeatK:
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    cGen(p1);
                    cGen(p2);
                    break; /* repeat */
                case StmtKind.IterationK:
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    cGen(p1);
                    cGen(p2);
                    break;
                case StmtKind.BlockK:
                    enBloque = true;
                    cGen(tree.child[0]);
                    enBloque = false;
                    break;
				case StmtKind.AssignK:
                    if (!enBloque)
                    {
                        l = symbolTable.st_lookup(tree.name);
                        tipoActual = l.tipo;

                        cGen(tree.child[0]);

                        Console.WriteLine(tree.name + "<- " + tree.child[0].valInt + " " + tree.child[0].valDouble);
                        if (!tree.child[0].typeError || !tree.child[0].undeclaredError)
                        {

                            if ((tree.child[0].isIntType && (l.tipo.Equals("Int"))) || (tree.child[0].isIntType == false && l.tipo.Equals("Float")))
                            {
                                symbolTable.st_insert(tree.name , tree.nline , tree.child[0].valInt , tree.child[0].valDouble , tree.child
                                    [0].valBool , l.tipo , false , true, memloc++);
                                tree.valInt = tree.child[0].valInt;
                                tree.valDouble = tree.child[0].valDouble;
                            }
                            else
                            {
                                Console.WriteLine("Error: tipos diferentes. Variables {0} <- {1} {2} " , tree.name , l.valI , tree.child[0].valDouble);
                                tree.valInt = l.valI;
                                tree.valDouble = l.valF;
                                writerInfoSemantic.WriteLine("Error: tipos diferentes. Variables {0}: int={1}, float={2}" , tree.name , l.valI , tree.child[0].valDouble);
                            }
                        }
                    }
					break; /* assign_k */

				case StmtKind.ReadK:
					l = symbolTable.st_lookup(tree.name);
					if (l != null)
					{
						l.haveVal = false;
						l.valF = 0.0;
						l.valI = 0;
					}
					else
					{
						Console.WriteLine("Variable not declared:"+ tree.name);
                        writerInfoSemantic.WriteLine("Variable not declared:" + tree.name + "N.Linea: " + tree.nline);
					}
					break;
				default:
					break;
			}
		}

		void genExp(TreeNode tree)
		{
			BucketListRec l;
			TreeNode p1 , p2;
			
			switch (tree.expK)
			{
				case ExpKind.ConstK:
					break; /* ConstK */

				case ExpKind.IdK:
					
					l = symbolTable.st_lookup(tree.name);
                    
                        if (l.tipo=="Int")
                        {
                            tree.isIntType = true;
                        }
                        else tree.isIntType = false;
                    
					if (l != null)
					{
						if (l.haveVal)
						{
							tieneVal = true;
							if (tree.isIntType)
							{
								tree.valInt = l.valI;

							}
							else
							{
								tree.valDouble = l.valF;
							}
						}
						else
						{
							tieneVal = false;
						}
					}
					else
					{
                        tree.undeclaredError = true;
						Console.WriteLine("Variable not  declared: {0}", tree.name);
                        writerInfoSemantic.WriteLine("Variable not declared:" + tree.name + "N.Linea: " + tree.nline);
					}
					break; /* IdK */

				case ExpKind.OpK:
					p1 = tree.child[0];
					p2 = tree.child[1];
					cGen(p1);
					cGen(p2);
                    if (!p1.typeError || !p1.undeclaredError || !p1.typeError || !p1.undeclaredError)
                    {
                        if (!(p1.isIntType && p2.isIntType))
                        {
                           // tree.typeError = true;
                            //Console.WriteLine("Tipos diferentes");
                            tree.isIntType = false;
                            switch (tree.op)
                            {
                                case Token_types.TKN_ADD:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valDouble = tree.child[0].valInt + tree.child[1].valDouble;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble + tree.child[1].valInt;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_MINUS:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valDouble = tree.child[0].valInt - tree.child[1].valDouble;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble - tree.child[1].valInt;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_PRODUCT:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valDouble = tree.child[0].valInt * tree.child[1].valDouble;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble * tree.child[1].valInt;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_DIVISION:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valDouble = tree.child[0].valInt / tree.child[1].valDouble;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble / tree.child[1].valInt;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_LTHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt < tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble < tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_LETHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt <= tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble <= tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_GTHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt > tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble > tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_GETHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt >= tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble >= tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_EQUAL:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt == tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble == tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_NEQUAL:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt != tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble != tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            } /* case op */
                        }
                        else
                        {

                            switch (tree.op)
                            {
                                case Token_types.TKN_ADD:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valInt = tree.child[0].valInt + tree.child[1].valInt;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble + tree.child[1].valDouble;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_MINUS:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valInt = tree.child[0].valInt - tree.child[1].valInt;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble - tree.child[1].valDouble;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_PRODUCT:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valInt = tree.child[0].valInt * tree.child[1].valInt;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble * tree.child[1].valDouble;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_DIVISION:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            tree.valInt = tree.child[0].valInt / tree.child[1].valInt;
                                            break;
                                        case false:
                                            tree.valDouble = tree.child[0].valDouble / tree.child[1].valDouble;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_LTHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt < tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble < tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_LETHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt <= tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble <= tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_GTHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt > tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble > tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_GETHAN:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt >= tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble >= tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_EQUAL:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt == tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble == tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                case Token_types.TKN_NEQUAL:
                                    switch (p1.isIntType)
                                    {
                                        case true:
                                            if (tree.child[0].valInt != tree.child[1].valInt)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                        case false:
                                            if (tree.child[0].valDouble != tree.child[1].valDouble)
                                                tree.valBool = true;
                                            else
                                                tree.valBool = false;
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            } /* case op */
                        }//fin else
                    }//fin if
                    else //alguno de los dos hijos tiene alugun tipo de error
                    {
                        if (p1.typeError || p2.typeError) tree.typeError = true;
                        if (p1.undeclaredError || p2.undeclaredError) tree.undeclaredError = true;                        
                    }

					break; /* OpK */

				default:
					break;
			}

		}

		
		//***************************************************************************************************
		public SyntacticAnalizer() {
			Lexico lexResults = new Lexico("LexiconAnalisysTokens.txt");
			tokenList = lexResults.AnalizadorLexico();
			try {
				syntacticTree = new FileStream("SyntacticTree.xml", FileMode.Create, FileAccess.Write);
				writerTree = new StreamWriter(syntacticTree);
				infoSyntacticAnalisys = new FileStream("infoSyntacticAnalisys.txt", FileMode.Create, FileAccess.Write);
                infoSemanticAnalisys = new FileStream("infoSemanticAnalisys.txt" , FileMode.Create , FileAccess.Write);
				writerInfo = new StreamWriter(infoSyntacticAnalisys);
                writerInfoSemantic = new StreamWriter(infoSemanticAnalisys);
				writerTree.WriteLine("<?xml version=\"1.0\"?>");
				semanticTree = new FileStream("SemanticTree.xml" , FileMode.Create , FileAccess.Write);
				writerSemanticTree = new StreamWriter(semanticTree);
				writerSemanticTree.WriteLine("<?xml version=\"1.0\"?>");
				SyntacticTree = Parse(); //Parse function creates the Syntactic Tree
				cGen(SyntacticTree.child[1]);
				printBothTrees(SyntacticTree);
                CodeGenerator codeGenerator = new CodeGenerator(SyntacticTree, symbolTable);
                codeGenerator.executeGeneration();
			} catch(FileNotFoundException e){Console.WriteLine("File Not Found because " + e.Message);
			} catch(ArgumentException e){Console.WriteLine("Cannot read file because " + e.Message);
			} finally {
				writerTree.Close();
				writerSemanticTree.Close();
				writerInfo.Close();
                writerInfoSemantic.Close();
			}
		}
		
		public static void Main(string[] args) {
			SyntacticAnalizer analizer = new SyntacticAnalizer();
			analizer.symbolTable.printSymTab();
            
			//Console.ReadKey();
		}
	}
}