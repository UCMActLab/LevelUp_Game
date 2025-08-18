=== scene_3_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art3
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Artículo número {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data_HC (print_article, headline)}

+ [Leer el artículo.] -> s3_article
+ [Saltarse el artículo.] -> s3_choice

== s3_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s3_choice

== s3_choice == 
* [Compartir con el grupo de amigos.] 
~ article_forwarded_group1++
~ article_sent = true

 -> s3_g1
    
* [Compartir con familia.] 
~ article_forwarded_group2++
~ article_sent = true

 -> s3_g2
    
* [Compartir con el grupo de vecinos.] 
~ article_sent = true

    -> s3_g3
    
* [No compartir con nadie {article_sent: más}.] -> s3_results

== s3_g1 ==
Enviado a grupo de amigos 
~ group_1_opinion(theme, checked)
Manu: ¡Nos están invadiendo!
María: Lo que van a conseguir es atraer a más extranjeros. -> s3_choice
    
== s3_g2 ==
Enviado a familia 
~ group_2_opinion(theme, checked)
Felisa: Muy bien, me alegra ver que estás pendiente de los periódicos. 
-> s3_choice

== s3_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
 Omar: Esta información es muy útil, gracias. Se lo voy a pasar a unos colegas del trabajo. 
 Paula: (Este mensaje ha sido borrado por el usuario)
 Emma: Esperemos que esto sea para bien.

 -> s3_choice

== s3_results == 

Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}


-> scene_3b_intro
