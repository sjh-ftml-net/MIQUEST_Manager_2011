*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,LTC071G,Subset LTC-PULMONARYOEDEMANOS
*QRY_ORDER,341,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR CON003G
SUBSET LTC071G TEMP
FROM JOURNALS (ONE FOR PATIENT)
WHERE CODE IN ("H541z")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
