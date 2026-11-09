-- =====================================================================
-- Parcial 3 - Punto 3  (MySQL 8 / MySQL Workbench)
-- Script 02: consultas para ANALIZAR los intentos de inicio de sesión.
-- Pon el cursor en una consulta y ejecútala sola con Ctrl + Enter.
-- =====================================================================

USE LoginBCryptDB;

-- 1) Usuarios registrados (solo hay hashes, ninguna contraseña)
SELECT Id, NombreUsuario, NombreCompleto, PasswordHash, FechaCreacion
FROM Usuarios
ORDER BY Id;

-- 2) Historial completo de intentos, del más reciente al más antiguo
SELECT i.Id,
       i.FechaIntento,
       i.NombreUsuarioIngresado,
       u.NombreCompleto,
       CASE WHEN i.Exitoso = 1 THEN 'EXITOSO' ELSE 'FALLIDO' END AS Resultado,
       i.Detalle
FROM IntentosLogin AS i
LEFT JOIN Usuarios AS u ON u.Id = i.UsuarioId
ORDER BY i.FechaIntento DESC, i.Id DESC;

-- 3) Resumen por usuario: total, exitosos, fallidos y último acceso exitoso
SELECT NombreUsuarioIngresado,
       COUNT(*)                                          AS TotalIntentos,
       SUM(CASE WHEN Exitoso = 1 THEN 1 ELSE 0 END)      AS Exitosos,
       SUM(CASE WHEN Exitoso = 0 THEN 1 ELSE 0 END)      AS Fallidos,
       MAX(CASE WHEN Exitoso = 1 THEN FechaIntento END)  AS UltimoAccesoExitoso
FROM IntentosLogin
GROUP BY NombreUsuarioIngresado
ORDER BY Fallidos DESC;

-- 4) Intentos fallidos por día
SELECT DATE(FechaIntento) AS Dia,
       COUNT(*)           AS IntentosFallidos
FROM IntentosLogin
WHERE Exitoso = 0
GROUP BY DATE(FechaIntento)
ORDER BY Dia DESC;

-- 5) Nombres de usuario que NO existen y que intentaron entrar (posible ataque)
SELECT NombreUsuarioIngresado, COUNT(*) AS Veces, MAX(FechaIntento) AS UltimaVez
FROM IntentosLogin
WHERE UsuarioId IS NULL
GROUP BY NombreUsuarioIngresado
ORDER BY Veces DESC;

-- 6) Usuarios con 3 o más intentos fallidos en la última hora (candidatos a bloqueo)
SELECT NombreUsuarioIngresado, COUNT(*) AS FallidosUltimaHora
FROM IntentosLogin
WHERE Exitoso = 0
  AND FechaIntento >= NOW() - INTERVAL 1 HOUR
GROUP BY NombreUsuarioIngresado
HAVING COUNT(*) >= 3;
