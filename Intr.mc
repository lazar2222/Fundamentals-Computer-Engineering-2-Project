//Intr.mc
INTR: brif !INTR then this

brif !avail then FETCH
ldBRU,incSP " BRU=UEXT,SP++
ldMAR,mxMAR2,incSP,ldMDR,mxMDR0,mxMDR2 " MAR=SP,SP++,MDR=PC7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,incSP,ldMDR,mxMDR1,mxMDR2 " MAR=SP,SP++,MDR=PC15..8
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,incSP,ldMDR,mxMDR0,mxMDR1 " MAR=SP,SP++,MDR=PSW7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,incSP,ldMDR,mxMDR2 " MAR=SP,SP++,MDR=PSW15..8
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,incSP,ldMDR,mxMDR0 " MAR=SP,SP++,MDR=A7..0
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR2,ldMDR,mxMDR1 " MAR=SP,MDR=A15..8
wrMEM brif !FCBUS then this " MEM[MAR]=MDR
ldMAR,mxMAR0,mxMAR1,mxOFFS0,mxOFFS1,mxOFS0,mxOFFS2 " MAR=IVTP+BRU2..00
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0,incMAR " B7..0=MDR,MAR++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB15..8,mxB0 " B15..8=MDR
ldPC,mxPC0 " PC=B

FETCH: clINTR,stFETCH br INTR " INTR=0,FETCH=1