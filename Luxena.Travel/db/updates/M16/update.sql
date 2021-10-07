CREATE TABLE `lt_closed_period` (
  `id` varchar(32) NOT NULL,
  `version` int(11) NOT NULL,
  `createdby` varchar(32) NOT NULL,
  `createdon` datetime NOT NULL,
  `modifiedby` varchar(32) DEFAULT NULL,
  `modifiedon` datetime DEFAULT NULL,
  `datefrom` datetime NOT NULL,
  `dateto` datetime NOT NULL,
  `periodstate` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;



alter table lt_system_configuration add isunifiedregistercoderequired tinyint(1) not null;

create unique index unifiedregistercode on lt_organization(unifiedregistercode);
