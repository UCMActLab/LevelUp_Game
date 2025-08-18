=== scene_3b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
Artículo número {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data (print_article, headline)}

+ [Leer el artículo.] -> s3b_article
+ [Saltarse el artículo.] -> s3b_choice

== s3b_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s3b_choice

== s3b_choice == 
* [Compartir con el grupo de amigos.] 
~ group_1_opinion(theme, checked)
~ article_forwarded_group1++
~ article_sent = true
-> s3b_g1

* [Compartir con familia.] 
~ group_2_opinion(theme, checked)
~ article_forwarded_group2++
~ article_sent = true


-> s3b_g2
* [Compartir con el grupo de vecinos.] 
~ group_3_opinion(theme, checked)
~ article_forwarded_group3++
~ article_sent = true


-> s3b_g3
* [No compartir con nadie {article_sent: más}.] -> s3b_results

== s3b_g1 ==
Enviado a grupo de amigos

TODO FRIENDS REACTIONS NEEDED
-> s3b_choice
    
== s3b_g2 ==
Enviado a familia -> s3b_choice
{scold1_group2()}
== s3b_g3 == 
Enviado a grupo de vecinos 
TODO NEIGHBOR REACTIONS NEEDED
-> s3b_choice
    
== s3b_results == 
Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}

-> scene_4_intro
