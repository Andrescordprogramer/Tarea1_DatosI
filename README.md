# Tarea1_DatosI
programa de mensajería instantánea.
Estudiante: Esteban Andrés Altamirano Cordero


Ejecutar:
1 - Dirijase a la carpeta InstantMessage/InstantMessage
2 - Click derecho >>> Abrir en Terminal(repetir este paso para crear varias instancias)

Creacion de instancias:
Escribir dotnet run -port <puerto de escucha> y luego enter
  - ejemplo instancia1: dotnet run -port 5000
  - ejemplo instancia2: dotnet run -port 5001
  - ejemplo instancia3: dotnet run -port 5002

Envio y recepcion de mensajes:
Despues de que ejecutaste el codigo anterior en cada una de tus instancias te debió de aparecer esto:
  - Ingrese el puerto destino: Servidor escuchando en el puerto 5000...
  - Ingrese el puerto destino: Servidor escuchando en el puerto 5001...
  - Ingrese el puerto destino: Servidor escuchando en el puerto 5002...

Ahora digamos que del puerto 5000 quiero enviarle mensaje al del puerto 5001.
Escribimos el puerto y nos aparecerá lo siguiente:
  - Ingrese el mensaje:

Escribiremos Hola por ejemplo y seguidamente le damos enter, nos aparecerá lo siguiente:
  - Mensaje enviado a 5001: Hola
  - Ingrese el puerto destino:

Mientras que en la instancia que escucha desde el puerto 5000 nos aparecerá este mensaje:
  - Mensaje recibido: Hola




Esto es todo, gracias.
 
