# Prueba Técnica Unity Quasar

## Para ejecutar la prueba técnica:

Dentro de la carpeta "api" se encuentran los archivos necesarios para que se ejecute el código de Symfony, habría que acceder a su interior y usar el comando:

```
symfony server:start
```
Con esto debería ser suficiente para iniciar el servidor y poder realizar las llamadas API necesarias desde el ejecutable de Unity, aunque es posible que sea necesario instalar algunos de los componentes que se listan en el proceso.

Ahora sería necesario acceder a la carpeta "unity" y ejecutar el archivo exe.

![Captura de pantalla 2023-01-07 151743](https://user-images.githubusercontent.com/76244023/211155198-9fdefeba-9cbe-4833-8d81-b069ac47ddd4.png)

## Proceso seguido

1. Aprendizaje básico de Symfony.
2. Creación de un proyecto Symfony mediante la terminal y desarrollo de la API.
   1. Comandos utilizados:
   ```
   scoop install symfony-cli
   symfony new api_quasar
   composer require annotations
   composer require logger
   composer require symfony/orm-pack
   composer require --dev symfony/maker-bundle
   composer require symfony/http-client
   ```
   2. Requerimientos de la API:
      * Obtener información de un registro de usuario de la base datos mediante email y la contraseña
      * Poder crear un registro de usuario en la base de datos mediante email, contraseña, nombre y apellidos
   3. Hash para guardar la contraseña dentro de la base de datos:
   ```
   $password_encrypted = password_hash($password, $hash_method, ['cost' => 15]);
   .
   .
   .
   $password_verified = password_verify($password, $get_user_response[0]["password"]);
   ```
3. Creación de una base de datos (he escogido Supabase) y generación de una tabla de usuarios.

![Captura de pantalla 2023-01-07 153503](https://user-images.githubusercontent.com/76244023/211155985-94aa1b56-8c05-41df-8441-9c1d502eda96.png)

4. Creación de un proyecto con la plantilla de juego en 3a persona de Unity, creación de la escena del login y el registro y de su correspondiente código.
   1. Se han generado distintos scripts:
      * Uno para el Login del usuario.
      * Uno para el registro del usuario.
      * Uno para el cambio entre login y registro.
      * Uno global para pasar la variable del nombre del usuario a la escena del entorno 3D.
      * Uno para añadir la variable global al texto del jugador.

## Archivos de códigos creados

Podéis acceder al código que he generado directamente desde la carpeta code, este está dividido entre el código de la API y el código de Unity (no añado el del personaje ya que viene por defecto con la plantilla en la creación).
