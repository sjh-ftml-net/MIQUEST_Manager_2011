*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,QOF010R,Report QOF-DEPRESSION
*QRY_ORDER,620,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR QOF010G
REPORT
PRINT NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE
FROM JOURNALS (LATEST FOR PATIENT)
WHERE ACTIVE IN ("R")
AND CODE IN ("X00Sb","X00SO%")
    AND CODE NOT_IN ("62T1.%","E2B0.","XaCHo","XaX54","XaX53")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
