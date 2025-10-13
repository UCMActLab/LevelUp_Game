
=== scene_1_intro ===
~ news_count++
~ article_sent = false

TODO The line is the line that selects the article from the database. This should hook into the external database. 

~ temp print_article = HC_art1 // this is the line where I'm selecting a specific article from the hardcoded database
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
~ comes_from = article_data_HC (print_article, sources)

ARTÍCULO RECIBIDO
Titular: {article_data_HC (print_article, headline)}
TODO: These values need to be taken from the database - this will be tags for each piece of news. The database also needs at least a headline and a text body. This code will be the same in each scene. 

+ [Leer el artículo.] -> s1_article
+ [Saltarse el artículo.] -> s1_choice

=== s1_article ===
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC(print_article, body)} -> s1_choice

== s1_choice == 
TODO:  Still need to fix the last option so the game moves to the next knot when all options have been exhausted. I fixed the last option, so it checks whether the article has been sent already or not and displays the right text, but this may have issues when translating the game to other languages -- it's not good practice for localization. 

¿Qué quieres hacer con esta noticia? #parrafo

* [Compartir con el grupo de amigos.] 
    ~ article_forwarded_group1++
    ~ article_sent = true
   -> s1_g1

* [Compartir con familia.] 
    ~ article_forwarded_group2++
    ~ article_sent = true

    -> s1_g2
    
* [Compartir con el grupo de vecinos.] 
    ~ article_forwarded_group3++
    ~ article_sent = true

    -> s1_g3
* [No compartir con nadie {article_sent: más}.] -> s1_results
    
== s1_g1 ==
Enviado a grupo de amigos 
~ group_1_opinion(theme, checked)
María: Ya me lo había mandado Juan. Creo que vamos a pedir algo, a ver si cuela. 

    -> s1_choice
    
== s1_g2 ==
Enviado a familia 
    ~ group_2_opinion(theme, checked)
Alfredo: Esto es un timo, por favor no caigas en ello. Sólo quieren tus datos. El gobierno no ofrece subvenciones así. 
     -> s1_choice

== s1_g3 == 
Enviado a grupo de vecinos
    ~ group_3_opinion(theme, checked)
Paula: ¡Gracias! Se nos echó a perder la comida del congelador del bar, este dinero nos vendría muy bien. 
    -> s1_choice

== s1_results == 
Has recibido una noticia falsa {article_sent == true: y la has compartido. Probablemente tendrá consecuencias después.}{article_sent == false: pero no la has compartido. Está bien ser prudente.} {article_sent == true && s1_article: Además la has leído antes de enviarla; recuerda que hay que prestar atención a cómo está escrito el texto para tener una idea de si la noticia está contrastada o no.}{article_sent == true && not s1_article: Compartir una noticia sin leerla es una de las razones por las que las fake news se extienden tan rápido.}{article_sent == false && not s1_article: En este caso has evitado extender información falsa, lo que no quiere decir que es mejor enviar noticias, sino que hay que leer lo que nos llega con cuidado.} {article_sent == false && s1_article: Has leído la noticia, así que probablemente hayas visto que era fake news y es mejor no compartirla. ¡Bien!}#parrafo

-> scene_1b_intro
