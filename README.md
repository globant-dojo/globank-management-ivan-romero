# globank-management-ivan-romero

## Instalaci�n

1. Crear la BDD (Utilizar script "BaseDatos.sql" que se encuentra en la ra�z del proyecto).
2. Cargar la soluci�n en Visual Studio 2022.
3. Actualizar la conexi�n Local dentro de ConnectionStrings en los archivos appsettings.json de los proyectos ClientesApp, CuentasApp, MovimientosApp, ReportesApp.
4. Establecer docker-compose como proyecto de Inicio.
5. Ejecutar proyecto Docker Compose.

## Pruebas

1. Cargar el archivo "GloBankManagement_IvanRomero.postman_collection.json" en PostMan.
2. Crear un Environment y setear las siguientes llaves:
	- portCliente XXXXX (reemplazar por el puerto en el que este corriendo la aplicaci�n en docker)
	- portCuenta XXXXX (reemplazar por el puerto en el que este corriendo la aplicaci�n en docker)
	- portMovimiento XXXXX (reemplazar por el puerto en el que este corriendo la aplicaci�n en docker)
	- portReporte XXXXX (reemplazar por el puerto en el que este corriendo la aplicaci�n en docker)

	[Mas informaci�n](https://learning.postman.com/docs/sending-requests/variables/)
3. Ejecutar las peticiones que se encuentran en el proyecto.