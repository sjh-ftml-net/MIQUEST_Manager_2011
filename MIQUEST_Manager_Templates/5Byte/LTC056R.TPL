*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,LTC056R,Report LTC-SUPRAVENTRICULARTACHYCARDIANOS
*QRY_ORDER,312,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR LTC056G
REPORT
PRINT NHS_NUMBER,PRACT_NUMBER,CODE,DATE
FROM JOURNALS (LATEST FOR PATIENT)
WHERE CODE IN ("G57y9")
    AND DATE IN ("<ltc_start_date>"-"<ltc_end_date>")
