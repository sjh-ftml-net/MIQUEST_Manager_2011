*QRY_WDATE,20120619,19/06/2012
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,LTC099R,Report LTC-CHRONICRENAL2
*QRY_ORDER,828,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR LTC099G
REPORT
PRINT NHS_NUMBER,PRACT_NUMBER,CODE,DATE
FROM JOURNALS (LATEST FOR PATIENT)
WHERE CODE IN ("K05%")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
