*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,DRU019R,Report DRUG_CONTRAST_MEDIA
*QRY_ORDER,138,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR DRU019G
REPORT
PRINT NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE
FROM JOURNALS (ALL FOR PATIENT)
WHERE CODE IN ("u%")
    AND DATE IN ("<start_date>"-"<end_date>")
