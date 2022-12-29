SELECT c.ComputerID, 
c.Name, 
b.ProtectionStatus, 
b.KeyProtector, 
b.VolumeStatus, 
b.TPMPresent, 
b.TPMReady, 
IF(b.RecoveryKey IS NULL or b.RecoveryKey = '', "False", "True") AS HasRecoveryKey,
b.IsDomainController
FROM computers c
LEFT JOIN {0} b ON c.ComputerID = b.ComputerID
WHERE c.ComputerID = @ComputerID;