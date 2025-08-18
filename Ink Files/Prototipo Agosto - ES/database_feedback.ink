// Below is a set of functions and the database that will generate feedback to the player after they send a piece of news. Each group will have positive or negative reactions to the news.

/* The functions are below 
{feedback_group1_positive()}
{feedback_group1_negative()}
{feedback_group2_positive()}
{feedback_group2_negative()}
{feedback_group3_positive()}
{feedback_group3_negative()}
*/

// the lists below are the names of the characters divided by group

LIST group1_members = María, Marcos, Manu
LIST group2_members = Alfredo, Fede, Felisa
LIST group3_members = Paula, Omar, Emma

=== function feedback_group1_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g1_pos)) //this selects one of the corresponding feedback lines at random. 
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members)) // this selects one of the names of the people that belong to the corresponding group at random. 
{group1_speaker}: {feedback_output_group1_positive (print_feedback)}


LIST fb_lines_g1_pos = fb1_g1_pos, fb2_g1_pos, fb3_g1_pos, fb4_g1_pos, fb5_g1_pos, fb6_g1_pos, fb7_g1_pos, fb8_g1_pos, fb9_g1_pos, fb10_g1_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group1_positive (feedback_lines_g1) // this is the feedback database that has the positive responses for group 1
{feedback_lines_g1:
    - fb1_g1_pos: Ah, esto es lo que quiero ver. 
    - fb2_g1_pos: Ya lo había visto, pero gracias de todas maneras. 
    - fb3_g1_pos: Así me gusta, gente que piensa como yo. 
    - fb4_g1_pos: Eso, la verdad que se sepa. 
    - fb5_g1_pos: Esto confirma lo que ya sospechaba. 
    - fb6_g1_pos: Gracias por enviar esto, mándanos más. 
    - fb7_g1_pos: No sabía de esto, pero está bien estar informado. 
    - fb8_g1_pos: Gracias por compartir, lo tendré en cuenta. 
    - fb9_g1_pos: Gracias por mantenernos informados, da gusto contigo. 
    - fb10_g1_pos: Bien, me gusta que hay más gente que ve las cosas como yo. 
    - else: [No se encuentran {feedback_lines_g1} positivas para el grupo 1.]

    }

=== function feedback_group1_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g1_neg))
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
{group1_speaker}: {feedback_output_group1_negative (print_feedback)}


LIST fb_lines_g1_neg = fb1_g1_neg, fb2_g1_neg, fb3_g1_neg, fb4_g1_neg, fb5_g1_neg, fb6_g1_neg, fb7_g1_neg, fb8_g1_neg, fb9_g1_neg, fb10_g1_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group1_negative (feedback_lines_g1) // this is the feedback database that has the negative responses for group 1
{feedback_lines_g1:
    - fb1_g1_neg: No me lo creo. 
    - fb2_g1_neg: Esto es una basura. 
    - fb3_g1_neg: Mentira. Es mentira. No compartas esto.  
    - fb4_g1_neg: Estas noticias me molestan, no creo que sea verdad.  
    - fb5_g1_neg: No me gusta leer noticias de este tipo. 
    - fb6_g1_neg: Esto está mal. Yo no lo veo así.  
    - fb7_g1_neg: No nos mandes esto, no nos gusta. 
    - fb8_g1_neg: No estoy de acuerdo. Esto está mal. 
    - fb9_g1_neg: No, no, y no. 
    - fb10_g1_neg: Me desagrada leer estas cosas. 
    - else: [No se encuentran {feedback_lines_g1} negativas para el grupo 1.]

    }

=== function feedback_group2_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g2_pos))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
{group2_speaker}: {feedback_output_group2_positive(print_feedback)}


LIST fb_lines_g2_pos = fb1_g2_pos, fb2_g2_pos, fb3_g2_pos, fb4_g2_pos, fb5_g2_pos, fb6_g2_pos, fb7_g2_pos, fb8_g2_pos, fb9_g2_pos, fb10_g2_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group2_positive (feedback_lines_g2) // this is the feedback database that has the positive responses for group 2
{feedback_lines_g2:
    - fb1_g2_pos: Sí, gracias.  
    - fb2_g2_pos: Me alegra ver que estás al día con las noticias.
    - fb3_g2_pos: Lo había visto ya, pero no está mal que se siga compartiendo. 
    - fb4_g2_pos: Había visto algo parecido, gracias por recordármelo. 
    - fb5_g2_pos: Mis amigos lo habían mencionado, este artículo lo confirma. 
    - fb6_g2_pos: Gracias por compartir. Da gusto ver que te preocupas por las noticias. 
    - fb7_g2_pos: Genial, gracias. 
    - fb8_g2_pos: Gracias por mantenernos informados.  
    - fb9_g2_pos: Lo he leído ya, pero está bien que se siga difundiendo.
    - fb10_g2_pos: Gracias por la información. 
    - else: [No se encuentran {feedback_lines_g2} positivas para el grupo 2.]

    }
    
    
