// This is an example of how the game would start 

TODO THIS SCENE IS THE INTRODUCTION TO THE GAME. THE GOAL OF THIS SCENE IS TO TEACH PLAYERS THAT THEY HAVE TO READ THE NEWS

== tutorial_check == 
¿Quieres hacer el tutorial? #parrafo
* [Sí.] -> Introduction
* [No.] -> scene_0_intro

== Introduction ==
En este juego vas a recibir artículos de diversas fuentes (periodísticos, publicaciones de blogs y de redes sociales. #parrafo
Vamos a ver cómo llegarán los artículos. Primero verás el titular. #parrafo

LA UNIÓN EUROPEA REQUIERE QUE PAÍSES MIEMBROS RECORTEN LAS PENSIONES UN 30-40%

+ [Leer el artículo.] -> article
+ [Saltarse el artículo.] -> opinion

== article ==
Fuente: Redes Sociales.

Atención los que cobramos una pensión fruto de nuestro esfuerzo y cotizaciones a la seguridad social durante muchísimos años. Con esta economía hundida y este desgobierno que hemos elegido y ellos pactado, ante la desconfianza de la U.E. les están exigiendo condiciones y garantías para el préstamo/rescate y entre otras han ofrecido rebajas salariales a todos los Funcionarios Públicos (menos ellos, claro) y a los PENSIONISTAS sobre todo. Se empieza a rumorear (para ir preparándonos el cuerpo) que la rebaja estará entre un 30 o 40 % (igual que en Grecia). No lo podemos permitir y tenemos que pararlo antes de que sea tarde.. Después de toda la vida trabajando y cotizando como desgraciados para que ahora nos vengan con la puntilla. Pasadlo a todos que se sepa y que lo paremos.REENVIAR TODOS LOS PENSIONISTAS O FUTUROS. 
-> opinion

== opinion ==
...
¿Qué te parece?

+ [Me parece una basura manipuldora.] -> explanation
+ [Pues es muy bueno saber esto.] -> explanation
+ [No sé qué pensar.]-> explanation

== explanation ==
{article: ¡Has leído el artículo, muy bien! Parece que ya tienes algunos buenos hábitos}{not article:¿Cómo puedes opinar sobre algo que no has leído? ¡Ay, ay, ay!} #parrafo
Esto es un bulo, que también se denomina con el término inglés "fake news". Como puedes ver, no aparecen nombres de personas específicas, instituciones o fuentes. El tono es incendiario para que despertar la indignación del lector. #parrafo
No es un artículo contrastado ni está escrito de manera profesional (aunque a veces puede haber artículos contrastados que no estén muy bien escritos). El propósito de este texto es enfadar al lector para conseguir una respuesta emocional, y que se extienda el artículo para así que haya más gente indignada. #parrafo

Vamos a ver otra noticia. #parrafo
-> scene_0_intro

