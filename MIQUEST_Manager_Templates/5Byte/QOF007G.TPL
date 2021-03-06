*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,QOF007G,Subset QOF-CVD
*QRY_ORDER,513,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR CON003G
SUBSET QOF007G TEMP
FROM JOURNALS (LATEST FOR PATIENT)
WHERE CODE IN ("G2","G20%","G24","G240%","G241","G2410","G244"-"G24z","G2y"-"G2z")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
