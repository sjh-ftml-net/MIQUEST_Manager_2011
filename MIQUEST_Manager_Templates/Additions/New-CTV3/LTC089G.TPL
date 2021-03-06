*QRY_WDATE,20120526,26/05/2012
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,LTC089G,Subset LTC-NEW-CANCER  
*QRY_ORDER,902,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR CON003G
SUBSET LTC089G TEMP
FROM JOURNALS (ONE FOR PATIENT)
WHERE ACTIVE IN ("R")
    AND CODE IN ("B10..%","B133.%","B47..%")
