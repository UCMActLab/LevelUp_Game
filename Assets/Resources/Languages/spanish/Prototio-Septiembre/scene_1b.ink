=== scene_1b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
Artículo número {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data (print_article, headline)}

+ [Leer el artículo.] -> s1b_article
+ [Saltarse el artículo.] -> s1b_choice

== s1b_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s1b_choice

== s1b_choice == 
* [Compartir con el grupo de amigos.] 
~ article_forwarded_group1++
~ article_sent = true
-> s1b_g1

* [Compartir con familia.] 
~ article_forwarded_group2++
~ article_sent = true


-> s1b_g2
* [Compartir con el grupo de vecinos.] 
~ article_forwarded_group3++
~ article_sent = true


-> s1b_g3
* [No compartir con nadie {article_sent: más}.]. -> s1b_results

== s1b_g1 ==
Enviado a grupo de amigos 
TODO FRIENDS REACTIONS NEEDED

~ group_1_opinion(theme, checked)
-> s1b_choice
    
== s1b_g2 ==
Enviado a familia
~ group_2_opinion(theme, checked)
{scold1_group2()}
-> s1b_choice
== s1b_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
TODO NEIGHBOR REACTIONS NEEDED

-> s1b_choice
    
== s1b_results == 
Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}

-> scene_2_intro
