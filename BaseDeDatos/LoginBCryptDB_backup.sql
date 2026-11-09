CREATE DATABASE  IF NOT EXISTS `loginbcryptdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `loginbcryptdb`;
-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: loginbcryptdb
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `intentoslogin`
--

DROP TABLE IF EXISTS `intentoslogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `intentoslogin` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UsuarioId` int DEFAULT NULL,
  `NombreUsuarioIngresado` varchar(50) NOT NULL,
  `Exitoso` tinyint(1) NOT NULL,
  `FechaIntento` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Detalle` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_IntentosLogin_Usuarios` (`UsuarioId`),
  KEY `IX_IntentosLogin_FechaIntento` (`FechaIntento`),
  CONSTRAINT `FK_IntentosLogin_Usuarios` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `intentoslogin`
--

LOCK TABLES `intentoslogin` WRITE;
/*!40000 ALTER TABLE `intentoslogin` DISABLE KEYS */;
INSERT INTO `intentoslogin` VALUES (1,1,'admin',1,'2026-11-09 12:01:26','Acceso concedido'),(2,2,'Christian',0,'2026-11-09 12:04:31','Contraseña incorrecta'),(3,2,'Christian',1,'2026-11-09 12:04:48','Acceso concedido');
/*!40000 ALTER TABLE `intentoslogin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `NombreUsuario` varchar(50) NOT NULL,
  `NombreCompleto` varchar(100) NOT NULL,
  `PasswordHash` char(60) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_Usuarios_NombreUsuario` (`NombreUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'admin','Administrador del Sistema','$2a$11$ahVWUzTV88L0jQk0i86A.OkOmIUOw01hgeu7A98bYNeSpl0JEsnhm','2026-11-09 11:29:10'),(2,'Christian','Christian Arce','$2a$11$x6.b3gwAXDeNq65xfbUy9e0Bl36T9eoFFLyNs7OTGN1ZGBD.50GH6','2026-11-09 12:03:11');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-09 12:19:06
