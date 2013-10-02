/*
 * Created by SharpDevelop.
 * User: Loli Delgado
 * Date: 01/10/2013
 * Time: 11:48 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TablaSimbolos
{
	public class BucketListRec {
		public string name;
		public LineListRec lines;
		public int memloc ; /* localidad de memoria para variable */
		public BucketListRec next;
		public BucketListRec(string nom, int loc, BucketListRec next, LineListRec lines ) {//name, loc, this.hashTable[h],new LineListRec(linenu)
			this.name = nom;
			this.memloc = loc;
			this.next = next;
			this.lines = lines;
		}
	}
	
	public class LineListRec {
		public int lineno;
		public LineListRec next;
		public LineListRec(int linenu) {
			this.next = null;
			this.lineno = linenu;
		}
	}
	
	public class SymbolTable{
		
		const int SHIFT = 4;
		const int SIZE = 211;
		public BucketListRec [] hashTable = new BucketListRec[211];
		
		
		public int hash ( string key ) { // ok
			int temp = 0;
			int i = 0;
			char [] key2 = key.ToCharArray();
			while (i<key2.Length) {
				temp = ((temp << SHIFT) + key2[i]) % SIZE;
				++i;
			}
			Console.Write("<<{0}",temp);
			Console.WriteLine();
			return temp;
		}
		
		

		public void st_insert( string name, int linenu, int loc ) {
			//Console.WriteLine(" LLegan Nombre: {0}, Linea: {1}, Loc: {2}", name, linenu, loc);//lo que se debe guardar llega bien
			int h = hash(name); 
			//Console.WriteLine("h = {0}",h); // el numero hash esta bien calculado
			BucketListRec l =  this.hashTable[h];
			//Console.WriteLine("Hashtable[{0}] = {1}",h,this.hashTable[h]);
			
			while ((l != null) && string.Equals(name,l.name)){
				l = l.next;
				if (l == null) /* variable que todavía no está en la tabla */
				{ //bucket list lleva.. nombre, memloc, next, LineListREc
					//linelistrec lleva... lineno, next = null
					l = new BucketListRec(name, loc, this.hashTable[h],new LineListRec(linenu));
					Console.WriteLine("Nombre: {0}, Localidad: {1}",l.name, l.memloc);
					//l.name = name;
					//l.lines = new LineListRec();
					//l.memloc = loc;
					//l.lines.lineno = lineno;
					//l.lines.next = null;
					//l.next = this.hashTable[h];
					this.hashTable[h] = l;
				}
				else /* está en la tabla, de modo que sólo se agrega el número de línea*/
				{ 
					Console.WriteLine("Nombre: {0}, Localidad: {1}",l.name, l.memloc);
					LineListRec t = l.lines;
					while (t.next != null) t = t.next;
					t.next = new LineListRec(linenu);
					//t.next = (LineList) malloc(sizeof(struct LineListRec));
					/*t.next.lineno = lineno;
					t.next.next = null;*/
				}
			}
		} /* de st_insert */
		
		public int st_lookup ( string name ) {
			int h = hash(name);
			BucketListRec l =  this.hashTable[h];
			while ((l != null) && string.Equals(name,l.name))
				l = l.next;
			if (l == null) return -1;
			else return l.memloc;
		}

		public void printSymTab() {
			
			int i;
			Console.WriteLine("Variable Name  Location   Line Numbers");
			Console.Write("-------------  --------   ------------\n");
			for (i = 0; i < SIZE; ++i) {
				if (this.hashTable[i] != null) {
					BucketListRec l = this.hashTable[i];
					while (l != null) {
						LineListRec t = l.lines;
						Console.Write("{0}",l.name);
						Console.Write("{0}",l.memloc);
						while (t != null) {
							Console.Write("{0}",t.lineno);
							t = t.next;
						}
						Console.Write("\n");
						l = l.next;
					}
				}
			}
		} /* de printSymTab */
		
	}
	
	class Program {
		
		public static void Main(string[] args) {
			SymbolTable tabla = new SymbolTable();
			Console.Write("         METIENDO VARIABLES \n\n");
			Console.Write("________________________________________________\n");
			tabla.st_insert( "Uno",1, 0 );
			tabla.st_insert( "Dos",2, 1 );
			tabla.st_insert( "Uno",2, 0 );
			tabla.st_insert( "Dos",2, 0 );
			tabla.st_insert( "Dos",2, 7 );
			tabla.st_insert( "X",5, 2 );
			tabla.st_insert( "X",6, 0 );
			tabla.st_insert( "X",6, 0 );
			Console.Write("           BUSCANDO VARIABLES \n\n");
			Console.Write("________________________________________________\n");
			//int st_lookup ( char * name );
			int a = 0;
			a = tabla.st_lookup ( "hola" );
			if (a < 0)
				Console.Write ("variable no encontrada hola \n");
			else
				Console.Write("Variable en TABLA DE SIMBOLOS \n");
			a=tabla.st_lookup ( "Uno" );
			if (a<0)
				Console.Write ("variable no encontrada Uno \n");
			else
				Console.Write("Variable en TABLA DE SIMBOLOS <<Uno>> en registro %d \n", a);
			
			a = tabla.st_lookup ( "Dos" );
			if (a<0)
				Console.Write ("variable no encontrada Dos \n");
			else
				Console.Write("Variable en TABLA DE SIMBOLOS <<Dos>> en registro %d \n\n\n", a);

			Console.Write("       TABLA DE SIMBOLOS  \n");
			Console.Write("________________________________________________\n");
			//void printSymTab(FILE * listing);
			tabla.printSymTab();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}