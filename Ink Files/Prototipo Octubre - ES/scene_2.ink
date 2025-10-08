TODO TEACH USERS TO CHECK THE SOURCE

=== scene_2_intro ===
Una manera de detectar si un artículo es una noticia está sin contrastar o no es comprobar la fuente. #parrafo
Las noticias que te llegan pueden ser artículos periodísticos, publicaciones de blogs y de redes sociales. #parrafo
Las noticias que provienen de periódicos suelen tener más rigor, mientras que muchos de los bulos, timos y teorías de la conspiración se extienden a través de blogs e imágenes en redes sociales. #parrafo

~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art2
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC(print_article, verified)
Artículo número {news_count} 

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
{s2_article: ¡Bien por leerte el artículo!}{not s2_article: No te estás leyendo los artículos, ¿eh?}
Antes de que sigamos, ¿cuál es la fuente del artículo? #parrafo
+ [Periódico] -> s2_feedback_good
+ [Blog] -> s2_feedback_bad
+ [Redes sociales] -> s2_feedback_bad

== s2_feedback_good == 
{s2_article: Sí, es una noticia de los periódicos. ¡Te has fijado!}{not s2_article: Has acertado de casualidad, porque te has saltado el artículo.}#parrafo
-> s2_feedback_coda

== s2_feedback_bad == 
{s2_article: Pues no es así. Has leído el artículo, pero parece que se te ha pasado mirar la fuente.}{not s2_article: Es lo que pasa por no leerse las cosas, que es difícil acertar.}#parrafo
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
Alfredo: Muy preocupante. Por favor, no te olvides de llevar la mascarilla. -> s2_choice
{scold2_group2()}
== s2_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
Paula: ¿Otra vez? Esto no se acaba nunca...
Omar: Todavía me quedan máscarillas en casa. Avisad si os hacen falta. 
-> s2_choice
    
== s2_results == 

Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}

-> checkin_1
