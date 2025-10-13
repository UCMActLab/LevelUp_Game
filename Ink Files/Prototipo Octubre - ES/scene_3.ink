TODO REINFORCE THAT PLAYERS NEED TO CHECK THE SOURCE
=== scene_3_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art3
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)

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

¿Qué quieres hacer con esta noticia? #parrafo
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
Has recibido una noticia contrastada proveniente de un periódico, {article_sent == true && s3_article: y la has compartido después de leerla. Has comprobado la noticia antes de enviarla, ¡muy bien!}{article_sent == true && not s3_article: y la has compartido sin leerla. Aunque en este caso es de una fuente fiable, es conveniente el echar un vistazo para mantenerse al día.}{article_sent == false && not s3_article: Abstenerse de enviar o leer nada puede prevenir que se extiendan las fake news, pero no conocer la actualidad es en cierta manera un tipo de desinformación.}{article_sent == false && s3_article: No tenemos por qué compartir todas las noticias contrastadas que leemos, aunque puede ser una manera de mantener nuestras relaciones sociales.} #parrafo 

-> scene_3b_intro
