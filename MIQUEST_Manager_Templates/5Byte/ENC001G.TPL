*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,ENC001G,Subset
*QRY_ORDER,821,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR CON003G
SUBSET ENC001G TEMP
FROM ENCOUNTERS (ONE FOR PATIENT)
WHERE ACTIVE IN ("R")
AND DATE IN ("<start_date>"-"<end_date>")
