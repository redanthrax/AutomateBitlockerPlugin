UPDATE `{0}`
SET
`Encrypt` = @Encrypt,
`DomainControllers` = @DomainControllers,
`HasBitlockerGPO` = @HasBitlockerGPO
WHERE (`LocationID` = @LocationID)