=== scene_4_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art4
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Artículo número {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTÍCULO RECIBIDO
Titular: {article_data_HC (print_article, headline)}

+ [Leer el artículo.] -> s4_article
+ [Saltarse el artículo.] -> s4_choice

== s4_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s4_choice

== s4_choice == 
* [Compartir con el grupo de amigos.] 
~ article_forwarded_group1++
~ article_sent = true

-> s4_g1
    
* [Compartir con familia.] 
~ article_forwarded_group2++
~ article_sent = true

 -> s4_g2
* [Compartir con el grupo de vecinos.] 
~ article_forwarded_group3++
~ article_sent = true

  -> s4_g3
* [No compartir con nadie {article_sent: más}.] -> s4_results

== s4_g1 ==
Enviado a grupo de amigos
~ group_1_opinion(theme, checked)
Manu: Ya me lo habían dicho. No está de más tener unas pocas dosis a mano.
Marcos: No tenía ni idea de que fuera tan fácil de conseguir. Voy a pedir un bote online {s2_g1:, a ver si se me pasa este virus que no me deja en paz}. 
-> s4_choice

== s4_g2 ==
Enviado a familia 
~ group_2_opinion(theme, checked)
Felisa: ¡Esto no es verdad! Por favor, no compartas estos bulos, que pueden hacer daño a alguien. 
Alfredo: El Ivermectin es peligroso tomarlo sin receta médica.   
{scold1_group2()}
-> s4_choice

== s4_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
Paula : ¿Pero esto lo tienen en la farmacia?
Emma: No, esto es para animales, no para personas. Ni se te ocurra. 
 {scold4_group3()}
-> s4_choice



== s4_results == 
Relación con amigos: {group_1}
Relación con familia: {group_2}
Relación con vecinos: {group_3}

-> checkin_2