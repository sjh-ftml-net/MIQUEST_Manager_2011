*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,QOF014G,Subset QOF-HYPERTENSION
*QRY_ORDER,627,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR CON003G
SUBSET QOF014G TEMP
FROM JOURNALS (LATEST FOR PATIENT)
WHERE ACTIVE IN ("R")
    AND CODE IN ("XE0Ub","XE0Uc%","G24..%","G2...%","Xa0Cs","XSDSb","G202.","Xa3fQ")
    AND CODE NOT_IN ("61462")
