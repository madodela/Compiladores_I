using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSSyntacticAnalizer
{
    public class BucketListRec
    {
        public string name;
        public LineListRec lines;
        public string tipo;
        public int valI;
        public bool isInt;
        public double valF;
        public bool valB;
        public BucketListRec next;
        public BucketListRec(string nom  , BucketListRec next , LineListRec lines , int valI , double valF , bool valB , string tipo)
        {
            this.name = nom;
            this.next = next;
            this.lines = lines;
            this.valI = valI;
            this.valF = valF;
            this.valB = valB;
            this.tipo = tipo;
        }
    }

    public class LineListRec
    {
        public int lineno;
        public LineListRec next;
        public LineListRec(int linenu)
        {
            this.next = null;
            this.lineno = linenu;
        }
    }

    public class SymbolTable
    {

        const int SHIFT = 4;
        const int SIZE = 211;
        public BucketListRec[] hashTable = new BucketListRec[211];


        public int hash(string key)
        {
            int temp = 0;
            int i = 0;
            char[] key2 = key.ToCharArray();
            while (i < key2.Length)
            {
                temp = ((temp << SHIFT) + key2[i]) % SIZE;
                ++i;
            }
            Console.Write("<<{0}" , temp);
            Console.WriteLine();
            return temp;
        }



        public void st_insert(string name , int linenu , int valI , double valF ,bool valB, string tipo , bool isDec )
        { 
            int h = hash(name);
            BucketListRec l = this.hashTable[h];
            while ((l != null) && !string.Equals(name , l.name))
                l = l.next;
            if (l == null) /* variable que todavía no está en la tabla */
            {
                LineListRec list = new LineListRec(linenu);
                l = new BucketListRec(name , this.hashTable[h] , list , valI , valF , valB, tipo);
                Console.WriteLine("Nombre: {0}" , l.name );
                this.hashTable[h] = l;
            }
            else /* está en la tabla, de modo que sólo se agrega el número de línea*/
            {
                Console.WriteLine("Variable ya en tabla");
                Console.WriteLine("Nombre: {0}" , l.name );
                LineListRec t = l.lines;
                if (string.Equals(l.tipo , "Int"))
                    l.valI = valI;
                else if (string.Equals(l.tipo , "Float"))
                    l.valF = valF;
                else
                    l.valB = valB;
                while (t.next != null) t = t.next;
                t.next = new LineListRec(linenu);
            }
        } /* de st_insert */

        public int st_lookup(string name)
        {
            int h = hash(name);
            BucketListRec l = this.hashTable[h];
            while ((l != null) && !string.Equals(name , l.name))
                l = l.next;
            if (l == null) return -1;
            else return 1;
        }

        public void printSymTab()
        {
            FileStream tableSymbolFile = new FileStream("tableSymbolFile.txt" , FileMode.Create , FileAccess.Write);
            StreamWriter info = new StreamWriter(tableSymbolFile);
            int i;
            Console.WriteLine("Variable Name  Tipo  Valor  Line Numbers");

            for (i = 0 ; i < SIZE ; ++i)
            {
                if (this.hashTable[i] != null)
                {
                    BucketListRec l = this.hashTable[i];
                    while (l != null)
                    {
                        LineListRec t = l.lines;
                        Console.Write("{0}" , l.name);
                        info.Write("{0}" , l.name);
                        Console.Write("".PadLeft(15 - l.name.Length) + "{0}" , l.tipo);
                        info.Write("\t{0}",l.tipo);
                        if (l.isInt)
                        {
                            Console.Write("".PadLeft(6 - l.tipo.Length) + "{0}" , l.valI);
                            info.Write("\t{0}" , l.valI);
                        }
                        else
                        {
                            Console.Write("".PadLeft(6 - l.tipo.Length) + "{0}" , l.valF);
                            info.Write("\t{0}" , l.valF);
                            // Console.Write("".PadLeft(7) + "{0}" , l.memloc);
                        }
                        while (t != null)
                        {
                            Console.Write("".PadLeft(10) + "{0}" , t.lineno);
                            info.Write("\t{0}" , t.lineno);
                            t = t.next;
                        }
                        Console.Write("\n"); 
                        info.WriteLine("");

                        l = l.next;
                    }
                }
            }
            info.Close();
        } /* de printSymTab */

    }
}
