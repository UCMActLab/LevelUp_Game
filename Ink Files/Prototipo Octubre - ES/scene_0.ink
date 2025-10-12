TODO REWRITE TO REINFORCE THAT PLAYERS HAVE TO READ THE NEWS

=== scene_0_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)

Titular: {article_data (print_article, headline)}

+ [Leer el artículo.] -> s0_article
+ [Saltarse el artículo.] -> s0_results

== s0_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s0_results

== s0_results == 
{s0_article: Muy bien. Lo primero que tenemos que hacer para estar informados es leer la noticia, y entonces podemos empezar a evaluar si está contrastada o no. }{not s0_article:¿En qué habíamos quedado? No leer los artículos es la base de cómo se extiende la desinformación. } #parrafo
Enviar artículos falsos o fake news puede tener consecuencias nefastas. #parrafo
Cada vez que recibas un artículo, puedes compartirlo con diferentes grupos - ¡o no! - para no contribuir a que se extiendan artículos sin contrastar. #parrafo
Cada grupo responderá a los artículos de manera diferente. A unos les interesará lo que les envías, mientras que a otros no les gustará o estarán en desacuerdo con lo que diga el artículo. ¿Puedes identificar qué les hace reaccionar así a cada noticia? #parrafo
¿Quieres saber más de estos grupos? #parrafo

* [Sí.]-> explanation_groups
* [No, ya los conozco.] -> moving_on

== explanation_groups ==

* [Amigos] -> explain_g1
* [Familia] -> explain_g2
* [Vecinos] -> explain_g3
* -> moving_on // this is not an option, it just means that once all the groups have been described the game will move to the next knot. 

== explain_g1 ==
El grupo de amigos son gente con la que tienes vínculos de amistad. Pueden ser el aquellos a quien siempre ves en el bar, la peluquería, o la parroquia, por ejemplo. También es gente a la que conoces desde hace muchos años como compañeros de trabajo o miembros de la peña de fútbol. Este grupo tiende a tener ideas fijas y opiniones ya formadas, y parece que están un poco enfadados con el mundo. #parrafo
/*They believe news about disinformation. They do not like science. They believe conspiracy theories. They do not like computers and artificial intelligence . 
They often fall from scams*/
-> explanation_groups
== explain_g2 ==
La famila son los hijos y nietos. Los nietos son quienes te han enseñado a usar el móvil y mandar mensajes, porque se les da bien la tecnología. Tu familia parece estar al día con lo que pasa, y se les da bien identificar qué noticias pueden ser bulos y cuáles están contrastadas. #parrafo
/* This is group 2
They are very critical of things that sound like disinformation. They believe in science. They get angry at conspiracy theories. They are very skeptical about artificial intelligence. They dislike anything that sounds like a scams.*/-> explanation_groups
== explain_g3 ==
Los vecinos son gente a la que conoces desde hace muchos, muchos años. Algunos tienen hijos que han crecido con los tuyos. Otros son más jóvenes y conocen menos el vecindario; también hay algunos emigrantes. Es un grupo variado, con gente a la que conoces bien después de mucho tiempo, y con otros a quienes no soportas. Pero todos habéis aprendido a convivir. Pueden tener reacciones muy diferentes a las noticias. #parrafo
/*This is group 3. They often fall for disinformation. They like science, but it is scary. They believe conspiracy theories. They think artificial intelligence is scary. They can fall from scams. */
-> explanation_groups

== moving_on ==
Vamos a ver cómo compartir (o no) las noticias. 

-> scene_1_intro
