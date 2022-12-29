UPDATE `{0}`
SET
`TpmPresent` = @TPMPresent,
`TpmReady` = @TPMReady,
`TpmManagedAuthLevel` = @TpmManagedAuthLevel,
`TpmAutoProvisioning` = @TpmAutoProvisioning,
`VolumeType` = @VolumeType,
`MountPoint` = @MountPoint,
`CapacityGB` = @CapacityGB,
`VolumeStatus` = @VolumeStatus,
`EncryptionPercentage` = @EncryptionPercentage,
`KeyProtector` = @KeyProtector,
`ProtectionStatus` = @ProtectionStatus,
`RecoveryKey` = @RecoveryKey,
`Updated` = NOW(),
`IsDomainController` = @IsDomainController
WHERE (`ComputerID` = @ComputerID)