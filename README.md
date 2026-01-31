# 📝 Tasks Manager App

[![.NET](https://img.shields.io/badge/.NET-6.0%2B-512bd4)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-In--Memory-blue)](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/)

Esta es una API RESTful desarrollada con **ASP.NET Core Minimal APIs**. El proyecto demuestra el uso de tipos genéricos en DTOs, integración con Entity Framework Core y manejo estructurado de excepciones.

---

## 🚀 Instrucciones para el Docente

Para facilitar la revisión y asegurar que el proyecto funcione de inmediato en su equipo, siga estas instrucciones:

### 1. Requisitos
* Tener instalado el **SDK de .NET 6.0** (o versiones superiores como 7.0 u 8.0).
* Visual Studio 2022 o VS Code.

### 2. Configuración de Base de Datos
La aplicación utiliza una **Base de Datos en Memoria (In-Memory Database)**. 
> [!IMPORTANT]
> **No se requiere SQL Server ni ejecutar migraciones.** La base de datos se inicializa vacía cada vez que se arranca la aplicación, lo que garantiza un entorno de prueba limpio y sin dependencias externas.

### 3. Cómo ejecutar
1. Abra la solución `.sln` en Visual Studio.
2. Asegúrese de que el proyecto `TasksManagerApp` esté configurado como proyecto de inicio.
3. Presione `F5` o ejecute desde la terminal:
   ```bash
   dotnet run
