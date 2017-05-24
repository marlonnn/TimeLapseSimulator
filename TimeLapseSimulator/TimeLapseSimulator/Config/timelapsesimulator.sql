/*
Navicat MySQL Data Transfer

Source Server         : test
Source Server Version : 50717
Source Host           : localhost:3306
Source Database       : timelapsesimulator

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2017-05-24 14:43:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for environment
-- ----------------------------
DROP TABLE IF EXISTS `environment`;
CREATE TABLE `environment` (
  `Culture_ID` int(11) NOT NULL,
  `Oxygen_Concentration` float DEFAULT NULL,
  `Temperature_Value` float DEFAULT NULL,
  `Humidity_Value` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Culture_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for slide1
-- ----------------------------
DROP TABLE IF EXISTS `slide1`;
CREATE TABLE `slide1` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Slide_ID` int(11) DEFAULT NULL,
  `Slide_Name` varchar(255) DEFAULT NULL,
  `Cell_ID` int(11) DEFAULT NULL,
  `Cell_Name` varchar(255) DEFAULT NULL,
  `Focal_ID` int(11) DEFAULT NULL,
  `Focal_Name` varchar(255) DEFAULT NULL,
  `Time` datetime DEFAULT NULL,
  `Image` longblob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1438 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for slide2
-- ----------------------------
DROP TABLE IF EXISTS `slide2`;
CREATE TABLE `slide2` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Slide_ID` int(11) DEFAULT NULL,
  `Slide_Name` varchar(255) DEFAULT NULL,
  `Cell_ID` int(11) DEFAULT NULL,
  `Cell_Name` varchar(255) DEFAULT NULL,
  `Focal_ID` int(11) DEFAULT NULL,
  `Focal_Name` varchar(255) DEFAULT NULL,
  `Time` datetime DEFAULT NULL,
  `Image` longblob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1366 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for slide3
-- ----------------------------
DROP TABLE IF EXISTS `slide3`;
CREATE TABLE `slide3` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Slide_ID` int(11) DEFAULT NULL,
  `Slide_Name` varchar(255) DEFAULT NULL,
  `Cell_ID` int(11) DEFAULT NULL,
  `Cell_Name` varchar(255) DEFAULT NULL,
  `Focal_ID` int(11) DEFAULT NULL,
  `Focal_Name` varchar(255) DEFAULT NULL,
  `Time` datetime DEFAULT NULL,
  `Image` longblob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1269 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for slide4
-- ----------------------------
DROP TABLE IF EXISTS `slide4`;
CREATE TABLE `slide4` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Slide_ID` int(11) DEFAULT NULL,
  `Slide_Name` varchar(255) DEFAULT NULL,
  `Cell_ID` int(11) DEFAULT NULL,
  `Cell_Name` varchar(255) DEFAULT NULL,
  `Focal_ID` int(11) DEFAULT NULL,
  `Focal_Name` varchar(255) DEFAULT NULL,
  `Time` datetime DEFAULT NULL,
  `Image` longblob,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1227 DEFAULT CHARSET=utf8;
