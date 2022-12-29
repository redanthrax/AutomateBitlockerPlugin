CREATE TABLE `{0}` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`created` datetime NOT NULL DEFAULT NOW(),
`action` varchar(300) NOT NULL,
`computerid` int(11) NOT NULL,
`locationid` int(11) NOT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY `id_UNIQUE` (`id`));