CREATE TABLE `{0}` (
  `LocationID` INT NOT NULL,
  `Encrypt` TINYINT(1) NOT NULL,
  `DomainControllers` VARCHAR(255) NULL,
  `HasBitlockerGPO` TINYINT(1) NULL,
  PRIMARY KEY (`LocationID`));