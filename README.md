# Alkemy.Disney

# Register  
Ejemplo de solicitud de registro de usuario.  

curl --location --request POST 'https://localhost:44392/Auth/Register'  
--form 'email="cesar@email.com.ar"'  
--form 'password="Cesar*1234"'  

# Login
El EndPont Auth/Login devuelve un JsonWebToken que permite identificar al usuario logueado. El mismo debe ser enviado en el encabezado de las peticiones a 
los EndPoint de la api para poder acceder a los recursos protegidos.   

### Ejemplo de solicitud de token  
curl --location --request POST 'https://localhost:44392/Auth/Login'  
--form 'email="cesar@email.com.ar"'  
--form 'password="Cesar*1234"'  


### Ejemplo de una solicitud a un recurso protegido  

curl --location --request GET 'https://localhost:44392/Movies'  
--header 'Authorization: Bearer JwtOptenidoDeLaAPI  

# Requests
### Todas las solicitudes a los distintos EndPoints reciben datos a traves de form-data. Ejemplo de envio de informaciona a un EndPoint.  

curl --location --request PUT 'https://localhost:44392/genres/3'  
--header 'Authorization: Bearer JwtOptenidoDeLaAPI  
--form 'id="3"'
--form 'name="drama modificado"'
--form 'image=@"/path/to/file"'

# Many to Many  
### Para establecer una relación entre Personajes y Pelicualas se debe realizar post al endpoint de la siguiente manera.

curl --location --request POST 'https://localhost:44392/Movies/6/Caracters'  
--header 'Authorization: Bearer JwtOptenidoDeLaAPI  
--form 'id="2"'

### Para eliminar una relacion entre Personajes y Peliculas se debe realizar un Delete al endpoint de la siguiente manera.

curl --location --request DELETE 'https://localhost:44392/Movies/6/Caracters/2'  
--header 'Authorization: Bearer JwtOptenidoDeLaAPI  

# Imagenes
### Todos los endpoints devuelven las propiedades image de las entidades como un link a la propia api. Al acceder a el link, la api devuelve el archivo de imagen solicitado. 

{  
    "id": 3,  
    "name": "drama modificado",  
    "image": "https://localhost:44392/Images/Genres/3",  
    "movies": [...]  
}  

# Envio de emails
### Para que funcione es necesario cambiar las credenciales de SendGrid en el archivo appsettings.json

"SendGrid": {  
    "ApiKey": "suapikey",  
    "ApiId": "suapiid",  
    "FromEmail": "sucorreo@dominio.com",  
    "FromName": "El nombre del remitente"  
  }  
