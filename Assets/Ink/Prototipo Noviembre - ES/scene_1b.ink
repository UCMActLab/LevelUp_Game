
TODO REINFORCE THAT SENDING FAKE NEWS HAS CONSEQUENCES

=== scene_1b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

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

¿Qué quieres hacer con esta noticia? #parrafo
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
~ group_1_opinion(theme, checked)

-> s1b_choice
    
== s1b_g2 ==
Enviado a familia
~ group_2_opinion(theme, checked)

-> s1b_choice

== s1b_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)

-> s1b_choice
    
== s1b_results == 
Has recibido una noticia {checked == true: contrastada.}{checked == false: falsa.} {article_sent == true && s1b_article: La has compartido después de leerla.}{article_sent == true && not s1b_article: La has compartido sin leerla.} {article_sent == true && s1b_article && checked == true: Compartir información contrastada también ayuda a los demás a estar informados.} {article_sent == true && not s1b_article && checked == false: Comprueba las noticias antes de reenviarlas, por favor.}{article_sent == true && s1b_article && checked == false: ¿Quieres que se extienda la desinformación?.} {article_sent == false && not s1b_article: No todos tenemos tiempo de leer todo lo que nos mandan, pero también es conveniente intentar estar un poco al día.}{article_sent == false && s1b_article && checked == false: Puede que hayas visto que esto son fake news, y por eso no la has reenviado. }{article_sent == false && s1b_article && checked == true: Bien por mantenerte al día con noticias rigurosas, aunque no las compartas. }

-> scene_2_intro
