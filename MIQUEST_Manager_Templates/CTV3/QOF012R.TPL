*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,QOF012R,Report QOF-EPILEPSY
*QRY_ORDER,624,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR QOF012G
REPORT
PRINT NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE
FROM JOURNALS (LATEST FOR PATIENT)
WHERE ACTIVE IN ("R")
AND CODE IN ("F25..%","SC200","F257.")
    AND CODE NOT_IN ("X006g%","X006n","Q480.%","XaEIj%","Xa0lJ","Xa0lJ","Xa0lJ","Xa0lJ","Xa0lJ","Xa0lJ","Xa0lJ","X005t%","Xa0lJ","X006G%","XaBM2")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
