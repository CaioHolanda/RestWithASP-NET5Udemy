CREATE TABLE IF NOT EXISTs `person` (
  `id` BIGINT(20) not null AUTO_INCREMENT,
  `address` varchar(100) NOT NULL,
  `first_name` VARCHAR(80) NOT NULL,
  `gender` VARCHAR(6) NOT NULL,
  `last_name` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`)
)