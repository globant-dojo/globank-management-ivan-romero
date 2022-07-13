# globank-management-ivan-romero

## Instalación

1. Crear la BDD (Utilizar script "BaseDatos.sql" que se encuentra en la raíz del proyecto).
2. Cargar la solución en Visual Studio 2022.
3. Actualizar la conexión Local dentro de ConnectionStrings en los archivos appsettings.json de los proyectos ClientesApp, CuentasApp, MovimientosApp, ReportesApp.
4. Establecer docker-compose como proyecto de Inicio.
5. Ejecutar proyecto Docker Compose.

## Pruebas

1. Cargar el archivo "GloBankManagement_IvanRomero.postman_collection.json" en PostMan.
2. Crear un Environment y setear las siguientes llaves:
	- portCliente XXXXX (reemplazar por el puerto en el que este corriendo la aplicación en docker)
	- portCuenta XXXXX (reemplazar por el puerto en el que este corriendo la aplicación en docker)
	- portMovimiento XXXXX (reemplazar por el puerto en el que este corriendo la aplicación en docker)
	- portReporte XXXXX (reemplazar por el puerto en el que este corriendo la aplicación en docker)

	[Mas información](https://learning.postman.com/docs/sending-requests/variables/)
3. Ejecutar las peticiones que se encuentran en el proyecto.