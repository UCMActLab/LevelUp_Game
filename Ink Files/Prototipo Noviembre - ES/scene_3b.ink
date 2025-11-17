TODO: REINFORCE CORE POINTS (READ NEWS, CHECK SOURCES, CONSEQUENCES)

=== scene_3b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

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

¿Qué quieres hacer con esta noticia? #parrafo

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
~ group_1_opinion(theme, checked)

-> s3b_choice
    
== s3b_g2 ==
Enviado a familia -> s3b_choice

== s3b_g3 == 
Enviado a grupo de vecinos 
~ group_3_opinion(theme, checked)

-> s3b_choice
    
== s3b_results == 
Has recibido una noticia verdadera proveniente de {comes_from == "social": redes sociales}{comes_from == "blog": un blog}{comes_from == "news": un periódico}, {article_sent == true && s3b_article: y la has compartido después de leerla. Está bien que ayudes a otros a estar al día después de informarte tú.}{article_sent == true && not s3b_article: y la has compartido sin leerla. Vale que quieras ayudar a los demás a estar al día, pero hay que leer las noticias también.}{article_sent == false && not s3b_article: No enviar noticias es una manera de no extender la desinformación, pero también hay que prestar algo de interés por lo que pasa en el día a día.}{article_sent == false && s1b_article && checked == false: Está bien que no hayas compartido estas fake news, te habrás dado cuenta.}{article_sent == false && s1b_article && checked == true: No pasa nada por no compartir todos los artículos, aunque también puede ser una manera de conectar con los demás y que todos estemos al día con información relevante.}#parrafo 
-> scene_4_intro
