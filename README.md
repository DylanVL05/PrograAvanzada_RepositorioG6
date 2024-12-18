# E-Commerce Librería - Proyecto de Programación Avanzada

## Descripción del Proyecto

Este proyecto consiste en el desarrollo de un sitio web de **comercio electrónico** para una **librería** utilizando **ASP.NET 4.8 MVC**. La aplicación permite a los usuarios navegar, buscar, y comprar libros en línea. Además, los administradores pueden gestionar productos, pedidos y usuarios entre otros.

### Funcionalidades:

- **Usuarios:**
  - Registro y autenticación de usuarios.
  - Visualización de productos (libros) disponibles.
  - Búsqueda y filtrado de productos por categoría o autor.
  - Carrito de compras con la posibilidad de agregar y eliminar productos.
  - Proceso de compra con formulario de pago.

- **Administradores:**
  - Gestión de productos (agregar, editar, eliminar libros).
  - Gestión de pedidos y usuarios.
  

## Tecnologías Utilizadas

- **Frontend:**
  - HTML5
  - CSS3
  - JavaScript (jQuery)
  - Bootstrap (para la interfaz de usuario)

- **Backend:**
  - ASP.NET 4.8 MVC
  - C# para la lógica del servidor

- **Base de Datos:**
  - SQL Server (o base de datos SQL equivalente)
  - Entity Framework (para la gestión de datos)

- **Otros:**
  - Autenticación mediante **ASP.NET Identity**.
  - Validación de datos en el frontend y backend.

## Instalación

### Requisitos previos:
- **Visual Studio 2019 o superior** con soporte para **ASP.NET 4.8**.
- SQL SERVER
- Hacer la migración con entity o usar la db abjunta en el repo 
  
### Pasos para ejecutar el proyecto:

1. Clona este repositorio:
   ```bash
  https://github.com/DylanVL05/PrograAvanzada_RepositorioG6.git

2.Descagar el .bak para la base 

3.Abrir el pryecto y el SQL Management studio restaurar base 

4.Cambiar o ajustar el conection string del webconfig segun la ruta y nombre del servicio 
Ejemplo:
<connectionStrings>
    <add name="DefaultConnection"
         connectionString="Data Source=MI_SERVIDOR;AttachDbFilename=C:\Ruta\A\Tu\BaseDeDatos.mdf;Initial Catalog=NombreDeBaseDeDatos;Integrated Security=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>

