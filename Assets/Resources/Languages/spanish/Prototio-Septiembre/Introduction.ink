// This is an example of how the game would start 

== tutorial_check == 
-> scene_0_intro
¿Quieres hacer el tutorial? 
* [Sí.] -> Introduction
* [No.] -> scene_0_intro

== Introduction ==
En este juego vas a tratar con diferentes grupos de gente, quienes te mandarán artículos periodísticos, publicaciones de blogs y de redes sociales. Puedes compartir con ellos los artículos que te llegan (¡o no!) y ver cómo reaccionan a lo que les envías. 
Cada grupo responderá a los artículos de manera diferente. A unos les interesará lo que les envías, mientras que a otros no les gustará o estarán en desacuerdo con lo que diga el artículo. ¿Puedes identificar qué les hace reaccionar así a cada noticia? ¿De dónde salen sus respuestas?
¿Quieres saber más de estos grupos?

* [Sí.]-> explanation_groups
* [No, ya los conozco.] -> article_tutorial

== explanation_groups ==

* [Amigos] -> explain_g1
* [Familia] -> explain_g2
* [Vecinos] -> explain_g3
* -> article_tutorial // this is not an option, it just means that once all the groups have been described the game will move to the next knot. 

== explain_g1 ==
El grupo de amigo representa a gente con la que tienes vínculos de amistad en diferentes sitios. Pueden ser el grupo a quien siempre ves en el bar, la peluquería, o la parroquia, por poner un ejemplo. También es gente a la que conoces desde hace muchos años (compañeros de trabajo, miembros de la peña de fútbol). Este grupo tiende a tener ideas fijas y opiniones ya formadas, y parece que están un poco enfadados con el mundo. 
/*They believe news about disinformation. They do not like science. They believe conspiracy theories. They do not like computers and artificial intelligence . 
They often fall from scams*/
-> explanation_groups
== explain_g2 ==
La famila en este juego van a ser hijos y nietos. Los nietos son quienes te han enseñado a usar el móvil y a mandar mensajes, así que se les da bien eso de la tecnología. Por ser más jóvenes, también parecen estar al día con lo que pasa, con qué noticias pueden ser bulos y cuáles puede que estén contrastadas. 
/* This is group 2
They are very critical of things that sound like disinformation. They believe in science. They get angry at conspiracy theories. They are very skeptical about artificial intelligence. They dislike anything that sounds like a scams.*/-> explanation_groups
== explain_g3 ==
Los vecinos son gente a la que conoces desde hace muchos, muchos años. Algunos vecinos tienen hijos que han crecido con los tuyos. Otros son más jóvenes y conocen menos el vecindario, y hay un par de emigrantes también. Es un grupo variado, en el que hay gente a la que conocies bien porque os habéis ayudado a través de los años, y hay otros a quienes no soportas. Pero todos habéis aprendido a convivir. 
/*This is group 3. They often fall for disinformation. They like science, but it is scary. They believe conspiracy theories. They think artificial intelligence is scary. They can fall from scams. */
-> explanation_groups


== article_tutorial ==
Vamos a aprender cómo se interactúa con cada grupo. Abajo hay un ejemplo de lo que verás cuando te llegue un artículo. Primero verás el titular. 

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
{article: ¡Has leído el artículo, muy bien!}{not article:¿Cómo puedes opinar sobre algo que no has leído?}
Esto es un bulo, que también se denomina con el término inglés "fake news". Como puedes ver, no aparecen nombres de personas específicas, instituciones o fuentes. El tono es incendiario para que despertar la indignación del lector. Esto no es un artículo contrastado, y no está escrito de manera profesional (aunque a veces puede haber artículos contrastados que no estén muy bien escritos). El propósito de este texto es enfadar al lector para conseguir una respuesta emocional, y que se extienda el artículo para así que haya más gente indignada. 
Muchos de estos artículos se extienden a través de redes sociales y blogs, así que también habrás de prestar atención a la fuente de los artículos. 
Tú decides si quieres compartir el artículo o no. Si lo compartes, observa cómo caca grupo reacciona al artículo. Hay quienes sólo quieren recibir noticias que confirmen cómo se sienten, sin importar si están contrastadas o no. Otros puede que se den cuenta de que el artículo es un timo, mientras que hay otros que no y sean estafados. 
La decisión es tuya de si leer el artículo o no, o si mandarlo a otros o no, y a quién enviarlo. Compartir noticias es una matera de reforzar lazos sociales, o de conseguir que otros desconfíen de ti. 
-> scene_0_intro

