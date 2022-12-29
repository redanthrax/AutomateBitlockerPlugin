INSERT INTO `{0}` (
`LocationID`,
`Encrypt`,
`DomainControllers`,
`HasBitlockerGPO`)
SELECT `LocationID`,
0 AS `Encrypt`,
'' AS `DomainControllers`,
0 AS `HasBitlockerGPO`
FROM locations;