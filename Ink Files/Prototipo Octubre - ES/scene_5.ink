TODO Reinfoce all the points.  

=== scene_5_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art5
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data_HC (print_article, headline)}

+ [Leer el artículo.] -> s5_article
+ [Saltarse el artículo.] -> s5_choice

== s5_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s5_choice

== s5_choice == 

¿Qué quieres hacer con esta noticia? #parrafo
* [Compartir con el grupo de amigos.] 
~ article_forwarded_group1++
~ article_sent = true

  -> s5_g1
* [Compartir con familia.] 
~ article_forwarded_group2++
~ article_sent = true

   -> s5_g2
* [Compartir con el grupo de vecinos.] 
~ article_forwarded_group3++
~ article_sent = true

   -> s5_g3
* [No compartir con nadie {article_sent: más}.] -> s5_results

== s5_g1 ==
Enviado a grupo de amigos 
~ group_1_opinion (theme, checked)
  María: ¿Veis? Ya decía yo que eso era una tontería. 
  Manu: Ya lo decía yo, lo del cambio climático es un invento de los comunistas. 
  -> s5_choice
    
== s5_g2 ==
Enviado a familia
~ group_2_opinion (theme, checked)
Alfredo: ¿Te has creído esto? Suena a tongo. 
Fede: Esto no es verdad, ya lo he leído. Han encontrado un fragmento de tierra compactada que parece ser de antes de la época de los romanos, pero no es una calzada. Esto es un ejemplo de desinformación.
Felisa: No te creas todo lo que te llegue por redes sociales, por favor. 
-> s5_choice

== s5_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion (theme, checked)
Paula: Ay, no sé qué pensar.
Emma: ¡Mira tú que cosas!
-> s5_choice

== s5_results == 
Has recibido una noticia falsa proveniente de un blog. {article_sent == true && s5_article: La has compartido después de leerla, pero parece que no te has percatado de que es una teoría de la conspiración.}{article_sent == true && not s5_article: La has compartido sin leerla, que es el principal problema que hace que se extienda la desinformación.}{article_sent == false && not s5_article: No la has compartido, con lo que no se extiende la desinformación, pero tampoco has mirado qué ponía. Deberías echar un vistazo a las noticias para ir acostumbrándote a ver la diferencia entre las noticias contrastadas y las fake news.} {article_sent == false && s5_article: La has leído primero, y has decidido no enviarla, que es como debería ser.}#parrafo 
-> game_ending
