# JustPharm

![Esquema](/docs/tomas.png)
![Esquema](/docs/estadisticas.png)

## Índice

1. [Especificaciones Técnicas](#especificaciones-técnicas)
2. [Requerimientos del Sistema](#requerimientos-del-sistema)
   - [Entorno de Desarrollo](#entorno-de-desarrollo)
   - [Dependencias Externas](#dependencias-externas)
3. [Configuración de Valores](#configuración-de-valores)
4. [Esquemas y arquitectura](#esquema-de-la-base-de-datos)
  - [Base de datos](#esquema-de-la-base-de-datos)
  - [Esquema de flujo de la aplicación](#flujo-de-la-aplicación-y-casos-de-uso)
  - [Arquitectura](#descripción-de-la-arquitectura-de-la-aplicación)
5. [Despliegue en Azure Web Service](#despliegue-en-azure-web-service)
   - [Crear la Aplicación en Azure](#crear-la-aplicación-en-azure)
   - [Configurar la Cadena de Conexión](#configurar-la-cadena-de-conexión)
   - [Desplegar la Aplicación](#desplegar-la-aplicación)
   - [Configuraciones Adicionales](#configuraciones-adicionales)
6. [Consideracion Final](#consideraciones-finales)
7. [Descripción General del Proyecto](#descripción-general-del-proyecto)
   - [Finalidad y Objetivos del Trabajo](#finalidad-y-objetivos-del-trabajo)
   - [Objetivos de Aprendizaje](#objetivos-de-aprendizaje)
8. [Metodología de Desarrollo](#metodología-de-desarrollo)
   - [Metodología Ágil con Kanban](#metodología-ágil-con-kanban)
   - [Herramientas y Tecnologías](#herramientas-y-tecnologías)
9. [Gestión de Tareas](#gestión-de-tareas)
10. [Evaluación del Progreso y la Calidad](#evaluación-del-progreso-y-la-calidad)
11. [Planificación del Proyecto](#planificación-del-proyecto)
    - [Cronograma del Proyecto](#cronograma-del-proyecto)
    - [Recursos del Proyecto](#recursos-del-proyecto)
    - [Presupuesto del Proyecto](#presupuesto-del-proyecto)
    - [Riesgos y Contingencias](#riesgos-y-contingencias)
12. [Desarrollo del Sistema](#desarrollo-del-sistema)
    - [Arquitectura del Sistema](#arquitectura-del-sistema)
    - [Tecnologías y Herramientas Específicas](#tecnologías-y-herramientas-específicas)
    - [Interfaz de Usuario](#interfaz-de-usuario)
    - [Seguridad y Privacidad](#seguridad-y-privacidad)
13. [Mantenimiento y Soporte](#mantenimiento-y-soporte)
    - [Plan de Mantenimiento](#plan-de-mantenimiento)
    - [Soporte Técnico](#soporte-técnico)

---

## Especificaciones Técnicas

### Framework
- **.NET 8.0**

### Tipo de Proyecto
- **Blazor Web App**

### Dependencias Principales
- **Microsoft.EntityFrameworkCore.SqlServer** (versión 8.0.4) para la conexión a la base de datos SQL Server de Azure.
- **Syncfusion.Blazor** (versión 24.2.6) para componentes de interfaz de usuario en Syncfusion Blazor.
- **log4net** (versión 2.0.17) para el registro de eventos.
- Otros paquetes necesarios para Syncfusion Blazor (calendarios, tarjetas, cuadrículas, etc.).

## Requerimientos del Sistema

### Entorno de Desarrollo
- IDE compatible con .NET 8.0 (por ejemplo, Visual Studio 2022).
- SDK de .NET 8.0 o superior.

### Dependencias Externas
- Base de datos SQL Server para almacenar datos.

## Configuración de Valores

- **ConnectionStrings**: El archivo `appsettings.json` contiene la cadena de conexión a la base de datos SQL Server de Azure en la sección `"ConnectionStrings"`. Asegúrate de tener los valores correctos para `Server`, `Initial Catalog`, `User ID`, `Password`, etc.
- **ApiUriEndpoint**: Define la URI de la API si tu aplicación necesita interactuar con servicios externos.
- **UriString**: Define la URI base de la aplicación para el desarrollo local (`http://localhost:5065/` en este caso).
- **DefaultAdmin**: Proporciona las credenciales predeterminadas del administrador para la inicialización de la base de datos u otras configuraciones.

## Esquema de la Base de Datos

![Esquema](/docs/EsquemaBD.JPG)

## Flujo de la aplicación y casos de uso

### Diagrama del flujo de la aplicación
![Diagrama](/docs/diagram.png)


## Desarrollo del Sistema

### Arquitectura del Sistema
- **Cliente-Servidor**:
  - **Front-end**:
    - Implementado con Blazor.
    - Utiliza la biblioteca de componentes Syncfusion.
  - **Back-end**:
    - Desarrollado en ASP.NET Core utilizando C#.
    - Sigue el patrón de diseño Repository.
    - Proporciona APIs seguras y eficientes.
  - **Base de Datos**:
    - Implementada en SQL Server, alojada en Azure.
    - Gestionada a través de SQL Server Management Studio.

### Tecnologías y Herramientas Específicas

#### Front-end
- **Blazor**: Framework para crear aplicaciones web interactivas con C# y .NET.
- **Syncfusion**: Biblioteca de componentes de UI.

#### Back-end
- **ASP.NET Core**: Framework de desarrollo web.
- **C#**: Lenguaje de programación utilizado para el desarrollo del back-end.

#### Gestión de la Base de Datos
- **SQL Server**: Sistema de gestión de bases de datos relacional.
- **SQL Server Management Studio**: Herramienta para administrar y gestionar la base de datos.

### Interfaz de Usuario
- **Login**: Pantalla de inicio de sesión.
- **Panel de Control del Paciente**: Vista para pacientes con datos anónimos.
- **Vista del Calendario**: Información sobre el calendario de tomas y tratamientos.

### Seguridad y Privacidad
- **Autenticación y Autorización**:
  - Utiliza Microsoft Identity para la gestión de usuarios y autenticación.
  - Implementa roles y permisos para restringir el acceso a ciertas funcionalidades.
- **Protección de Datos**:
  - Encriptación de datos sensibles en tránsito y en reposo.
  - Cumplimiento con normativas como GDPR para la protección de datos personales.


## Descripción de la Arquitectura de la Aplicación

### Connected Services

- **Connected Services**: Esta carpeta contiene los servicios conectados que interactúan con APIs externas o servicios web. Es esencial para la integración con servicios de terceros, permitiendo que la aplicación aproveche funcionalidades externas sin necesidad de implementarlas desde cero.

### Dependencias (Dependencies)

- **Dependencias**: Este nodo lista todas las bibliotecas y paquetes NuGet que el proyecto utiliza. Incluye las dependencias necesarias para la funcionalidad de la aplicación, como Microsoft.EntityFrameworkCore.SqlServer para la base de datos, Syncfusion.Blazor para los componentes de la interfaz de usuario y log4net para el registro de eventos.

### Properties

- **Properties**: La carpeta Properties contiene archivos que configuran las propiedades del proyecto, incluyendo el archivo `launchSettings.json`, que define los perfiles de lanzamiento para el entorno de desarrollo, como la URL de la aplicación y otras configuraciones de inicio.

### wwwroot

- **wwwroot**: Este directorio es la raíz de la web pública del proyecto Blazor. Contiene archivos estáticos accesibles desde el navegador, como imágenes, estilos CSS, scripts JavaScript y otros recursos necesarios para la interfaz de usuario.

### CodeTemplates

- **CodeTemplates**: Este directorio almacena plantillas de código predefinidas que pueden ser utilizadas para generar rápidamente componentes comunes dentro de la aplicación. Facilita la consistencia y eficiencia en el desarrollo.

### Components

- **Components**: La carpeta Components contiene los componentes Blazor reutilizables que constituyen la interfaz de usuario de la aplicación. Los componentes pueden ser simples como botones o complejos como formularios de entrada de datos, y son fundamentales para la arquitectura modular de la aplicación.

### Data

- **Data**: En esta carpeta se encuentran las clases que representan el contexto de la base de datos y las entidades del modelo de datos. Incluye el DbContext de Entity Framework Core, que maneja las operaciones de base de datos y la conexión con SQL Server.

### Hubs

- **Hubs**: Contiene los SignalR hubs que permiten la comunicación en tiempo real entre el servidor y los clientes. Los hubs son esenciales para funcionalidades como notificaciones en tiempo real y actualizaciones de datos dinámicas.

### Migrations

- **Migrations**: Esta carpeta almacena las migraciones de la base de datos generadas por Entity Framework Core. Las migraciones son utilizadas para actualizar el esquema de la base de datos de forma incremental, reflejando los cambios en el modelo de datos del código.

### Models

- **Models**: La carpeta Models contiene las clases de modelos de datos que representan las entidades de negocio de la aplicación. Estas clases son utilizadas por Entity Framework Core para mapear los datos a las tablas de la base de datos.

### Resources

- **Resources**: Este directorio contiene recursos compartidos como cadenas de texto para localización y archivos de configuración adicionales que son utilizados en toda la aplicación.

### Services

- **Services**: La carpeta Services contiene los servicios que encapsulan la lógica de negocio de la aplicación. Los servicios son inyectables a través del contenedor de dependencias de .NET y son utilizados por los componentes y controladores para realizar operaciones como acceso a datos, procesamiento de negocios y llamadas a APIs externas.

## Despliegue en Azure Web Service

### Crear la Aplicación en Azure
1. Accede al portal de Azure y crea una nueva aplicación web.
2. Configura la pila como .NET 8.0 y selecciona el plan de servicio adecuado.

### Configurar la Cadena de Conexión
1. En la configuración de la aplicación en Azure, define la cadena de conexión a tu base de datos SQL Server de Azure en la sección de configuración de la aplicación.

### Desplegar la Aplicación
1. Puedes usar Visual Studio para publicar directamente en Azure desde tu proyecto Blazor.
2. También puedes usar herramientas de línea de comandos como Azure CLI o Git para desplegar la aplicación.

### Configuracion Adicional
1. Asegúrate de configurar las variables de entorno, configuraciones específicas de la aplicación (si es necesario) y las políticas de seguridad adecuadas para tu aplicación en Azure.



## Descripción General del Proyecto

### Finalidad y Objetivos del Trabajo
**Objetivo General (OG)**
- Desarrollar e implementar un Sistema de Gestión de Medicamentos que permita mejorar la calidad de la atención médica y la adherencia al tratamiento de los pacientes, así como facilitar el acceso a información relevante para los profesionales de la salud.

**Objetivos Secundarios (OS)**
- OS1: Evaluar la efectividad del Sistema de Gestión de Medicamentos en mejorar la adherencia al tratamiento y la calidad de vida de los pacientes.
- OS2: Validar la utilidad del sistema para los profesionales de la salud en la gestión y seguimiento de tratamientos médicos.
- OS3: Implementar y mantener altos estándares de seguridad y privacidad de datos para garantizar la confidencialidad de la información de los pacientes.
- OS4: Establecer una integración exitosa con farmacias locales para asegurar el acceso oportuno a medicamentos recetados y facilitar la sincronización de datos relacionados con las prescripciones.

### Objetivos de Aprendizaje
- **OA1**: Toma conciencia sobre la situación actual del sector médico.
- **OA2**: Profundizar en las posibilidades que ofrecen las distintas soluciones de IA en relación con el análisis e interpretación de informes médicos e imagen médica.
- **OA3**: Profundizar en el conocimiento de tecnologías utilizadas en la empresa.

## Metodología de Desarrollo

### Metodología Ágil con Kanban
- **Fases del Proceso de Desarrollo**:
  1. Investigación
  2. Análisis
  3. Diseño
  4. Desarrollo
  5. Implementación

### Herramientas y Tecnologías
- **Investigación**: CHAT GPT, Google, Wikipedia, entrevistas con profesionales de la salud.
- **Análisis y Diseño**: Notion, brainstorming.
- **Desarrollo**: .NET, Blazor Web App, Syncfusion, SQL Server en Docker.
- **Implementación**: Azure App Service, SQL Server en Azure.

## Gestión de Tareas
- Uso de Jira para la planificación y gestión de tareas.
- Creación y mantenimiento de un backlog en Jira.
- Uso de un tablero Kanban para visualizar el progreso de las tareas.

## Evaluación del Progreso y la Calidad

### Criterios de Evaluación
- **Investigación y Análisis**: Compleción de los requisitos y documentación detallada.
- **Diseño**: Aprobación del diseño de la arquitectura y las interfaces de usuario.
- **Desarrollo**: Pruebas unitarias y de integración satisfactorias.
- **Implementación**: Despliegue exitoso en Azure App Service, pruebas de aceptación del usuario, monitoreo de rendimiento.

## Planificación del Proyecto

### Cronograma del Proyecto
- **Duración**: 200 horas distribuidas en tres meses.
  - Investigación: 28 de febrero - 10 de marzo (30 horas)
  - Análisis: 11 de marzo - 24 de marzo (30 horas)
  - Diseño: 25 de marzo - 7 de abril (30 horas)
  - Desarrollo: 8 de abril - 19 de mayo (100 horas)
  - Implementación: 20 de mayo - 28 de mayo (10 horas)

### Recursos del Proyecto

#### Recursos Materiales
- Ordenador personal adecuado para el desarrollo de software y la gestión de bases de datos.
- Conexión a internet para la investigación, desarrollo, y despliegue en la nube.

#### Recursos Tecnológicos
- **IDE Visual Studio Community**: Herramienta principal para el desarrollo de la aplicación en .NET y Blazor.
- **Docker**: Para la creación y gestión de contenedores durante el desarrollo y pruebas.
- **SQL Server Management Studio**: Para la gestión y administración de la base de datos SQL Server en Azure.

### Presupuesto del Proyecto
- **Visual Studio Community**: Gratuito.
- **Docker**: Gratuito.
- **SQL Server Management Studio**: Gratuito.
- **Azure App Service**: Se utilizará el plan gratuito

 durante el desarrollo y pruebas.

### Riesgos y Contingencias

#### Riesgos Identificados
1. Problemas Técnicos: Fallos en el hardware del ordenador o problemas de conectividad a internet.
2. Retos en el Desarrollo: Dificultades técnicas al implementar ciertas funcionalidades o integrar tecnologías.
3. Gestión del Tiempo: Posibles retrasos en las fases del proyecto debido a subestimación de la duración de tareas o imprevistos personales.

#### Planes de Contingencia
1. **Problemas Técnicos**:
   - Realizar copias de seguridad periódicas del código y la documentación.
   - Tener acceso a un equipo de respaldo y planes alternativos de conexión a internet.
2. **Retos en el Desarrollo**:
   - Planificar tiempo adicional para la investigación y solución de problemas.
   - Utilizar recursos de soporte técnico y comunidades en línea para resolver problemas técnicos.
3. **Gestión del Tiempo**:
   - Implementar un seguimiento riguroso del cronograma utilizando Jira.
   - Revaluar y ajustar las prioridades y tiempos de las tareas según sea necesario para mantener el proyecto en marcha.

## Soporte


### Soporte Técnico
- **Contacto**: amin.m.boankod@gmail.com

