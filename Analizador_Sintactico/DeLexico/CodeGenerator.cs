using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSSyntacticAnalizer
{
    class CodeGenerator
    {
        SymbolTable symTable;
        public CodeGenerator(SymbolTable st)
        {
            symTable = st;
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
                    //savedLoc1 = emitSkip(1) ;
                    //emitComment("if: jump to else belongs here");
                    /* recurse on then part */
                    cGen(p2);
                    //savedLoc2 = emitSkip(1) ;
                    //emitComment("if: jump to end belongs here");
                    //currentLoc = emitSkip(0) ;
                    //emitBackup(savedLoc1) ;
                    //emitRM_Abs("JEQ",ac,currentLoc,"if: jmp to else");
                    //emitRestore() ;
                    /* recurse on else part */
                    cGen(p3);
                    //currentLoc = emitSkip(0) ;
                    //emitBackup(savedLoc2) ;
                    //emitRM_Abs("LDA",pc,currentLoc,"jmp to end") ;
                    //emitRestore() ;
                    //if (TraceCode)  emitComment("<- if") ;
                    break; /* if_k */

                case StmtKind.RepeatK:
                    //if (TraceCode) emitComment("-> repeat") ;
                    p1 = tree.child[0];
                    p2 = tree.child[1];
                    //savedLoc1 = emitSkip(0);
                    //emitComment("repeat: jump after body comes back here");
                    /* generate code for body */
                    cGen(p1);
                    /* generate code for test */
                    cGen(p2);
                    //emitRM_Abs("JEQ",ac,savedLoc1,"repeat: jmp back to body");
                    //if (TraceCode)  emitComment("<- repeat") ;
                    break; /* repeat */

                case StmtKind.AssignK:
                    //if (TraceCode) emitComment("-> assign") ;
                    /* generate code for rhs */
                    cGen(tree.child[0]);
                    /* now store value */
                    l = symTable.st_lookup(tree.name);
                    BucketListRec lchild = symTable.st_lookup(tree.child[0].name);
                    if (string.Equals(l.name , lchild.name))
                    {
                        symTable.st_insert(tree.name , tree.nline , tree.child[0].valInt , tree.child[0].valDouble , tree.child[0].valBool , l.tipo , false , true);
                    }
                    else
                    {
                        Console.WriteLine("Error de tipo");
                    }
                    //emitRM("ST",ac,loc,gp,"assign: store value");
                    //if (TraceCode)  emitComment("<- assign") ;
                    break; /* assign_k */

                case StmtKind.ReadK:
                    //emitRO("IN",ac,0,0,"read integer value");
                    l = symTable.st_lookup(tree.name);
                    //emitRM("ST",ac,loc,gp,"read: store value");
                    break;
                case StmtKind.WriteK:
                    /* generate code for expression to write */
                    cGen(tree.child[0]);
                    /* now output it */
                    //emitRO("OUT",ac,0,0,"write ac");
                    break;
                default:
                    break;
            }
        } /* genStmt */

        void genExp(TreeNode tree)
        { 
          BucketListRec l;
          TreeNode  p1,  p2;

          switch (tree.expK)
          {
              case ExpKind.ConstK:
                  // if (TraceCode) emitComment("-> Const") ;
                  /* gen code to load integer constant using LDC */
                  //emitRM("LDC",ac,tree->attr.val,0,"load const");
                  //if (TraceCode)  emitComment("<- Const") ;
                  break; /* ConstK */

              case ExpKind.IdK:
                  //if (TraceCode) emitComment("-> Id") ;
                  l = symTable.st_lookup(tree.name);
                  //emitRM("LD",ac,loc,gp,"load id value");
                  //if (TraceCode)  emitComment("<- Id") ;
                  break; /* IdK */

              case ExpKind.OpK:
                  //if (TraceCode) emitComment("-> Op") ;
                  p1 = tree.child[0];
                  p2 = tree.child[1];
                  /* gen code for ac = left arg */
                  cGen(p1);
                  /* gen code to push left operand */
                  //emitRM("ST",ac,tmpOffset--,mp,"op: push left");
                  /* gen code for ac = right operand */
                  cGen(p2);
                  /* now load left operand */
                  //emitRM("LD",ac1,++tmpOffset,mp,"op: load left");
                  BucketListRec f , g , h;
                  f = symTable.st_lookup(tree.name);
                  g = symTable.st_lookup(tree.child[0].name);
                  h = symTable.st_lookup(tree.child[1].name);
                  if (!string.Equals(f.tipo , g.tipo) && !string.Equals(h.tipo , g.tipo))
                  {
                      Console.WriteLine("Error de tipos");
                  }
                  else
                  {
                      switch (tree.op)
                      {
                          case Token_types.TKN_ADD:
                              switch (f.tipo)
                              {
                                  case "Int":
                                      f.valI = g.valI + h.valI;
                                      break;
                                  case "Float":
                                      f.valF = g.valF + h.valF;
                                      break;
                              }
                              //emitRO("ADD",ac,ac1,ac,"op +");
                              break;
                          case Token_types.TKN_MINUS:
                              //emitRO("SUB",ac,ac1,ac,"op -");
                              switch (f.tipo)
                              {
                                  case "Int":
                                      f.valI = g.valI - h.valI;
                                      break;
                                  case "Float":
                                      f.valF = g.valF - h.valF;
                                      break;
                              }
                              break;
                          case Token_types.TKN_PRODUCT:
                              //emitRO("MUL",ac,ac1,ac,"op *");
                              switch (f.tipo)
                              {
                                  case "Int":
                                      f.valI = g.valI * h.valI;
                                      break;
                                  case "Float":
                                      f.valF = g.valF * h.valF;
                                      break;
                              }
                              break;
                          case Token_types.TKN_DIVISION:
                              //emitRO("DIV",ac,ac1,ac,"op /");
                              switch (f.tipo)
                              {
                                  case "Int":
                                      f.valI = g.valI / h.valI;
                                      break;
                                  case "Float":
                                      f.valF = g.valF / h.valF;
                                      break;
                              }
                              break;
                          case Token_types.TKN_LTHAN:
                              //emitRO("SUB",ac,ac1,ac,"op <") ;
                              //emitRM("JLT",ac,2,pc,"br if true") ;
                              //emitRM("LDC",ac,0,ac,"false case") ;
                              //emitRM("LDA",pc,1,pc,"unconditional jmp") ;
                              //emitRM("LDC",ac,1,ac,"true case") ;
                              break;
                          case Token_types.TKN_EQUAL:
                              //emitRO("SUB",ac,ac1,ac,"op ==") ;
                              //emitRM("JEQ",ac,2,pc,"br if true");
                              //emitRM("LDC",ac,0,ac,"false case") ;
                              //emitRM("LDA",pc,1,pc,"unconditional jmp") ;
                              //emitRM("LDC",ac,1,ac,"true case") ;
                              break;
                          default:
                              //emitComment("BUG: Unknown operator");
                              break;
                      } /* case op */
                  }

                  //if (TraceCode)  emitComment("<- Op") ;
                  break; /* OpK */

              default:
                  break;
          }
          
        }
    }
}

