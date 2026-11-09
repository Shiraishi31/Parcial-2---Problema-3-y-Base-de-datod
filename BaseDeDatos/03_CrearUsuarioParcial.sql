-- Parcial 3 - Punto 3
-- Crea el usuario de MySQL que usa el programa (Conexion.cs: User ID=parcial; Password=Parcial123*).
-- Ejecutar DESPUÉS del script 01 (o después de restaurar el backup).

CREATE USER IF NOT EXISTS 'parcial'@'localhost' IDENTIFIED BY 'Parcial123*';
GRANT ALL PRIVILEGES ON LoginBCryptDB.* TO 'parcial'@'localhost';
FLUSH PRIVILEGES;