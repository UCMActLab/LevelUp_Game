TODO TEACH PLAYERS TO LOOK FOR FURTHER INFORMATION

=== scene_4_intro ===

~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art4
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

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

¿Qué quieres hacer con esta noticia? #parrafo
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
-> s4_choice

== s4_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)
Paula : ¿Pero esto lo tienen en la farmacia?
Emma: No, esto es para animales, no para personas. Ni se te ocurra. 
-> s4_choice

== s4_results == 
Has recibido una noticia falsa proveniente de redes sociales, que además difunde bulos que pueden tener serias consecuencias para la salud. {article_sent == false && s4_article: Has mirado primero lo que decía y has decidido no compartir con nadie, que es lo correcto.} {article_sent == true && s4_article: La has compartido después de leerla, ¿no te has fijado en cómo está escrita y de dónde viene?}{article_sent == true && not s4_article: Además la has compartido sin leerla, que es más grave.}{article_sent == false && not s4_article: En este caso has evitado extender información perjudicial, pero no te olvides de leer lo que te llega para ir distinguiendo la información fiable de la que no es.} #parrafo 

-> checkin_2