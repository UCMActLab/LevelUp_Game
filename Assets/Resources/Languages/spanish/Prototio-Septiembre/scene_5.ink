=== scene_5_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art5
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Artículo número {news_count} 

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
Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}


-> game_ending
