*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,DIA002G,Subset DIAG_NEOPLASMS
*QRY_ORDER,007,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR CON003G
SUBSET DIA002G TEMP
FROM JOURNALS (ONE FOR PATIENT)
WHERE CODE IN ("B%")
    AND DATE IN ("<start_date>"-"<end_date>")

