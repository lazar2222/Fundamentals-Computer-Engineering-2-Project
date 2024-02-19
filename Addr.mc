//Addr.mc
ADDR: brif !ADDR then this

brcase immed,memdir,regdir,memind,preincr,postdec,regindpom then IMMED,MEMDIR,REGDIR,MEMIND,PREINCR,POSTDEC,REGINDPOM

IMMED: ldB15..8,ldB7..0 br EXEC " B=IR15..0

MEMDIR: ldMAR,mxMAR0 br LOAD " MAR=IR15..0

REGDIR: brif STORE then EXEC
ldB15..8,ldB7..0,mxB1 br EXEC " B=GPR[IR3..1]

MEMIND: ldMAR,mxMAR0 " MAR=IR15..0
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0,incMAR " B7..0=MDR,MAR++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB15..8,mxB0 " B15..8=MDR
ldMAR,mxMAR1 br LOAD " MAR=B

PREINCR: mxOFS0,ldMAR,mxMAR0,mxMAR1,ldGPR br LOAD " MAR=GPR[IR3..1]+2,GPR[IR3..1]=GPR[IR3..1]+2

POSTDEC: ldMAR,mxMAR0,mxMAR1 " MAR=GPR[IR3..1]
mxOFS1,ldGPR br LOAD " GPR[IR3..1]=GPR[IR3..1]-2

REGINDPOM: mxOFS0,mxOFS1,ldMAR,mxMAR0,mxMAR1 br LOAD " MAR=GPR[IR3..1]+IR15..15 IR15..8

LOAD: brif STORE||STRLEN then EXEC
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB7..0,mxB0,incMAR " B7..0=MDR,MAR++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldB15..8,mxB0 " B15..8=MDR

EXEC: clADDR,stEXEC br ADDR " ADDR=0,EXEC=1