=== function feedback_group2_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g2_neg))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
{group2_speaker}: {feedback_output_group2_negative (print_feedback)}


LIST fb_lines_g2_neg = fb1_g2_neg, fb2_g2_neg, fb3_g2_neg, fb4_g2_neg, fb5_g2_neg, fb6_g2_neg, fb7_g2_neg, fb8_g2_neg, fb9_g2_neg, fb10_g2_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group2_negative (feedback_lines_g2) // this is the feedback database that has the negative responses for group 2
{feedback_lines_g2:
    - fb1_g2_neg: Nooo, no compartas esto, por favor. 
    - fb2_g2_neg: Creo que esto es un fake. No es verdad. 
    - fb3_g2_neg: Ya he recibido esto y creo que es mentira. 
    - fb4_g2_neg: Esta información es sesgada. Creo que no voy a leerlo. 
    - fb5_g2_neg: ¡Vaya trola de artículo! 
    - fb6_g2_neg: Esto es una tontería, no hace falta más que ver el titular. 
    - fb7_g2_neg: ¿De verdad lees estas cosas? Tienes que tener más cuidado. 
    - fb8_g2_neg: Se ha demostrado que esto es falso, por favor no lo compartas más. 
    - fb9_g2_neg: ¡Esto es pura manipulación!
    - fb10_g2_neg: Las cosas que se inventa la gente para que se hagan virales... No te lo creas. 
    - else: [No se encuentran {feedback_lines_g2} negativas para el grupo 2.]

    }
    
=== function feedback_group3_positive()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g3_pos))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))
{group3_speaker}: {feedback_output_group3_positive(print_feedback)}


LIST fb_lines_g3_pos = fb1_g3_pos, fb2_g3_pos, fb3_g3_pos, fb4_g3_pos, fb5_g3_pos, fb6_g3_pos, fb7_g3_pos, fb8_g3_pos, fb9_g3_pos, fb10_g3_pos //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group3_positive (feedback_lines_g3) // this is the feedback database that has the positive responses for group 3
{feedback_lines_g3:
    - fb1_g3_pos: ¡Muchas gracias!
    - fb2_g3_pos: ¡Qué interesante! Gracias.  
    - fb3_g3_pos: Anda, no sabía esto. 
    - fb4_g3_pos: Me lo guardo para luego. 
    - fb5_g3_pos: Mira, quería infomarme más de esto, gracias. 
    - fb6_g3_pos: Sí, algo había oído de esto. 
    - fb7_g3_pos: Gracias por compartir. 
    - fb8_g3_pos: Bien, gracias.  
    - fb9_g3_pos: ¡Mira qué cosas! Gracias.  
    - fb10_g3_pos: Esto da qué pensar, te lo agradezco. 
    - else: [No se encuentran {feedback_lines_g3} positivas para el grupo 3]

    }

=== function feedback_group3_negative()
~ temp print_feedback = LIST_RANDOM(LIST_ALL(fb_lines_g3_neg))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))
{group3_speaker}: {feedback_output_group3_negative (print_feedback)}


LIST fb_lines_g3_neg = fb1_g3_neg, fb2_g3_neg, fb3_g3_neg, fb4_g3_neg, fb5_g3_neg, fb6_g3_neg, fb7_g3_neg, fb8_g3_neg, fb9_g3_neg, fb10_g3_neg //this list is the number of items in the database. New entry in the database means adding an item here. 

=== function feedback_output_group3_negative (feedback_lines_g3) // this is the feedback database that has the negative responses for group 1
{feedback_lines_g3:
    - fb1_g3_neg: Estas noticias me dejan mal cuerpo. 
    - fb2_g3_neg: Este artículo no me gusta nada.  
    - fb3_g3_neg: ¿Por qué nos mandas esto? 
    - fb4_g3_neg: Ya no sé qué creerme y qué no.  
    - fb5_g3_neg: ¿En qué mundo vivimos?  
    - fb6_g3_neg: Esto es preocupante.  
    - fb7_g3_neg: ¡Qué miedo da todo!
    - fb8_g3_neg: Ya no sé qué pensar, de verdad. 
    - fb9_g3_neg: No me convence esto. 
    - fb10_g3_neg: No lo tengo muy claro. 
    - else: [No se encuentran {feedback_lines_g3} positivas para el grupo 3.]

    }

   
    