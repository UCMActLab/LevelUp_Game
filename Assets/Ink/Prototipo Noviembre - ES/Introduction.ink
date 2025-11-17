// This is an example of how the game would start 

TODO THIS SCENE IS THE INTRODUCTION TO THE GAME. THE GOAL OF THIS SCENE IS TO TEACH PLAYERS THAT THEY HAVE TO READ THE NEWS

//== tutorial_check == 
//¿Quieres hacer el tutorial?#parrafo
//* [Sí.] -> Introduction
//* [No.] -> scene_0_intro

== Introduction ==
Gracias por participar en nuestro programa de entrenamiento para la brigada F.A.B.U.S. (Fuerza Anti Bulos Unida y Sabia) #parrafo

Nuestro objetivo es preparar a grupos selectos para combatir la desinformación. Tus años y experiencia te convierten en el tipo de persona ideal para llevar a cabo esta misión. Los abuelos y abuelas son fuente de conocimiento, y siempre están dispuestos para ayudar a sus allegados y personas queridas en los momentos difíciles. #parrafo

Estamos en un momento de crisis, y necesitamos tu ayuda. Vivimos en tiempos de división y desacuerdo, y uno de los factores principales es la desinformación, que se extiende como la pólvora gracias a las nuevas tecnologías, como los teléfonos móviles con conexiones a internet.   #parrafo

Tu sabiduría puede ayudar a parar la desinformación. Lo que ofrecemos a través de F.A.B.U.S. es preparar a nuestros miembros para ayudar a mantener a la población bien informada, con artículos contrastados y actuales. #parrafo

Tu misión es procesar los artículos que recibas, y evaluarlos para ver si son información veraz o no. 

Vamos a empezar con el entrenamiento. Aquí tienes un titular. #parrafo

LA UNIÓN EUROPEA REQUIERE QUE PAÍSES MIEMBROS RECORTEN LAS PENSIONES UN 30-40%

+ [Leer el artículo.] -> article
+ [Saltarse el artículo.] -> opinion

== article ==
Fuente: Redes Sociales.

Atención los que cobramos una pensión fruto de nuestro esfuerzo y cotizaciones a la seguridad social durante muchísimos años. Con esta economía hundida y este desgobierno que hemos elegido y ellos pactado, ante la desconfianza de la U.E. les están exigiendo condiciones y garantías para el préstamo/rescate y entre otras han ofrecido rebajas salariales a todos los Funcionarios Públicos (menos ellos, claro) y a los PENSIONISTAS sobre todo. Se empieza a rumorear (para ir preparándonos el cuerpo) que la rebaja estará entre un 30 o 40 % (igual que en Grecia). No lo podemos permitir y tenemos que pararlo antes de que sea tarde. Después de toda la vida trabajando y cotizando como desgraciados para que ahora nos vengan con la puntilla. Pasadlo a todos que se sepa y que lo paremos. REENVIAR TODOS LOS PENSIONISTAS O FUTUROS. 
-> opinion

== opinion ==
¿Qué te parece?

+ [Me parece una basura manipuldora.] -> explanation
+ [Pues es muy bueno saber esto.] -> explanation
+ [No sé qué pensar.]-> explanation

== explanation ==
{article: ¡Bien hecho, recluta! Has leído el artículo, como debe ser. Parece que ya tienes algunos buenos hábitos}{not article: A ver, recluta, ¿cómo puedes opinar sobre algo que no has leído? Vamos a parar un momento y leer el artículo que te has saltado.} #parrafo

{not article: Atención los que cobramos una pensión fruto de nuestro esfuerzo y cotizaciones a la seguridad social durante muchísimos años. Con esta economía hundida y este desgobierno que hemos elegido y ellos pactado, ante la desconfianza de la U.E. les están exigiendo condiciones y garantías para el préstamo/rescate y entre otras han ofrecido rebajas salariales a todos los Funcionarios Públicos (menos ellos, claro) y a los PENSIONISTAS sobre todo. Se empieza a rumorear (para ir preparándonos el cuerpo) que la rebaja estará entre un 30 o 40 % (igual que en Grecia). No lo podemos permitir y tenemos que pararlo antes de que sea tarde. Después de toda la vida trabajando y cotizando como desgraciados para que ahora nos vengan con la puntilla. Pasadlo a todos que se sepa y que lo paremos. REENVIAR TODOS LOS PENSIONISTAS O FUTUROS. }

Esto es un bulo, que también se denomina con el término inglés "fake news". Como puedes ver, no aparecen nombres de personas específicas, instituciones o fuentes. El tono es incendiario para que despertar la indignación del lector. Recuerda, mucha de la desinformación está escrita para dividir a la problación. #parrafo
No es un artículo contrastado ni está escrito de manera profesional, aunque a veces puede haber artículos contrastados que no estén muy bien escritos, y artículos bien escritos que se inventan hechos. El propósito de este texto es enfadar al lector para conseguir una respuesta emocional, y que se extienda el artículo para así que haya más gente indignada. #parrafo

Vamos a ver otra noticia. #parrafo
-> scene_0_intro

