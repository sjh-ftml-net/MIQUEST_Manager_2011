*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,CON001G,Subset PRACTICE POPULATION
*QRY_ORDER,001,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
SUBSET CON001G TEMP
FROM PATIENTS
WHERE ACTIVE IN ("R","T","S","D","L","P")
