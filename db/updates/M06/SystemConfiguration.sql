ALTER TABLE lt_system_configuration
 DROP FOREIGN KEY configuration_birthdaytaskresposible,
 ADD defaultcurrency VARCHAR(32) AFTER birthdaytaskresposible;
ALTER TABLE lt_system_configuration
 ADD CONSTRAINT configuration_birthdaytaskresposible FOREIGN KEY (birthdaytaskresposible) REFERENCES lt_person (id) ON UPDATE RESTRICT ON DELETE RESTRICT,
 ADD CONSTRAINT configuration_defaultcurrency FOREIGN KEY (defaultcurrency) REFERENCES lt_currency (id) ON UPDATE RESTRICT ON DELETE RESTRICT;
