//Exec.mc
EXEC: brif !EXEC then this

brcase halt,rts,pushpsw,poppsw,push,pop||rti,inte||intd,rjmp,cjmp,jmp,ld,st,jsr,tst,xor,strlen,add then HALT,RTS,PUSHPSW,POPPSW,PUSH,POP,INT,RJMP,CJMP,JMP,LD,ST,JSR,TST,XOR,STRLEN,ADD

HALT: ldPSWSTART br INTR " PSWSTART=0

POP: ldMAR,mxMAR2,decSP " MAR=SP,SP--
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldMAR,mxMAR2,decSP,ldB15..8,mxB0 " MAR=SP,SP--,B15..8=MDR
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0 " B7..0=MDR

LD: ldA " A=B
ldPSWZ,ldPSWN brif LD||POP then INTR " PSWZ=Z,PSWN=N

POPPSW: ldMAR,mxMAR2,decSP " MAR=SP,SP--
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldMAR,mxMAR2,decSP,ldB15..8,mxB0 " MAR=SP,SP--,B15..8=MDR
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0 " B7..0=MDR
ldPSWALL brif POPPSW then INTR " PSW=B

RTS: ldMAR,mxMAR2,decSP " MAR=SP,SP--
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldMAR,mxMAR2,decSP,ldB15..8,mxB0 " MAR=SP,B15..8=MDR
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0 " B7..0=MDR
ldPC,mxPC0 br INTR " PC=B

PUSH: incSP " SP++
ldMAR,mxMAR2,incSP " MAR=SP,SP++

ST: brif regdir then REG
ldMDR,mxMDR0 " MDR=A7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMDR,mxMDR1,incMAR " MDR=A15..8,MAR++
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
br INTR

PUSHPSW: incSP " SP++
ldMAR,mxMAR2,incSP,ldMDR,mxMDR0,mxMDR1 " MAR=SP,SP++,MDR=PSW7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,ldMDR,mxMDR2 " MAR=SP,MDR=PSW15..8
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
br INTR

JSR: incSP " SP++
ldMAR,mxMAR2,incSP,ldMDR,mxMDR0,mxMDR2 " MAR=SP,SP++,MDR=PC7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,ldMDR,mxMDR1,mxMDR2 " MAR=SP,MDR=PC15..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldPC,mxPC0 br INTR " PC=B

JMP: ldPC,mxPC0,mxPC1 br INTR " PC=IR23..8

REG: ldGPR,mxOFSS1 br INTR " GPR[IR3..1]=A

INT: ldPSWI br INTR " PSWI=INTE?1:0

RJMP: brif !cond then INTR
ldPC,mxPC1,mxOFS2,mxOFSS0 br INTR " PC=PC+IR23..23IR23..16

TST: ldPSWZ,ldPSWN,aluAND br INTR " PSWZ=Z,PSWN=N,ALU=A AND B

STRLEN: clA,ldB,mxB0,mxB1 " A=0,B=1
GIT: rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
brif MDR0 then FLAG
ldA,aluADD,incMAR br GIT " A=A+B,MAR++

XOR: ldA,aluXOR br FLAG " A=A XOR B

ADD: ldA,aluADD,ldPSWC,ldPSWV " A=A+B,PSWC=C,PSWV=V

FLAG: ldPSWZ,ldPSWN br INTR " PSWZ=Z,PSWN=N

CJMP: brif cond then JMP

INTR: clEXEC,stINTR br EXEC " EXEC=0,INTR=1