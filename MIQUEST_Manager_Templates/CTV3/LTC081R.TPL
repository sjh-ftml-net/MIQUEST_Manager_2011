*QRY_WDATE,20110523,23/05/2011
*QRY_SDATE,20110201,01/02/2011
*QRY_TITLE,LTC081R,Report LTC-DRUGDEPENDENCE
*QRY_ORDER,561,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,CTV3, CTV3 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R3,Read Version 3
FOR LTC081G
REPORT
PRINT NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE
FROM JOURNALS (LATEST FOR PATIENT)
WHERE ACTIVE IN ("R")
AND CODE IN ("X00S3","X00S4","XE1YR%","E24..","E242.%","E246.%","E248.%","E249.%","E24A.","E24z.","XE1YT%","XE1YU%","XE1YV%","XE1YW%")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
