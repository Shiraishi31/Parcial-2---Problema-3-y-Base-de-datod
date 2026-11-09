
CREATE DATABASE IF NOT EXISTS LoginBCryptDB
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci;

USE LoginBCryptDB;


-- Tabla 1: Usuarios

CREATE TABLE IF NOT EXISTS Usuarios
(
    Id             INT          NOT NULL AUTO_INCREMENT,
    NombreUsuario  VARCHAR(50)  NOT NULL,
    NombreCompleto VARCHAR(100) NOT NULL,
    PasswordHash   CHAR(60)     NOT NULL,
    FechaCreacion  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario)
);


-- Tabla 2: IntentosLogin

CREATE TABLE IF NOT EXISTS IntentosLogin
(
    Id                     INT          NOT NULL AUTO_INCREMENT,
    UsuarioId              INT          NULL,
    NombreUsuarioIngresado VARCHAR(50)  NOT NULL,
    Exitoso                BOOLEAN      NOT NULL,
    FechaIntento           DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Detalle                VARCHAR(100) NULL,
    CONSTRAINT PK_IntentosLogin PRIMARY KEY (Id),
    CONSTRAINT FK_IntentosLogin_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios (Id),
    INDEX IX_IntentosLogin_FechaIntento (FechaIntento)
);


INSERT IGNORE INTO Usuarios (NombreUsuario, NombreCompleto, PasswordHash)
VALUES ('admin', 'Administrador del Sistema',
        '$2a$11$ahVWUzTV88L0jQk0i86A.OkOmIUOw01hgeu7A98bYNeSpl0JEsnhm');

SELECT Id, NombreUsuario, NombreCompleto, PasswordHash, FechaCreacion
FROM Usuarios;
