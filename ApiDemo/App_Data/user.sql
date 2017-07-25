/*
Navicat MySQL Data Transfer

Source Server         : rm-bp1w9i2163go2xna9.mysql.rds.aliyuncs.com
Source Server Version : 50518
Source Host           : rm-bp1w9i2163go2xna9.mysql.rds.aliyuncs.com:3306
Source Database       : rr8sisa5se

Target Server Type    : MYSQL
Target Server Version : 50518
File Encoding         : 65001

Date: 2017-07-25 13:53:12
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `Id` bigint(20) NOT NULL,
  `Name` varchar(50) DEFAULT NULL COMMENT '昵称',
  `Age` smallint(6) DEFAULT NULL,
  `UpdateAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('56907279991046144', 'tyS5', '25', null);
INSERT INTO `user` VALUES ('56907279995240448', 'T8qd', '24', '2017-06-15 17:57:55');
INSERT INTO `user` VALUES ('56907279995240449', 'quE1', '25', null);
INSERT INTO `user` VALUES ('56907279995240450', 'jvpM', '23', null);
INSERT INTO `user` VALUES ('56907279995240451', 'HaWY', '22', null);
INSERT INTO `user` VALUES ('56907279995240452', 'cUKD', '23', null);
INSERT INTO `user` VALUES ('56907279995240453', 'fNrQ', '21', null);
INSERT INTO `user` VALUES ('56907279995240454', 'JuCj', '21', null);
INSERT INTO `user` VALUES ('56907279995240455', 'jyf1', '27', null);
INSERT INTO `user` VALUES ('56907431682244608', '7Gp5', '24', null);
INSERT INTO `user` VALUES ('56907822041923584', 'Dfi4', '24', null);
