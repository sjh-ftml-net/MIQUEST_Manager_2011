*QRY_WDATE,20110614,14/06/2011
*QRY_SDATE,20110401,01/04/2011
*QRY_TITLE,DIA006G,Subset DIAG_NERVOUS_SYSTEM_AND_SENSE_ORGAN_DISEASES
*QRY_ORDER,015,
*QRY_MEDIA,D,DISK
*QRY_AGREE,LOCAL,
*ENQ_IDENT,LOCAL,
*QRY_SETID,ReadV2, Read V2 set
*ENQ_RSPID,<practice_code>,
*QRY_CODES,0,9999R2,
FOR CON003G
SUBSET DIA006G TEMP
FROM JOURNALS (ONE FOR PATIENT)
WHERE CODE IN ("F%")
    AND DATE IN ("<start_date>"-"<end_date>")

