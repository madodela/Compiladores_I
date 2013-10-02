/*
 * Created by SharpDevelop.
 * User: Loli Delgado
 * Date: 01/10/2013
 * Time: 11:48 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace TablaSimbolos
{
	public class BucketListRec {
		public string name;
		public LineListRec lines;
		public int memloc ; /* localidad de memoria para variable */
        public string tipo;
        public int valI;
        public bool isInt;
        public double valF;
		public BucketListRec next;
        public BucketListRec(string nom , int loc , BucketListRec next , LineListRec lines , int valI , double valF , bool isInt, string tipo)
        {
			this.name = nom;
			this.memloc = loc;
			this.next = next;
			this.lines = lines;
                this.valI = valI;
                this.valF = valF;
                this.isInt = isInt;
                this.tipo = tipo;
		}
	}
	
	public class LineListRec {//ok
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
		
		

		public void st_insert( string name, int linenu, int loc , int valI, double valF, bool isInt, bool isDec, string tipo ) {//ok 
			int h = hash(name); 
			BucketListRec l =  this.hashTable[h];
			while ((l != null) && !string.Equals(name,l.name))
				l = l.next;
				if (l == null) /* variable que todavía no está en la tabla */
				{
                    LineListRec list = new LineListRec(linenu);
					l = new BucketListRec(name, loc, this.hashTable[h],list, valI, valF, isInt, tipo);
					Console.WriteLine("Nombre: {0}, Localidad: {1}",l.name, l.memloc);
					this.hashTable[h] = l;
				}
				else /* está en la tabla, de modo que sólo se agrega el número de línea*/
                {
                    Console.WriteLine("Variable ya en tabla");
					Console.WriteLine("Nombre: {0}, Localidad: {1}",l.name, l.memloc);
					LineListRec t = l.lines;
                    if (l.isInt)
                        l.valI = valI;
                    else
                        l.valF = valF;
					while (t.next != null) t = t.next;
					t.next = new LineListRec(linenu);
				}		
		} /* de st_insert */
		
		public int st_lookup ( string name ) {
			int h = hash(name);
			BucketListRec l =  this.hashTable[h];
			while ((l != null) && !string.Equals(name,l.name))
				l = l.next;
			if (l == null) return -1;
			else return l.memloc;
		}

		public void printSymTab() {		
			int i;
			Console.WriteLine("Variable Name  Tipo  Valor  Location   Line Numbers");
			
			for (i = 0; i < SIZE; ++i) {
				if (this.hashTable[i] != null) {
					BucketListRec l = this.hashTable[i];
					while (l != null) {
						LineListRec t = l.lines;
						Console.Write("{0}",l.name);
                        Console.Write("".PadLeft(15 - l.name.Length) + "{0}" , l.tipo);
                        if(l.isInt)
                            Console.Write("".PadLeft(6 - l.tipo.Length) + "{0}" , l.valI);
                        else
                            Console.Write("".PadLeft(6 - l.tipo.Length) + "{0}" , l.valF);
						Console.Write("".PadLeft(7)+"{0}",l.memloc);
						while (t != null) {
                            Console.Write("".PadLeft(10) + "{0}" , t.lineno);
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
			tabla.st_insert( "Uno",1, 0 , 0,4.2,false,true,"float");            
			tabla.st_insert( "Dos",2, 1, 2, 3.3, true,true, "int" );
			tabla.st_insert( "Uno",2, 0,2,1.1,false,false,null);
            tabla.st_insert("Dos" , 2 , 0 , 2 , 3.3 , true , false , null);
            tabla.st_insert("Dos" , 2 , 7 , 2 , 3.3 , true , false , null);
            tabla.st_insert("X" , 5 , 2 , 4 , 3.3 , true , true , "int");
            tabla.st_insert("X" , 6 , 0 , 2 , 3.3 , true , false , null);
            tabla.st_insert("X" , 6 , 0 , 1000 , 3.3 , true , false , null);
			Console.Write("\n           BUSCANDO VARIABLES \n\n");
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
				Console.Write("Variable en TABLA DE SIMBOLOS <<Uno>> en registro {0} \n", a);
			
			a = tabla.st_lookup ( "Dos" );
			if (a<0)
				Console.Write ("variable no encontrada Dos \n");
			else
				Console.Write("Variable en TABLA DE SIMBOLOS <<Dos>> en registro {0} \n\n\n", a);

			Console.Write("       TABLA DE SIMBOLOS  \n");
			Console.Write("________________________________________________\n");
			//void printSymTab(FILE * listing);
			tabla.printSymTab();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}