TODO TEACH USERS TO CHECK THE SOURCE

=== scene_2_intro ===
Vamos a la siguiente fase del entrenamiento. #parrafo
Una parte esencial en el combate contra la desinformación es comprobar la fuente de los artículos. #parrafo
Las noticias que recibirás pueden ser artículos periodísticos, publicaciones de blogs y de redes sociales. #parrafo
Las noticias provinientes de periódicos suelen tener más rigor, aunque también pueden tener sesgos ideológicos, mientras que muchos de los bulos, timos y teorías de la conspiración se extienden a través de blogs e imágenes en redes sociales. #parrafo

~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art2
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC(print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data_HC (print_article, headline)}

+ [Leer el artículo.] -> s2_article
+ [Saltarse el artículo.] -> s2_check

== s2_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)} -> s2_check

== s2_check ==
{s2_article: ¡Bien por leerte el artículo, recluta!}{not s2_article: Recluta, lee los artículos, por favor.}
Vamos a ponerte a prueba, ¿cuál es la fuente del artículo? #parrafo
+ [Periódico] -> s2_feedback_good
+ [Blog] -> s2_feedback_bad
+ [Redes sociales] -> s2_feedback_bad
+ [No lo sé.] -> s2_feedback_bad

== s2_feedback_good == 
{s2_article: Sí, es una noticia de los periódicos. Tu capacidad de percepción es excelente.}{not s2_article: Has acertado de casualidad, porque te has saltado el artículo.}#parrafo
-> s2_feedback_coda

== s2_feedback_bad == 
{s2_article: No es así. Has leído el artículo, pero parece que se te ha pasado mirar la fuente.}{not s2_article: Recluta, si no te lees los artículos no puedes luchar contra la desinformación.}#parrafo
-> s2_feedback_coda

== s2_feedback_coda == 
Esta noticia viene de un periódico, donde se suelen contrastar las fuentes y se sigue un manual de estilo. Aquí por ejemplo podemos ver cómo se nombran organismos y se cita el nombre de quien hace las declaraciones, mientras que muchas fake news tienden a omitir nombres de instituciones o de quién proporciona la información. #parrafo
-> s2_choice

== s2_choice == 

¿Qué quieres hacer con esta noticia? #parrafo
* [Compartir con el grupo de amigos.] 
~ article_sent = true
-> s2_g1

* [Compartir con familia.] 
~ article_sent = true


-> s2_g2
* [Compartir con el grupo de vecinos.] 
~ article_forwarded_group3++
~ article_sent = true

-> s2_g3
* [No compartir con nadie {article_sent: más}.] -> s2_results

== s2_g1 ==
Enviado a grupo de amigos 
~ group_1_opinion(theme, checked)
Marcos: ¡Es increíble! Quieren asustarnos otra vez. 
María: No me fío de estos de la OMS. A saber qué quieren hacer. 

-> s2_choice
    
== s2_g2 ==
Enviado a familia 
~ group_2_opinion(theme, checked)
Fede: Esto hay que tomárselo en serio. La OMS fue la primera organización que alertó del riesgo de pandemia en 2020. 
Alfredo: Muy preocupante. Por favor, no te olvides de llevar la mascarilla. 
-> s2_choice

== s2_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
Paula: ¿Otra vez? Esto no se acaba nunca...
Omar: Todavía me quedan máscarillas en casa. Avisad si os hacen falta. 
-> s2_choice
    
== s2_results == 
Has recibido una noticia verdadera proveniente de un periódico, {article_sent == true && s2_article: y la has compartido después de leerla. Así ayudas a los a estar al corriente después de informarte tú.}{article_sent == true && not s2_article: y la has compartido sin leerla. Reenviar artículos nos ayuda a conectar con otros, pero es importante el mirar la calidad de lo que mandamos.}{article_sent == false && not s2_article: Si no reenvias cosas evitas extender la desinformación, pero no te olvides de intentar seguir la actualidad.} {article_sent == false && s2_article: La noticia viene de una fuente fiable, así que al menos estás al día aunque no la hayas compartido. }#parrafo 

-> checkin_1
