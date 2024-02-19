//Fetch.mc
FETCH: brif !(PSWST&&FETCH) then this

ldMAR,incPC " MAR=PC,PC++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldIR31..24 " IR31..24=MDR

brif grop then FETCH
brif nadr then EXEC

ldMAR,incPC " MAR=PC,PC++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldIR23..16 brif jmp2 then EXEC " IR23..16=MDR

brif gradr then FETCH
brif adr2 then ADDR

ldMAR,incPC " MAR=PC,PC++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldIR15..8 brif jmp3 then EXEC " IR15..8=MDR

brif adr3 then ADDR

ldMAR,incPC " MAR=PC,PC++
rdMEM,ldMDR brif !FCBUS then this " MDR=MEM[MAR]
ldIR7..0 " IR7..0=MDR

ADDR: clFETCH,stADDR br FETCH " FETCH=0,ADDR=1
EXEC: clFETCH,stEXEC br FETCH " FETCH=0,EXEC=1