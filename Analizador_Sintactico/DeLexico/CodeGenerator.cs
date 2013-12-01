using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace NSSyntacticAnalizer
{
    class CodeGenerator
    {
        SymbolTable symTable;
        FileStream codeFile;
        StreamWriter writerCode;
        TreeNode syntacticTree;

        //Memory offset for tmps
        int tmpOffset = 0;
        //Tiny Machine registers
        int pc = 7 , mp = 6 , gp = 5 , ac = 0 , ac1 = 1;
        //Locations of instructions
        int emitLoc = 0 , highEmitLoc = 0;

        public CodeGenerator(TreeNode syntacticTree , SymbolTable st)
        {
            this.syntacticTree = syntacticTree;
            symTable = st;
        }

        public void executeGeneration()
        {
            try
            {
                codeFile = new FileStream("middleCode.tm" , FileMode.Create , FileAccess.Write);
                writerCode = new StreamWriter(codeFile);
                /* generate standard prelude */
                emitRM("LD",mp,0,ac,"load maxaddress from location 0");
                emitRM("ST",ac,0,ac,"clear location 0");
                /* generate code for TINY program */
                cGen(syntacticTree.child[1]);
                /* finish */                  
                emitRO("HALT",0,0,0,"");

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File Not Found because " + e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Cannot read file because " + e.Message);
            }
            finally
            {
                writerCode.Close();
            }
        }
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

        void genStmt(TreeNode tree)
        {
            TreeNode p1 , p2 , p3;
            int savedLoc1 , savedLoc2 , currentLoc;
            int loc;
            BucketListRec l;
            switch (tree.stmtK)
            {

                case StmtKind.IfK:
                    //if (TraceCode) emitComment("-> if") ;
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    p3 = tree.child[2];
                    /* generate code for test expression */
                    cGen(p1);
                    savedLoc1 = emitSkip(1);
                    //emitComment("if: jump to else belongs here");
                    /* recurse on then part */
                    cGen(p2);
                    savedLoc2 = emitSkip(1);
                    //emitComment("if: jump to end belongs here");
                    currentLoc = emitSkip(0);
                    emitBackup(savedLoc1);
                    emitRM_Abs("JEQ" , ac , currentLoc , "if: jmp to else");
                    emitRestore();
                    /* recurse on else part */
                    cGen(p3);
                    currentLoc = emitSkip(0);
                    emitBackup(savedLoc2);
                    emitRM_Abs("LDA" , pc , currentLoc , "jmp to end");
                    emitRestore();
                    //if (TraceCode)  emitComment("<- if") ;
                    break; /* if_k */

                case StmtKind.RepeatK:
                    //if (TraceCode) emitComment("-> repeat") ;
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    savedLoc1 = emitSkip(0);
                    //emitComment("repeat: jump after body comes back here");
                    /* generate code for body */
                    cGen(p1);
                    /* generate code for test */
                    cGen(p2);
                    emitRM_Abs("JEQ" , ac , savedLoc1 , "repeat: jmp back to body");
                    //if (TraceCode)  emitComment("<- repeat") ;
                    break; /* repeat */

                case StmtKind.IterationK:

                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    savedLoc1 = emitSkip(0);
                    //emitComment("repeat: jump after body comes back here");
                    /* generate code for body */
                    cGen(p1);
                    /* generate code for test */
                    cGen(p2);
                    emitRM_Abs("JEQ" , ac , savedLoc1 , "repeat: jmp back to body");
                    //if (TraceCode)  emitComment("<- repeat") ;
                    break; /* iteratiom */

                case StmtKind.BlockK:
                    p1 = tree.child[0];
                    cGen(p1);
                    break; /* repeat */
                case StmtKind.AssignK:
                    //if (TraceCode) emitComment("-> assign") ;
                    /* generate code for rhs */
                    cGen(tree.child[0]);
                    /* now store value */
                    l = symTable.st_lookup(tree.name);
                    if (l == null)
                        return;
                    loc = l.memloc;
                    emitRM("ST" , ac , loc , gp , "assign: store value");
                    //if (TraceCode)  emitComment("<- assign") ;
                    break; /* assign_k */

                case StmtKind.ReadK:
                    emitRO("IN" , ac , 0 , 0 , "read integer value");

                    l = symTable.st_lookup(tree.name);
                    loc = l.memloc;
                    emitRM("ST" , ac , loc , gp , "read: store value");
                    break;
                case StmtKind.WriteK:
                    /* generate code for expression to write */
                    cGen(tree.child[0]);
                    /* now output it */
                    emitRO("OUT" , ac , 0 , 0 , "write ac");
                    break;
                default:
                    break;
            }
        } /* genStmt */

        void genExp(TreeNode tree)
        {
            BucketListRec l;
            TreeNode p1 , p2;
            int loc;
            switch (tree.expK)
            {
                case ExpKind.ConstK:
                    // if (TraceCode) emitComment("-> Const") ;
                    /* gen code to load integer or float constant using LDC, we also sent the flag that allow us
                        to know if the constant in the node is a integer or not */
                    emitRM("LDC" , ac , tree.valInt , tree.valDouble , 0 , "load const" , tree.isIntType);
                    //if (TraceCode)  emitComment("<- Const") ;
                    break; /* ConstK */

                case ExpKind.IdK:
                    //if (TraceCode) emitComment("-> Id") ;
                    l = symTable.st_lookup(tree.name);
                    if (l != null)
                    {
                        loc = l.memloc;
                        emitRM("LD" , ac , loc , gp , "load id value");
                        //if (TraceCode)  emitComment("<- Id") ;
                    }
                    break; /* IdK */
            
                case ExpKind.OpK:
                    //if (TraceCode) emitComment("-> Op") ;
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    /* gen code for ac = left arg */
                    cGen(p1);
                    /* gen code to push left operand */
                    emitRM("ST" , ac , tmpOffset-- , mp , "op: push left");
                    /* gen code for ac = right operand */
                    cGen(p2);
                    /* now load left operand */
                    emitRM("LD" , ac1 , ++tmpOffset , mp , "op: load left");

                    switch (tree.op)
                    {
                        case Token_types.TKN_ADD:

                            emitRO("ADD" , ac , ac1 , ac , "op +");
                            break;
                        case Token_types.TKN_MINUS:
                            emitRO("SUB" , ac , ac1 , ac , "op -");


                            break;
                        case Token_types.TKN_PRODUCT:
                            emitRO("MUL" , ac , ac1 , ac , "op *");


                            break;
                        case Token_types.TKN_DIVISION:
                            emitRO("DIV" , ac , ac1 , ac , "op /");

                            break;
                        case Token_types.TKN_LTHAN:
                            emitRO("SUB" , ac , ac1 , ac , "op <");
                            emitRM("JLT" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        case Token_types.TKN_LETHAN:
                            emitRO("SUB" , ac , ac1 , ac , "op <");
                            emitRM("JLE" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        case Token_types.TKN_GTHAN:
                            emitRO("SUB" , ac , ac1 , ac , "op <");
                            emitRM("JGT" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        case Token_types.TKN_GETHAN:
                            emitRO("SUB" , ac , ac1 , ac , "op <");
                            emitRM("JGE" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        case Token_types.TKN_EQUAL:
                            emitRO("SUB" , ac , ac1 , ac , "op ==");
                            emitRM("JEQ" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        case Token_types.TKN_NEQUAL:
                            emitRO("SUB" , ac , ac1 , ac , "op ==");
                            emitRM("JNE" , ac , 2 , pc , "br if true");
                            emitRM("LDC" , ac , 0 , ac , "false case");
                            emitRM("LDA" , pc , 1 , pc , "unconditional jmp");
                            emitRM("LDC" , ac , 1 , ac , "true case");
                            break;
                        default:
                            //emitComment("BUG: Unknown operator");
                            break;
                    } /* case op */


                    //if (TraceCode)  emitComment("<- Op") ;
                    break; /* OpK */

                default:
                    break;
            }

        }
        //Methods to print instructions in file
        void emitRO(string op , int r , int s , int t , string c)
        {
            writerCode.WriteLine(" {0}:    {1}  {2},{3},{4}" , emitLoc++ , op , r , s , t);
            if (highEmitLoc < emitLoc) highEmitLoc = emitLoc;
        }
        //emitRM("LDC",ac,tree.val,0,"load const");
        void emitRM(string op , int r , int d , int s , string c)
        {
            writerCode.WriteLine(" {0}:    {1}  {2},{3}({4})" , emitLoc++ , op , r , d , s);
            if (highEmitLoc < emitLoc) highEmitLoc = emitLoc;
        }
        void emitRM(string op , int r , int valInt , double valDouble , int s , string c , bool isInt)
        {
            if (isInt)
            { //here we make difference between an integer and a float value
                writerCode.WriteLine(" {0}:    {1}  {2},{3}({4})" , emitLoc++ , op , r , valInt , s);
            }
            else
            {
                writerCode.WriteLine(" {0}:    {1}  {2},{3}({4})" , emitLoc++ , op , r , valDouble , s);
            }
            if (highEmitLoc < emitLoc) highEmitLoc = emitLoc;
        }

        int emitSkip(int howMany)
        {
            int i = emitLoc;
            emitLoc += howMany;
            if (highEmitLoc < emitLoc) highEmitLoc = emitLoc;
            return i;
        }

        void emitBackup(int loc)
        {
            emitLoc = loc;
        }

        void emitRestore()
        {
            emitLoc = highEmitLoc;
        }

        void emitRM_Abs(string op , int r , int a , string c)
        {
            writerCode.WriteLine(" {0}:    {1}  {2},{3}({4})" , emitLoc , op , r , a - (emitLoc + 1) , pc);
            ++emitLoc;

            if (highEmitLoc < emitLoc) highEmitLoc = emitLoc;
        }
    }


}


