*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,LTC069G,Subset LTC-INTESTINALMALABSORPTIONINCLUDINGCOELIACDISEASEANDPANCREATICSTEATORRHOEA
*QRY_ORDER,536,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR CON003G
SUBSET LTC069G TEMP
FROM JOURNALS (ONE FOR PATIENT)
WHERE ACTIVE IN ("R")
    AND CODE IN ("Xa9C9%")
    AND CODE NOT_IN ("X3038")
